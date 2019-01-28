using Demo4DotNetCore.ResourceServer.Identity.RequestModel;
using Demo4DotNetCore.ResourceServer.Identity.Service;
using Demo4DotNetCore.ResourceServer.Model;
using Demo4DotNetCore.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Demo4DotNetCore.AuthorizationServer.Controllers
{
    [Route("api/{area}/[controller]/[action]")]
    [ApiController]
    public class ApiResourceController : ControllerBase
    {
        private ILogger<ApiResourceController> Logger { get; }
        private IApiResourceService Service { get; }

        public ApiResourceController(ILogger<ApiResourceController> logger, IApiResourceService service)
        {
            Logger = logger;
            Service = service;
        }

        [HttpGet]
        public async Task<ActionResult> Retrieve([FromQuery] ApiResourceRequestModel model)
        {
            var result = new OperationResult<PaginatedResult<IdentityServer4.EntityFramework.Entities.ApiResource>>(true);
            try
            {
                result.Data = await Service.Retrieve(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<PaginatedResult<IdentityServer4.EntityFramework.Entities.ApiResource>>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<ActionResult> Single([FromQuery] ApiResourceRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiResource>(true);
            try
            {
                result.Data = await Service.Single(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiResource>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Add(ApiResourceRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiResource>(true);
            try
            {
                result.Data = await Service.Add(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiResource>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Modify(ApiResourceRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiResource>(true);
            try
            {
                result.Data = await Service.Modify(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiResource>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(ApiResourceRequestModel model)
        {

            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiResource>(true);
            try
            {
                result.Data = await Service.Delete(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiResource>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<ActionResult> UniqueApiResourceName(int id, string name)
        {
            try
            {
                var result = await Service.UniqueApiResourceName(id, name);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult> UniqueApiScopeName(int id, string name)
        {
            try
            {
                var result = await Service.UniqueApiScopeName(id, name);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
    }
}
