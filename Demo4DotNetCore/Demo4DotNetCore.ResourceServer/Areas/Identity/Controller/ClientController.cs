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
    public class ClientController : ControllerBase
    {
        private ILogger<ClientController> Logger { get; }
        private IClientService Service { get; }

        public ClientController(ILogger<ClientController> logger, IClientService service)
        {
            Logger = logger;
            Service = service;
        }

        [HttpGet]
        public async Task<ActionResult> Retrieve([FromQuery] ClientRequestModel model)
        {
            var result = new OperationResult<PaginatedResult<IdentityServer4.EntityFramework.Entities.Client>>(true);
            try
            {
                result.Data = await Service.Retrieve(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<PaginatedResult<IdentityServer4.EntityFramework.Entities.Client>>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<ActionResult> Single([FromQuery] ClientRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.Client>(true);
            try
            {
                result.Data = await Service.Single(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.Client>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Add(ClientRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.Client>(true);
            try
            {
                result.Data = await Service.Add(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.Client>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Modify(ClientRequestModel model)
        {
            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.Client>(true);
            try
            {
                result.Data = await Service.Modify(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.Client>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(ClientRequestModel model)
        {

            var result = new OperationResult<IdentityServer4.EntityFramework.Entities.Client>(true);
            try
            {
                result.Data = await Service.Delete(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = new OperationResult<IdentityServer4.EntityFramework.Entities.Client>(ex.Message);
            }
            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<ActionResult> UniqueClientId(int id, string clientId)
        {
            try
            {
                var result = await Service.UniqueClientId(id, clientId);
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
