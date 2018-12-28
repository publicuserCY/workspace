using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.IO;
using System.Web.Hosting;
using System.Configuration;
using Dtsc.TPCIS.Model.Tool;

namespace Demo4OAuth.Tools
{
    public class ThumbnailImageHelper
    {
        const int HEIGHT = 150;
        const int WIDTH = 150;
        public static void SetThumbnail_1(string FileName, string dirName)
        {
            //根目录
            var uploadDir = AppSettingTool.Value(Model.Core.Constants.Key_Directory_Upload, Model.Core.Constants.Default_Directory_Upload);
            string srcImgPath = @HostingEnvironment.ApplicationPhysicalPath + uploadDir + "//" + dirName + "//" + FileName;

            string targetFolder = @HostingEnvironment.ApplicationPhysicalPath + "Thumbnails//" + dirName + "//" + FileName;

            string getDirectoryName = Path.GetDirectoryName(targetFolder);
            if (!Directory.Exists(getDirectoryName)) { Directory.CreateDirectory(getDirectoryName); }
            using (Bitmap source = new Bitmap(srcImgPath))
            {

                int wi, hi;
                wi = WIDTH;
                hi = HEIGHT;
                using (Image thumb = source.GetThumbnailImage(wi, hi, null, IntPtr.Zero))
                {
                    //string targetPath = Path.Combine(targetFolder, FileName);
                    thumb.Save(targetFolder);
                }
            }
        }
        /// <summary>
        /// 保存缩略图
        /// </summary>
        /// <param name="srcImgPath">原图片路径</param>
        /// <param name="targetFolder">缩略图存放路径</param>
        public static void SaveThumbnail4(string srcImgPath, string targetFolder)
        {
            ////存储缩略图
            //System.IO.FileInfo fi = new System.IO.FileInfo(targetFolder);
            ////判断缩略图是否存在，如果存在则先删除
            //if (fi.Exists)
            //{
            //    fi.Delete();
            //}
            //using (Bitmap source = new Bitmap(srcImgPath))
            //{
            //    int wi, hi;
            //    wi = WIDTH;
            //    hi = HEIGHT;
            //    using (Image thumb = source.GetThumbnailImage(wi, hi, null, IntPtr.Zero))
            //    {
            //        thumb.Save(targetFolder);
            //    }
            //}
            //根目录
          //  var uploadDir = AppSettingTool.Value(Model.Core.Constants.Key_Directory_Upload, Model.Core.Constants.Default_Directory_Upload);
            string srcImgPath1 = @HostingEnvironment.ApplicationPhysicalPath +"//"+ srcImgPath;

            string targetFolder1 = @HostingEnvironment.ApplicationPhysicalPath + "//" + targetFolder;

            string getDirectoryName = Path.GetDirectoryName(targetFolder1);
            if (!Directory.Exists(getDirectoryName)) { Directory.CreateDirectory(getDirectoryName); }
            using (Bitmap source = new Bitmap(srcImgPath1))
            {

                int wi, hi;
                wi = WIDTH;
                hi = HEIGHT;
                using (Image thumb = source.GetThumbnailImage(wi, hi, null, IntPtr.Zero))
                {
                    //string targetPath = Path.Combine(targetFolder, FileName);
                    thumb.Save(targetFolder1);
                }
            }
        }
        /// <summary>
        /// 保存缩略图
        /// </summary>
        /// <param name="srcImgPath">原图片路径</param>
        /// <param name="targetFolder">缩略图存放路径</param>
        public static void SaveThumbnail(string srcImgPath, string targetFolder)
        {
            //存储缩略图
            System.IO.FileInfo fi = new System.IO.FileInfo(targetFolder);
            //判断缩略图是否存在，如果存在则先删除
            if (fi.Exists)
            {
                fi.Delete();
            }
            using (Bitmap source = new Bitmap(srcImgPath))
            {
                int wi, hi;
                wi = WIDTH;
                hi = HEIGHT;
                using (Image thumb = source.GetThumbnailImage(wi, hi, null, IntPtr.Zero))
                {
                    thumb.Save(targetFolder);
                }
            }
        }
        /// <summary>
        /// 存放身份缩略图专用
        /// </summary>
        /// <param name="FileName"></param>
        public static void SetThumbnail_Account(string FileName)
        {
            //string srcImgPath = @HostingEnvironment.ApplicationPhysicalPath + "UploadFiles" + FileName;
            //string targetFolder = @HostingEnvironment.ApplicationPhysicalPath + "Thumbnails";
            string srcImgPath = @ConfigurationManager.AppSettings["AbsolutePath"] + FileName;
            FileName = FileName.Replace("UploadFile", "Thumbnails");
            string targetFolder = @ConfigurationManager.AppSettings["AbsolutePath"] + FileName;
            //判断服务器是否存在改路径，如果不存在则创建
            string getDirectoryName = Path.GetDirectoryName(targetFolder);
            if (!Directory.Exists(getDirectoryName)) { Directory.CreateDirectory(getDirectoryName); }
            using (Bitmap source = new Bitmap(srcImgPath))
            {

                int wi, hi;
                wi = WIDTH;
                hi = HEIGHT;
                using (Image thumb = source.GetThumbnailImage(wi, hi, null, IntPtr.Zero))
                {
                    thumb.Save(targetFolder);
                }
            }
        }

