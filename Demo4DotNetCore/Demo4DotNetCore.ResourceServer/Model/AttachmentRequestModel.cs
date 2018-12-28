using System;

namespace Demo4DotNetCore.ResourceServer.Model
{
    public class AttachmentRequestModel : PaginatedRequestModel
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string Category { get; set; }
        public bool AllowAnonymous { get; set; }
        public string Reference { get; set; }
        public DateTime UploadTime { get; set; }
        public string UploadBy { get; set; }
        public int Flag { get; set; }
        public string MD5 { get; set; }
    }
}