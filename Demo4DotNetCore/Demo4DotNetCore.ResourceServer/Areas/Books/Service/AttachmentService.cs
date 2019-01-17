using Demo4DotNetCore.ResourceServer.Books.Model;
using Demo4DotNetCore.ResourceServer.Books.RequestModel;
using Demo4DotNetCore.ResourceServer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo4DotNetCore.ResourceServer.Books.Service
{
    public class AttachmentsService : IAttachmentService
    {
        private ResourceContext DbContext { get; }

        public AttachmentsService(ResourceContext context)
        {
            DbContext = context;
        }
        public Task<PaginatedResult<Attachment>> Retrieve(AttachmentRequestModel model)
        {
            var result = DbContext.Attachments.ToPaginatedList(model.PageIndex, model.PageSize);
            return Task.FromResult(result);
        }

        public Task<Attachment> Single(AttachmentRequestModel model)
        {
            var result = DbContext.Attachments.SingleOrDefault(p => p.Id == model.Id);
            return Task.FromResult(result);
        }

        public Task<Attachment> Add(AttachmentRequestModel model)
        {
            var entity = new Attachment()
            {
                FileName = model.Attachment.FileName,
                FileSize = model.Attachment.FileSize,
                FileType = model.Attachment.FileType,
                FilePath = model.Attachment.FilePath,
                Category = model.Attachment.Category,
                AllowAnonymous = model.Attachment.AllowAnonymous,
                Reference = model.Attachment.Reference,
                UploadTime = DateTime.Now,
                UploadBy = model.Attachment.UploadBy,
                MD5 = model.Attachment.MD5
            };
            var entry = DbContext.Entry(entity);
            entry.State = EntityState.Added;
            DbContext.SaveChanges();
            entry.Reload();
            return Task.FromResult(entry.Entity);
        }
        public Task<Attachment> Modify(AttachmentRequestModel model)
        {
            var entity = model.Attachment;
            var entry = DbContext.Entry(entity);
            entry.State = EntityState.Modified;
            DbContext.SaveChanges();
            entry.Reload();
            return Task.FromResult(entry.Entity);
        }
        public Task<Attachment> Delete(AttachmentRequestModel model)
        {
            var entity = model.Attachment;
            var entry = DbContext.Entry(entity);
            entry.State = EntityState.Deleted;
            DbContext.SaveChanges();
            return Task.FromResult(entry.Entity);
        }
    }
}