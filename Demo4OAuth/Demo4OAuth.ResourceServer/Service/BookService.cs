using Demo4OAuth.ResourceServer.Model;
using LinqKit;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Demo4OAuth.ResourceServer.Service
{
    public class BookService : IBookService
    {
        private ResourceContext context;
        public BookService(ResourceContext context)
        {
            this.context = context;
        }
        public Task<PaginatedList<Book>> GetBooks(BookRequestModel model)
        {
            var query = context.Books.Include(p => p.Attachments);
            if (!string.IsNullOrWhiteSpace(model.Criteria))
            {
                var predicate = PredicateBuilder.New<Book>();
                predicate = predicate.Or(p => p.Name.Contains(model.Criteria));
                query = query.AsExpandable().Where(predicate);
            }
            var result = query.AsQueryable().SortBy(model.SortExpression).ToPaginatedList(model.PageIndex, model.PageSize);
            return Task.FromResult(result);
        }

        public Task<Book> GetBook(BookRequestModel model)
        {
            var result = context.Books.Include(p => p.Attachments).SingleOrDefault(p => p.Id == model.Id);
            return Task.FromResult(result);
        }

        public Task<Book> InsertBook(BookRequestModel model)
        {
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    var entity = new Book()
                    {
                        Name = model.Name
                    };
                    context.Books.Add(entity);
                    if (model.Attachments != null)
                    {
                        model.Attachments.ForEach((item) =>
                        {
                            context.Attachments.Where(p => p.Id == item.Id).Update(attachment => new Attachment() { FK = entity.Id, Flag = 0 });
                        });
                    }
                    context.SaveChanges();
                    entity = context.Books.Include(p => p.Attachments).SingleOrDefault(p => p.Id == entity.Id);
                    dbContextTransaction.Commit();
                    return Task.FromResult(entity);
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public Task<Book> UpdateBook(BookRequestModel model)
        {
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    model.Attachments.Where(p => p.Flag == 3).ToList().ForEach((item) =>
                        {
                            context.Attachments.Where(p => p.Id == item.Id).Delete();
                        });
                    context.Attachments.Where(p => p.FK == model.Id && p.Flag == 1).Update(attachment => new Attachment() { Flag = 0 });
                    var entity = context.Books.Include(p => p.Attachments).SingleOrDefault(p => p.Id == model.Id);
                    entity.Name = model.Name;
                    context.SaveChanges();
                    dbContextTransaction.Commit();
                    return Task.FromResult(entity);
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public Task<Book> DeleteBook(BookRequestModel model)
        {
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    var entity = context.Books.SingleOrDefault(p => p.Id == model.Id);
                    context.Attachments.Where(p => p.FK == model.Id).Delete();
                    context.Books.Where(p => p.Id == model.Id).Delete();
                    context.SaveChanges();
                    dbContextTransaction.Commit();
                    return Task.FromResult(entity);
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