using Demo4DotNetCore.ResourceServer.Books.Model;
using Demo4DotNetCore.ResourceServer.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Demo4DotNetCore.ResourceServer.Books.Service
{
    public class BookService : IBookService
    {
        private ResourceContext DbContext { get; }

        public BookService(ResourceContext context)
        {
            DbContext = context;
        }
        public Task<PaginatedResult<Book>> Retrieve(BookRequestModel model)
        {
            var query = DbContext.Books.Include(p => p.Attachments).AsExpandable();
            var predicate = PredicateBuilder.New<Book>();
            if (!string.IsNullOrEmpty(model.Criteria))
            {
                predicate = predicate.And(p => p.Name.Contains(model.Criteria, StringComparison.OrdinalIgnoreCase));
                query = query.AsQueryable().Where(predicate);
            }
            var result = query.SortBy(model.SortExpression).ToPaginatedList(model.PageIndex, model.PageSize);
            return Task.FromResult(result);
        }

        public Task<Book> Single(BookRequestModel model)
        {
            var result = DbContext.Books.Include(p => p.Attachments).SingleOrDefault(p => p.Id == model.Id);
            return Task.FromResult(result);
        }

        public Task<Book> Add(BookRequestModel model)
        {
            using (var dbContextTransaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var entity = new Book()
                    {
                        Name = model.Name
                    };
                    var entry = DbContext.Entry(entity);
                    entry.State = EntityState.Added;
                    DbContext.SaveChanges();

                    if (model.Book.Attachments != null)
                    {
                        model.Book.Attachments.ToList().ForEach((item) =>
                        {
                            DbContext.Attachments.Where(p => p.Id == item.Id).Update(attachment => new Attachment() { Reference = entity.Id, Flag = 0 });
                        });
                    }
                    DbContext.SaveChanges();                   
                    dbContextTransaction.Commit();
                    entry.Reload();
                    return Task.FromResult(entry.Entity);
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public Task<Book> Modify(BookRequestModel model)
        {
            using (var dbContextTransaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var entity = model.Book;
                    var entry = DbContext.Entry(entity);
                    entry.State = EntityState.Modified;
                    model.Book.Attachments.Where(p => p.Flag == 3).ToList().ForEach((item) =>
                        {
                            DbContext.Attachments.Where(p => p.Id == item.Id).Delete();
                        });
                    DbContext.Attachments.Where(p => p.Reference == model.Id && p.Flag == 1).Update(attachment => new Attachment() { Flag = 0 });
                    DbContext.SaveChanges();
                    dbContextTransaction.Commit();
                    entry.Reload();
                    return Task.FromResult(entry.Entity);
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }
        public Task<Book> Delete(BookRequestModel model)
        {
            using (var dbContextTransaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var entity = model.Book;
                    var entry = DbContext.Entry(entity);
                    entry.State = EntityState.Deleted;
                    DbContext.Attachments.Where(p => p.Reference == entity.Id).Delete();
                    DbContext.SaveChanges();
                    dbContextTransaction.Commit();
                    return Task.FromResult(entry.Entity);
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }
    }
}