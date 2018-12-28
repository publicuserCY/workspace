using Demo4OAuth.ResourceServer.Model;
using Demo4OAuth.ResourceServer.Service;
using Demo4OAuth.Tools;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Demo4OAuth.ResourceServer.Controllers
{
    public class BookController : ApiController
    {
        private IBookService service;
        private NLog.Logger logger;

        public BookController(IBookService service)
        {
            this.service = service;
            logger = NLog.LogManager.GetLogger(GetType().FullName);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetBooks(string criteria = "", int pageIndex = 0, int pageSize = 0, string orderBy = "", string direction = "")
        {
            var result = new OperationResult<PaginatedList<Book>>(true);
            try
            {
                var model = new BookRequestModel()
                {
                    Criteria = criteria,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    Direction = direction
                };
                var list = await service.GetBooks(model);
                result.Data = list;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                result = new OperationResult<PaginatedList<Book>>(ex.Message);
            }
            return Json(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetBook(string id)
        {
            var result = new OperationResult<Book>(true);
            try
            {
                var model = new BookRequestModel() { Id = id };
                var book = await service.GetBook(model);
                if (book == null)
                {
                    result = new OperationResult<Book>(false, "100004");
                }
                else
                {
                    result.Data = book;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                result = new OperationResult<Book>(ex.Message);
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> InsertBook(BookRequestModel model)
        {
            var result = new OperationResult<Book>(true);
            try
            {
                var book = await service.InsertBook(model);
                result.Data = book;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                result = new OperationResult<Book>(ex.Message);
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateBook(BookRequestModel model)
        {
            var result = new OperationResult<Book>(true);
            try
            {
                var book = await service.UpdateBook(model);
                result.Data = book;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                result = new OperationResult<Book>(ex.Message);
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> DeleteBook(BookRequestModel model)
        {
            var result = new OperationResult<Book>(true);
            try
            {
                var book = await service.DeleteBook(model);
                result.Data = book;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                result = new OperationResult<Book>(ex.Message);
            }
            return Json(result);
        }
    }
}
