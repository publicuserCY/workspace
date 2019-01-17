using Demo4DotNetCore.ResourceServer.Books.Model;
using Demo4DotNetCore.ResourceServer.Model;
using System;

namespace Demo4DotNetCore.ResourceServer.Books.RequestModel
{
    public class AttachmentRequestModel : PaginatedRequestModel
    {
        public string Id { get; set; }
        public Attachment Attachment { get; set; }
}
}