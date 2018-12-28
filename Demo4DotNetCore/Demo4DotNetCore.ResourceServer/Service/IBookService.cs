using Demo4DotNetCore.ResourceServer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo4DotNetCore.ResourceServer.Service
{
    public interface IBookService
    {
        Task<PaginatedList<Book>> GetBooks(BookRequestModel model);
        Task<Book> GetBook(BookRequestModel model);
        Task<Book> InsertBook(BookRequestModel model);
        Task<Book> UpdateBook(BookRequestModel model);
        Task<Book> DeleteBook(BookRequestModel model);
    }
}