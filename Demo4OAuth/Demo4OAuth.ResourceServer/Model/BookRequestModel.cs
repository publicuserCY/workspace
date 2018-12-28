using System.Collections.Generic;
using System.Web.Http.ModelBinding;

namespace Demo4OAuth.ResourceServer.Model
{
    //[ModelBinder(typeof(BookRequestModelBinder))]
    public class BookRequestModel: PaginatedRequestModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}