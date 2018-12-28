using System;
using System.IO;
using System.Web;

namespace Demo4OAuth.Tools
{
    public static class PathTool
    {
        /// <summary>
        /// 获取服务器绝对路径(自定义目录)
        /// </summary>
        /// <returns></returns>
        public static string GetAbsolutePath(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException("name", "参数不能为空"); }
            //根目录
            var uploadDir = AppSettingTool.Value("Directory_Upload", "Upload");
            //服务器上的绝对保存路径
            var absoluteDir = HttpContext.Current.Server.MapPath(string.Concat("~/", uploadDir, "/", name));
            if (!Directory.Exists(absoluteDir)) { Directory.CreateDirectory(absoluteDir); }
            return absoluteDir;
        }

        /// <summary>
        /// 获取服务器相对对路径(自定义目录)
        /// </summary>
        /// <returns></returns>
        public static string GetRelativePath(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException("name", "参数不能为空"); }
            //根目录
            var uploadDir = AppSettingTool.Value("Directory_Upload", "Upload");
            return string.Concat("/", uploadDir, "/", name);
        }

        /// <summary>
        /// 获取服务器绝对路径(头像)
        /// </summary>
        /// <returns></returns>
        public static string GetAbsolutePortraitPath()
        {
            //根目录
            var uploadDir = AppSettingTool.Value("Directory_Upload", "Upload");
            //头像目录
            var portraitDir = AppSettingTool.Value("Directory_Portrait", "Portrait");
            //服务器上的绝对保存路径(头像目录)
            var portraitAbsoluteDir = HttpContext.Current.Server.MapPath(string.Concat("~/", uploadDir, "/", portraitDir));
            if (!Directory.Exists(portraitAbsoluteDir)) { Directory.CreateDirectory(portraitAbsoluteDir); }
            return portraitAbsoluteDir;
        }
        /// <summary>
        /// 获取缩略图路径
        /// </summary>
        /// <returns></returns>
        public static string GetAbsolutePortraitThumbnailsPath()
        {
            //根目录
            var uploadDir = AppSettingTool.Value("Directory_Upload", "Upload");
            //头像缩略目录
            var portraitThumbnailsDir = AppSettingTool.Value("Directory_PortraitThumbnails", "PortraitThumbnails");
            //服务器上的绝对保存路径(头像缩略目录)
            var portraitThumbnailsAbsoluteDir = HttpContext.Current.Server.MapPath(string.Concat("~/", uploadDir, "/", portraitThumbnailsDir));
            if (!Directory.Exists(portraitThumbnailsAbsoluteDir)) { Directory.CreateDirectory(portraitThumbnailsAbsoluteDir); }
            return portraitThumbnailsAbsoluteDir;
        }

        /// <summary>
        /// 获取服务器相对路径(头像)
        /// </summary>
        /// <returns></returns>
        public static string GetRelativePortraitPath(string fileName)
        {
            //根目录
            var uploadDir = AppSettingTool.Value("Directory_Upload", "Upload");
            //头像目录
            var portraitDir = AppSettingTool.Value("Directory_Portrait", "Portrait");
            return string.Concat("/", uploadDir, "/", portraitDir, "/", fileName);
        }

        /// <summary>
        /// 获取服务器绝对路径(附件)
        /// </summary>
        /// <returns></returns>
        public static string GetAbsoluteAttachmentPath(string dirName = "")
        {
            //根目录
            var uploadDir = AppSettingTool.Value("Directory_Upload", "Upload");
            //服务器上的绝对保存路径(附件目录)
            var attachmentAbsoluteDir = HttpContext.Current.Server.MapPath(string.Concat("~/", uploadDir));
            if (!Directory.Exists(attachmentAbsoluteDir)) { Directory.CreateDirectory(attachmentAbsoluteDir); }
            if (!string.IsNullOrEmpty(dirName))
            {
                attachmentAbsoluteDir = HttpContext.Current.Server.MapPath(string.Concat("~/", uploadDir, "/", dirName, "/"));
                if (!Directory.Exists(attachmentAbsoluteDir)) { Directory.CreateDirectory(attachmentAbsoluteDir); }
            }
            return attachmentAbsoluteDir;
        }

        /// <summary>
        /// 获取服务器相对路径(附件)
        /// </summary>
        /// <returns></returns>
        public static string GetRelativeAttachmentPath(string fileName, string dirName = "")
        {
            //根目录
            var uploadDir = AppSettingTool.Value("Directory_Upload", "Upload");
            if (string.IsNullOrEmpty(dirName))
            {
                return string.Concat("/", uploadDir, "/", fileName);
            }
            else
            {
                return string.Concat("/", uploadDir, "/", dirName, "/", fileName);
            }

        }

        /// <summary>
        /// 获取服务器绝对路径(附件)Video
        /// </summary>
        /// <returns></returns>
        public static string GetAbsoluteAttachmentPathVideo(string dirName)
        {
            //根目录
            var uploadDir = AppSettingTool.Value("Directory_Video", "Video");
            //当前日期
            var shortDate = DateTime.Now.ToString("yyyyMMdd");
            //服务器上的绝对保存路径(附件目录)
            var attachmentAbsoluteDir = HttpContext.Current.Server.MapPath(string.Concat("~/", uploadDir, "/", dirName, "/", shortDate));
            if (!Directory.Exists(attachmentAbsoluteDir)) { Directory.CreateDirectory(attachmentAbsoluteDir); }
            return attachmentAbsoluteDir;
        }

        /// <summary>
        /// 获取服务器相对路径(附件)Video
        /// </summary>
        /// <returns></returns>
        public static string GetRelativeAttachmentPathVideo(string fileName, string dirName)
        {
            //根目录
            var uploadDir = AppSettingTool.Value("Directory_Video", "Video");
            return string.Concat("/", uploadDir, "/", dirName, "/", fileName);
        }
    }
}