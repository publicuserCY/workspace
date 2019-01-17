using Demo4DotNetCore.ResourceServer.Books.Model;
using Demo4DotNetCore.ResourceServer.Books.RequestModel;
using Demo4DotNetCore.ResourceServer.Model;
using System.Threading.Tasks;

namespace Demo4DotNetCore.ResourceServer.Books.Service
{
    public interface IBookService
    {
        Task<PaginatedResult<Book>> Retrieve(BookRequestModel model);
        Task<Book> Single(BookRequestModel model);
        Task<Book> Add(BookRequestModel model);
        Task<Book> Modify(BookRequestModel model);
        Task<Book> Delete(BookRequestModel model);
    }
}