        public static void SetThumbnail_2(string srcImgPath, string targetFolder, string FileName)
        {
            using (Bitmap source = new Bitmap(srcImgPath))
            {
                // return the source image if it's smaller than the designated thumbnail   
                int wi, hi;
                wi = WIDTH;
                hi = HEIGHT;


                //// maintain the aspect ratio despite the thumbnail size parameters   
                //if (source.Width > source.Height)
                //{
                //    wi = WIDTH;
                //    hi = (int)(source.Height * ((decimal)WIDTH / source.Width));
                //}
                //else
                //{
                //    hi = HEIGHT;
                //    wi = (int)(source.Width * ((decimal)HEIGHT / source.Height));
                //}

                // original code that creates lousy thumbnails   
                // System.Drawing.Image ret = source.GetThumbnailImage(wi,hi,null,IntPtr.Zero);   
                using (System.Drawing.Bitmap thumb = new Bitmap(wi, hi))
                {
                    using (Graphics g = Graphics.FromImage(thumb))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.FillRectangle(Brushes.White, 0, 0, wi, hi);
                        g.DrawImage(source, 0, 0, wi, hi);
                    }
                    string targetPath = Path.Combine(targetFolder, FileName);
                    thumb.Save(targetPath);
                }

            }
        }

        public static void SetThumbnail_3(string srcImgPath, string targetFolder, string FileName)
        {
            //Configure JPEG Compression Engine   
            System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 75;
            System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            System.Drawing.Imaging.ImageCodecInfo[] arrayICI = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            System.Drawing.Imaging.ImageCodecInfo jpegICI = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[x];
                    break;
                }
            }

            using (Bitmap source = new Bitmap(srcImgPath))
            {
                int wi, hi;
                wi = WIDTH;
                hi = HEIGHT;


                // maintain the aspect ratio despite the thumbnail size parameters   
                //if (source.Width > source.Height)
                //{
                //    wi = WIDTH;
                //    hi = (int)(source.Height * ((decimal)WIDTH / source.Width));
                //}
                //else
                //{
                //    hi = HEIGHT;
                //    wi = (int)(source.Width * ((decimal)HEIGHT / source.Height));
                //}

                // original code that creates lousy thumbnails   
                // System.Drawing.Image ret = source.GetThumbnailImage(wi,hi,null,IntPtr.Zero);   
                using (System.Drawing.Bitmap thumb = new Bitmap(wi, hi))
                {
                    using (Graphics g = Graphics.FromImage(thumb))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.FillRectangle(Brushes.White, 0, 0, wi, hi);
                        g.DrawImage(source, 0, 0, wi, hi);
                    }
                    string targetPath = Path.Combine(targetFolder, FileName);
                    thumb.Save(targetPath, jpegICI, encoderParams);
                }

            }

        }
    }
}