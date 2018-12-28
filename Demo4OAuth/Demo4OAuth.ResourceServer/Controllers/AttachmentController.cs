
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Demo4OAuth.ResourceServer.Model;
using Demo4OAuth.ResourceServer.Service;
using Demo4OAuth.Tools;

namespace Demo4OAuth.ResourceServer.Controllers
{
    public class AttachmentController : ApiController
    {
        private IAttachmentService service;
        private NLog.Logger logger;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public AttachmentController(IAttachmentService service)
        {
            this.service = service;
            logger = NLog.LogManager.GetLogger(GetType().FullName);
        }

        /// <summary>
        /// 文件上传Api
        /// UploadFile(int startByte, bool complete, string resId, string dirName, string state = "")
        /// </summary>     
        /// <returns></returns>       
        [HttpPost]
        //[Route("UploadFile/startByte:int=0}/{complete:bool=true}/{resId}/{dirName}/{state?}")]
        public async Task<IHttpActionResult> UploadFile(string dir = "", string category = "", string fk = "", bool complete = true)
        {
            var result = new OperationResult<Attachment>(true);
            int startByte = 0;
            //文件名
            string fileName = string.Empty;
            //文件扩展名
            string fileExt = string.Empty;
            //文件标识
            var fileId = Guid.NewGuid().ToString("D");
            //文件类型
            string fileType = string.Empty;
            //分类
            //string category = string.Empty;
            //目录名称      
            //string dir = string.Empty;
            //目录名称      
            //string fk = string.Empty;

            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return StatusCode(HttpStatusCode.UnsupportedMediaType);
                }
                if (string.IsNullOrEmpty(dir))
                {

                }
                var absoluteDir = PathTool.GetAbsoluteAttachmentPath(dir);
                var provider = new MultipartFormDataStreamProvider(absoluteDir);
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (MultipartFileData fileData in provider.FileData)
                {
                    //category = fileData.Headers.ContentDisposition.Parameters.Single(p => p.Name == "category").Value;
                    //dir = fileData.Headers.ContentDisposition.Parameters.Single(p => p.Name == "dir").Value;
                    //fk = fileData.Headers.ContentDisposition.Parameters.Single(p => p.Name == "fk").Value;

                    //文件名
                    fileName = fileData.Headers.ContentDisposition.FileName;
                    fileName = fileName.TrimStart('"');
                    fileName = fileName.TrimEnd('"');
                    fileName = fileName.Replace(" ", string.Empty);
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }
                    //文件扩展名
                    fileExt = fileData.Headers.ContentDisposition.FileName.TrimEnd('\"').Split(new char[] { '.' }).Last();
                    //服务器本地暂存文件
                    FileStream source = File.OpenRead(fileData.LocalFileName);
                    //另存为文件
                    FileInfo target = new FileInfo(Path.Combine(absoluteDir, fileId + "." + fileExt));
                    if (target.Exists)
                    {
                        if (startByte > 0)
                        {
                            using (FileStream fs = target.Open(FileMode.Append, FileAccess.Write))
                            {
                                SaveFile(source, fs);
                            }
                        }
                        else
                        {
                            target.Delete();
                            using (FileStream fs = target.Create())
                            {
                                SaveFile(source, fs);
                            }
                        }
                    }
                    else
                    {
                        using (FileStream fs = target.Create())
                        {
                            SaveFile(source, fs);
                        }
                    }
                    source.Close();
                    File.Delete(fileData.LocalFileName);
                }

                fileType = "." + fileExt;
                var newFileName = string.Concat(fileId, fileType);
                string filePath = PathTool.GetRelativeAttachmentPath(newFileName, dir);
                FileInfo fi = new FileInfo(HttpContext.Current.Server.MapPath("~" + filePath));
                //当此次操作为最后一次时，修改本地数据库的业务数据，不是则返回告知客户端此次操作已完成
                if (complete)
                {
                    //保存文件相关信息
                    var entity = new AttachmentRequestModel()
                    {
                        Id = fileId,
                        FileName = fileName,
                        FileSize = fi.Length,
                        FileType = fileType,
                        FilePath = filePath,
                        Category = category,
                        AllowAnonymous = false,
                        FK = fk,
                        UploadTime = DateTime.Now,
                        UploadBy = User.Identity.Name,
                        Flag = 1,
                        MD5 = new FileTool().GetFileMD5String(fi.FullName)
                    };
                    var attachment = await service.InsertAttachment(entity);
                    result.Data = attachment;
                }

                //按照要求的格式，返回结果
                //var response = new
                //{
                //    name = fileName,
                //    size = fi.Length,
                //    url = filePath,
                //    deleteUrl = Url.Content("/api/Attachment/DeleteFile/" + fileId),
                //    deleteType = "DELETE"
                //};

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                result = new OperationResult<Attachment>(ex.Message);
            }
            return Json(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> ImageBase64(string attachmentId, bool isThumbnail)
        {
            var result = new OperationResult<string>(true);
            try
            {
                byte[] imageBytes = null;
                var model = new AttachmentRequestModel() { Id = attachmentId };
                var entity = await service.GetAttachment(model);
                if (entity == null)
                {
                    throw new Exception("附件不存在");
                }
                var fileType = entity.FileType.ToLower();
                if (fileType != ".jpg" && fileType != ".jpeg" && fileType != ".png" && fileType != ".bmp")
                {
                    throw new Exception("非图像文件");
                }
                var imagePath = System.Web.Hosting.HostingEnvironment.MapPath("~" + entity.FilePath);
                //生成略缩图
                if (isThumbnail)
                {
                    var size = Tools.AppSettingTool.Value<uint>("Attachment_Thumbnail_Size", 100);
                    imageBytes = new ThumbnailSharp.ThumbnailCreator().CreateThumbnailBytes(
                                        thumbnailSize: size,
                                        imageFileLocation: imagePath,
                                        imageFormat: ThumbnailSharp.Format.Jpeg);
                }
                else
                {
                    var ms = new MemoryStream();
                    var img = System.Drawing.Image.FromFile(imagePath);
                    img.Save(ms, img.RawFormat);
                    imageBytes = ms.ToArray();
                    ms.Close();
                }
                result.Data = "data:image/*;charset=utf-8;base64," + Convert.ToBase64String(imageBytes);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                result = new OperationResult<string>(ex.Message);
            }
            return Json(result);
        }

        /// <summary>
        /// 文件删除
        /// </summary>
        [HttpPost]
        public async Task<IHttpActionResult> DeleteAttachment(AttachmentRequestModel model)
        {
            var result = new OperationResult<Attachment>(true);
            var entry = await service.DeleteAttachment(model);
            if (entry == null)
            {
                result = new OperationResult<Attachment>(false);
                return Json(result);
            }
            string filePath = HttpContext.Current.Server.MapPath("~" + entry.FilePath);
            FileInfo fi = new FileInfo(filePath);
            if (fi.Exists)
            {
                fi.Delete();
            }
            return Json(result);
        }



        /// <summary>
        /// 文件保存
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fs"></param>
        private void SaveFile(Stream stream, FileStream fs)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, bytesRead);
            }
        }
    }
}
