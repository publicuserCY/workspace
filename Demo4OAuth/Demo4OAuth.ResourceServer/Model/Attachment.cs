using Demo4OAuth.Tools;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Demo4OAuth.ResourceServer.Model
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public class Attachment : BaseModel
    {
        public Attachment() : base(true) { }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 附件类别
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 允许匿名下载
        /// </summary>
        public bool AllowAnonymous { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        public string FK { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }
        /// <summary>
        /// 上传人员标识
        /// </summary>
        public string UploadBy { get; set; }

        /// <summary>
        /// 操作标志 0：未改 1：新增 2：更新 3：删除
        /// </summary>
        public int Flag { get; set; }

        /// <summary>
        /// MD5
        /// </summary>
        public string MD5 { get; set; }
        /*
        [NotMapped]
        public string ImageBase64
        {
            get
            {
                string result = string.Empty;
                try
                {
                    var fileType = FileType.ToLower();
                    if (fileType != ".jpg" && fileType != ".jpeg" && fileType != ".png" && fileType != ".bmp")
                    {
                        return result;
                    }
                    var imagePath = System.Web.Hosting.HostingEnvironment.MapPath("~" + FilePath);
                    //生成略缩图

                        var size = Tools.AppSettingTool.Value<uint>("Attachment_Thumbnail_Size", 100);
                        byte[] thumbnailBytes = new ThumbnailSharp.ThumbnailCreator().CreateThumbnailBytes(
                                                thumbnailSize: size,
                                                imageFileLocation: imagePath,
                                                imageFormat: ThumbnailSharp.Format.Jpeg
                                             );
                        result = "data:image/*;charset=utf-8;base64," + Convert.ToBase64String(thumbnailBytes);
              
                    return result;
                }
                catch (Exception ex)
                {
                    var logger = NLog.LogManager.GetLogger(GetType().FullName);
                    logger.Debug(ex.Message);
                    return "";
                }
            }
        }
        */
    }
}
