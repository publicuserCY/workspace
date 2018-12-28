using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Demo4DotNetCore.ResourceServer.Model;
using Demo4DotNetCore.ResourceServer.Service;
using Demo4DotNetCore.Tools;

namespace Demo4DotNetCore.ResourceServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IBookService service;
        private NLog.Logger logger;

        public TestController(IBookService service)
        {
            this.service = service;
            logger = NLog.LogManager.GetLogger(GetType().FullName);
        }

        [HttpGet]
        [Route("GetBooks")]
        public async Task<ActionResult> GetBooks()
        {
            var result = new OperationResult<PaginatedList<Book>>(true);
            try
            {
                var model = new BookRequestModel()
                {
                    Criteria = "",
                    PageIndex = 0,
                    PageSize = 0,
                    OrderBy = "",
                    Direction = "",
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

        // GET: api/Test
        [HttpGet]
        [Route("getb")]
        public Task<ActionResult> GetB()
        {
            var annoyCla1 = new
            {
                ID = 10010,
                Name = "BBB",
                Age = 25
            };
            return Task.FromResult<ActionResult>(new JsonResult(annoyCla1));
        }

        // GET: api/Test
        [HttpGet]
        [Route("geta")]
        public Task<ActionResult> GetA()
        {
            var annoyCla1 = new
            {
                ID = 10010,
                Name = "AAAAAA",
                Age = 25
            };
            return Task.FromResult<ActionResult>(new JsonResult(annoyCla1));
        }

        // GET: api/Test/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Test
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Test/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
