using Demo4DotNetCore.ResourceServer.Books.Model;
using Demo4DotNetCore.ResourceServer.Books.RequestModel;
using Demo4DotNetCore.ResourceServer.Model;
using System.Threading.Tasks;

namespace Demo4DotNetCore.ResourceServer.Books.Service
{
    public interface IAttachmentService
    {
        Task<PaginatedResult<Attachment>> Retrieve(AttachmentRequestModel model);
        Task<Attachment> Single(AttachmentRequestModel model);
        Task<Attachment> Add(AttachmentRequestModel model);
        Task<Attachment> Modify(AttachmentRequestModel model);
        Task<Attachment> Delete(AttachmentRequestModel model);
    }
}