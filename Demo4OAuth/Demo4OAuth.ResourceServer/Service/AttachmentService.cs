using Demo4OAuth.ResourceServer.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo4OAuth.ResourceServer.Service
{
    public class AttachmentsService : IAttachmentService
    {
        private ResourceContext context;

        public AttachmentsService(ResourceContext context)
        {
            this.context = context;
        }
        public Task<PaginatedList<Attachment>> GetAttachments(AttachmentRequestModel model)
        {
            var result = context.Attachments.ToPaginatedList(model.PageIndex, model.PageSize);
            return Task.FromResult(result);
        }

        public Task<Attachment> GetAttachment(AttachmentRequestModel model)
        {
            var result = context.Attachments.SingleOrDefault(p => p.Id == model.Id);
            return Task.FromResult(result);
        }

        public Task<Attachment> InsertAttachment(AttachmentRequestModel model)
        {
            var entity = new Attachment()
            {
                FileName = model.FileName,
                FileSize = model.FileSize,
                FileType = model.FileType,
                FilePath = model.FilePath,
                Category = model.Category,
                AllowAnonymous = model.AllowAnonymous,
                FK = model.FK,
                UploadTime = DateTime.Now,
                UploadBy = model.UploadBy,
                Flag = model.Flag,
                MD5 = model.MD5
            };
            var result = context.Attachments.Add(entity);
            context.SaveChanges();
            return Task.FromResult(result);
        }
        public Task<Attachment> UpdateAttachment(AttachmentRequestModel model)
        {
            var entity = context.Attachments.SingleOrDefault(p => p.Id == model.Id);
            if (entity == null)
            {
                return Task.FromResult<Attachment>(null);
            }
            entity.FileName = model.FileName;
            context.SaveChanges();
            return Task.FromResult(entity);
        }
        public Task<Attachment> DeleteAttachment(AttachmentRequestModel model)
        {
            var entity = context.Attachments.SingleOrDefault(p => p.Id == model.Id);
            if (entity == null)
            {
                return Task.FromResult<Attachment>(null);
            }
            context.Attachments.Remove(entity);
            context.SaveChanges();
            return Task.FromResult(entity);
        }
    }
}