using Demo4DotNetCore.AuthorizationServer.Dto;
using Demo4DotNetCore.AuthorizationServer.Model;
using Demo4DotNetCore.AuthorizationServer.Service;
using Demo4DotNetCore.Tools;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Demo4DotNetCore.AuthorizationServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class AdminController : ControllerBase
    {
        private NLog.Logger logger;
        private IConfiguration Configuration { get; }
        private IIdentityService IdentityService { get; }

        public AdminController(IConfiguration configuration, IIdentityService identityService)
        {
            Configuration = configuration;
            IdentityService = identityService;
        }

        [HttpGet]
        public async Task<ActionResult> SelectApiResource(string criteria = "", int pageIndex = 0, int pageSize = 0, string orderBy = "", string direction = "")
        {
            var result = new OperationResult<PaginatedList<ApiResource>>(true);
            try
            {
                var model = new ApiResourceRequestModel()
                {
                    Criteria = criteria,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    Direction = direction
                };
                var list = await IdentityService.SelectApiResource(model);
                result.Data = list;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                result = new OperationResult<PaginatedList<ApiResource>>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> InsertAccount(AccountDto dto)
        {

            var result = new OperationResult<ApplicationUser>(true);
            try
            {
                var account = await IdentityService.InsertAccount(dto);
                result.Data = account;
            }
            catch (Exception ex)
            {
                result = new OperationResult<ApplicationUser>(ex.Message);
            }
            return new JsonResult(result);
        }
    }
}
