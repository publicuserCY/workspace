using Demo4DotNetCore.ResourceServer.Models;
using System;

namespace Demo4DotNetCore.ResourceServer.Books.Models
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public class Attachment : BaseModel
    {
        public Attachment() : base() { }

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
        /// 引用键
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }
        /// <summary>
        /// 上传人员标识
        /// </summary>
        public string UploadBy { get; set; }

        /// <summary>
        /// MD5
        /// </summary>
        public string MD5 { get; set; }       
    }
}
