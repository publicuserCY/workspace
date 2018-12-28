using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo4DotNetCore.AuthorizationServer.Dto;
using Demo4DotNetCore.AuthorizationServer.Model;
using Demo4DotNetCore.AuthorizationServer.Service;
using Demo4DotNetCore.Tools;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Demo4DotNetCore.AuthorizationServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IIdentityService identityService;
        private ConfigurationDbContext cdb;

        public AdminController(IConfiguration configuration, IIdentityService identityService, ConfigurationDbContext cdb)
        {
            this.configuration = configuration;
            this.identityService = identityService;
            this.cdb = cdb;
        }

        // GET api/values
        [HttpPost]
        public async Task<ActionResult> InsertAccount(AccountDto dto)
        {

            var result = new OperationResult<ApplicationUser>(true);
            try
            {
                var account = await identityService.InsertAccount(dto);
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
