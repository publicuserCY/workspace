using System.Collections.Generic;

namespace Demo4DotNetCore.ResourceServer.Model
{
    public class BookRequestModel: PaginatedRequestModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}