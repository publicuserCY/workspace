using Demo4DotNetCore.ResourceServer.Books.Model;
using Demo4DotNetCore.ResourceServer.Books.RequestModel;
using Demo4DotNetCore.ResourceServer.Books.Service;
using Demo4DotNetCore.ResourceServer.Model;
using Demo4DotNetCore.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo4DotNetCore.ResourceServer.Books.Controller
{
    [Route("api/{area}/[controller]/[action]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookService service;
        private ILogger<BookController> Logger { get; }

        public BookController(ILogger<BookController> logger, IBookService service)
        {
            this.service = service;
            Logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult> GetBooks(string criteria = "", int pageIndex = 0, int pageSize = 0, string orderBy = "", string direction = "")
        {
            var result = new OperationResult<PaginatedResult<Book>>(true);
            try
            {
                var query = from c in User.Claims select new { c.Type, c.Value };

                var model = new BookRequestModel()
                {
                    Criteria = criteria,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    Direction = direction
                };
                var list = await service.Retrieve(model);
                result.Data = list;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<PaginatedResult<Book>>(ex.Message);
            }
            return new JsonResult(result);
        }
    }
}
