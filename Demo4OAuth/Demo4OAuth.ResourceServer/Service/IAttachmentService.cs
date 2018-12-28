using Demo4OAuth.ResourceServer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo4OAuth.ResourceServer.Service
{
    public interface IAttachmentService
    {
        Task<PaginatedList<Attachment>> GetAttachments(AttachmentRequestModel model);
        Task<Attachment> GetAttachment(AttachmentRequestModel model);
        Task<Attachment> InsertAttachment(AttachmentRequestModel entity);
        Task<Attachment> UpdateAttachment(AttachmentRequestModel entity);
        Task<Attachment> DeleteAttachment(AttachmentRequestModel entity);
    }
}