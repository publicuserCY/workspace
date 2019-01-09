using Demo4DotNetCore.AuthorizationServer.Model;
using Demo4DotNetCore.AuthorizationServer.RequestModel;
using Demo4DotNetCore.AuthorizationServer.Service;
using Demo4DotNetCore.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Demo4DotNetCore.AuthorizationServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Microsoft.AspNetCore.Authorization.Authorize]
    public class IdentityController : ControllerBase
    {
        private ILogger<IdentityController> Logger { get; }
        private IConfiguration Configuration { get; }
        private IIdentityService IdentityService { get; }
        private ApiScopeService ApiScopeService { get; }

        public IdentityController(
            IConfiguration configuration, ILogger<IdentityController> logger,
            IIdentityService identityService,
            ApiScopeService apiScopeService)
        {
            Configuration = configuration;
            IdentityService = identityService;
            Logger = logger;
        }

        #region ApiResource
        [HttpGet]
        public async Task<ActionResult> RetrieveApiResource([FromQuery] ApiResourceRequestModel model)
        {
            var result = new OperationResult<PaginatedResult<IdentityServer4.EntityFramework.Entities.ApiResource>>(true);
            try
            {
                var list = await IdentityService.RetrieveApiResource(model);
                result.Data = list;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<PaginatedResult<IdentityServer4.EntityFramework.Entities.ApiResource>>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddApiResource(ApiResourceRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiResource>(true);
            try
            {
                var entity = await IdentityService.AddApiResource(model);
                result.Data = entity;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiResource>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyApiResource(ApiResourceRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiResource>(true);
            try
            {
                var entity = await IdentityService.ModifyApiResource(model);
                result.Data = entity;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiResource>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteApiResource(ApiResourceRequestModel model)
        {

            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiResource>(true);
            try
            {
                var entity = await IdentityService.DeleteApiResource(model);
                result.Data = entity;
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
            var result = await IdentityService.UniqueApiResourceName(id, name);
            return new JsonResult(result);
        }
        #endregion

        #region ApiSecret
        public async Task<ActionResult> AddApiSecret(ApiSecretRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiSecret>(true);
            try
            {
                var entity = await IdentityService.AddApiSecret(model);
                result.Data = entity;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiSecret>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyApiSecret(ApiSecretRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiSecret>(true);
            try
            {
                var entity = await IdentityService.ModifyApiSecret(model);
                result.Data = entity;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiSecret>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteApiSecret(ApiSecretRequestModel model)
        {

            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiSecret>(true);
            try
            {
                var entity = await IdentityService.DeleteApiSecret(model);
                result.Data = entity;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiSecret>(ex.Message);
            }
            return new JsonResult(result);
        }
        #endregion

        #region ApiScope
        public async Task<ActionResult> AddApiScope(ApiScopeRequestModel model)
        {
            //var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiScope>(true);
            //try
            //{
            //    var entity = await IdentityService.AddApiScope(model);
            //    result.Data = entity;
            //}
            //catch (Exception ex)
            //{
            //    Logger.LogError(ex.Message);
            //    result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiScope>(ex.Message);
            //}
            //return new JsonResult(result);
            var result = await ApiScopeService.Add(model);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyApiScope(ApiScopeRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiScope>(true);
            try
            {
                var entity = await IdentityService.ModifyApiScope(model);
                result.Data = entity;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiScope>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteApiScope(ApiScopeRequestModel model)
        {

            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiScope>(true);
            try
            {
                var entity = await IdentityService.DeleteApiScope(model);
                result.Data = entity;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.ApiScope>(ex.Message);
            }
            return new JsonResult(result);
        }
        #endregion
    }
}
