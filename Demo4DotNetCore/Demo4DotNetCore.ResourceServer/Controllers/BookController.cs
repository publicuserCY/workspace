using Demo4DotNetCore.ResourceServer.Model;
using Demo4DotNetCore.ResourceServer.Service;
using Demo4DotNetCore.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo4DotNetCore.ResourceServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
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
            var result = new OperationResult<PaginatedList<Book>>(true);
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
                var list = await service.GetBooks(model);
                result.Data = list;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<PaginatedList<Book>>(ex.Message);
            }
            return new JsonResult(result);
        }
    }
}
