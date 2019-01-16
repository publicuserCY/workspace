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
    public class ApiSecretController : ControllerBase
    {
        private ILogger<ApiSecretController> Logger { get; }
        private IApiSecretService Service { get; }

        public ApiSecretController(ILogger<ApiSecretController> logger, IApiSecretService service)
        {
            Logger = logger;
            Service = service;
        }

        [HttpGet]
        public async Task<ActionResult> Single([FromQuery] ApiSecretRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiSecret>(true);
            try
            {
                result.Data = await Service.Single(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiSecret>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Add(ApiSecretRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiSecret>(true);
            try
            {
                result.Data = await Service.Add(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiSecret>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Modify(ApiSecretRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiSecret>(true);
            try
            {
                result.Data = await Service.Modify(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiSecret>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(ApiSecretRequestModel model)
        {

            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiSecret>(true);
            try
            {
                result.Data = await Service.Delete(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiSecret>(ex.Message);
            }
            return new JsonResult(result);
        }
    }
}
