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
    //[Microsoft.AspNetCore.Authorization.Authorize]
    public class IdentityController : ControllerBase
    {
        private NLog.Logger logger;
        private IConfiguration Configuration { get; }
        private IIdentityService IdentityService { get; }

        public IdentityController(IConfiguration configuration, IIdentityService identityService)
        {
            Configuration = configuration;
            IdentityService = identityService;
        }

        #region ApiResource
        [HttpGet]
        public async Task<ActionResult> SelectApiResource([FromQuery] ApiResourceRequestModel model)
        {
            var result = new OperationResult<PaginatedList<ApiResource>>(true);
            try
            {
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
        public async Task<ActionResult> InsertApiResource(ApiResourceRequestModel model)
        {
            var result = new OperationResult<ApiResource>(true);
            try
            {
                var entity = await IdentityService.InsertApiResource(model);
                result.Data = entity;
            }
            catch (Exception ex)
            {
                result = new OperationResult<ApiResource>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateApiResource(ApiResourceRequestModel model)
        {
            var result = new OperationResult<ApiResource>(true);
            try
            {
                var entity = await IdentityService.UpdateApiResource(model);
                result.Data = entity;
            }
            catch (Exception ex)
            {
                result = new OperationResult<ApiResource>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteApiResource(ApiResourceRequestModel model)
        {

            var result = new OperationResult<ApiResource>(true);
            try
            {
                var entity = await IdentityService.DeleteApiResource(model);
                result.Data = entity;
            }
            catch (Exception ex)
            {
                result = new OperationResult<ApiResource>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<ActionResult> UniqueApiResourceName(int id, string name)
        {
            var result = await IdentityService.UniqueApiResourceName(id, name);
            return new JsonResult(result);
        }
        #endregion

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
