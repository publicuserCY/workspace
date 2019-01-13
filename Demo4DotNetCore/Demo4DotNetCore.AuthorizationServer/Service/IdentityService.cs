﻿using Demo4DotNetCore.AuthorizationServer.Model;
using Demo4DotNetCore.AuthorizationServer.RequestModel;
using IdentityServer4.EntityFramework.DbContexts;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Demo4DotNetCore.AuthorizationServer.Service
{
    public class IdentityService : IIdentityService
    {
        private UserManager<ApplicationUser> UserManager { get; }
        private ConfigurationDbContext ConfigurationDbContext { get; }

        public IdentityService(UserManager<ApplicationUser> userManager, ConfigurationDbContext configurationDbContext)
        {
            UserManager = userManager;
            ConfigurationDbContext = configurationDbContext;
        }

        //public Task<ApplicationUser> InsertAccount(AccountDto dto)
        //{
        //    var user = UserManager.FindByNameAsync(dto.UserName).Result;
        //    if (user != null)
        //    {
        //        throw new Exception("用户已存在");
        //    }
        //    user = new ApplicationUser()
        //    {
        //        UserName = dto.UserName
        //    };
        //    var result = UserManager.CreateAsync(user, dto.Password).Result;
        //    if (!result.Succeeded)
        //    {
        //        throw new Exception(result.Errors.First().Description);
        //    }
        //    user = UserManager.FindByNameAsync(dto.UserName).Result;
        //    return Task.FromResult(user);
        //}

        public ApplicationUser FindByExternalProvider(string provider, string userId)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser FindBySubjectId(string subjectId)
        {
            return UserManager.FindByIdAsync(subjectId).Result;
        }

        public ApplicationUser FindByUsername(string username)
        {
            return UserManager.FindByNameAsync(username).Result;
        }

        public bool ValidateCredentials(string username, string password)
        {
            var user = UserManager.FindByNameAsync(username).Result;
            return UserManager.CheckPasswordAsync(user, password).Result;
        }

        public ApplicationUser AutoProvisionUser(string provider, string userId, List<Claim> claims)
        {
            throw new NotImplementedException();
        }
    }
}
