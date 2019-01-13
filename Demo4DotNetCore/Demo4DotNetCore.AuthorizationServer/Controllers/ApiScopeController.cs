using Demo4DotNetCore.AuthorizationServer.RequestModel;
using Demo4DotNetCore.AuthorizationServer.Service;
using Demo4DotNetCore.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Demo4DotNetCore.AuthorizationServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Microsoft.AspNetCore.Authorization.Authorize]
    public class ApiScopeController : ControllerBase
    {
        private ILogger<ApiScopeController> Logger { get; }
        private IApiScopeService Service { get; }

        public ApiScopeController(ILogger<ApiScopeController> logger, IApiScopeService service)
        {
            Logger = logger;
            Service = service;
        }

        [HttpGet]
        public async Task<ActionResult> Single([FromQuery] ApiScopeRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiScope>(true);
            try
            {
                result.Data = await Service.Single(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiScope>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Add(ApiScopeRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiScope>(true);
            try
            {
                result.Data = await Service.Add(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiScope>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Modify(ApiScopeRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiScope>(true);
            try
            {
                result.Data = await Service.Modify(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiScope>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(ApiScopeRequestModel model)
        {

            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiScope>(true);
            try
            {
                result.Data = await Service.Delete(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiScope>(ex.Message);
            }
            return new JsonResult(result);
        }
    }
}
