using Demo4DotNetCore.ResourceServer.Model;
using Demo4DotNetCore.ResourceServer.Service;
using Demo4DotNetCore.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private NLog.Logger logger;

        public BookController(IBookService service)
        {
            this.service = service;
            logger = NLog.LogManager.GetLogger(GetType().FullName);
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
                logger.Error(ex.Message);
                result = new OperationResult<PaginatedList<Book>>(ex.Message);
            }
            return new JsonResult(result);
        }
    }
}
