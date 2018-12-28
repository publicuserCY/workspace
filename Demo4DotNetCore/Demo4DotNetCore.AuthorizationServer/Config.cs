// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Demo4DotNetCore.AuthorizationServer
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("resapi", "Resource API")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                { 
                    ClientId = "resource-owner-client",
                    ClientName="Resource Owner Client",
                    ClientUri = "http://localhost:4200",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    AccessTokenLifetime = 180,
                    RedirectUris =
                    {
                        "http://localhost:4200/home"
                    },
                    PostLogoutRedirectUris = { "http://localhost:4200/home" },
                    AllowedCorsOrigins = { "http://localhost:4200" },
                    // ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "openid","profile","resapi" },
                    AllowOfflineAccess = true,
                    RequireClientSecret = false
                },
                new Client
                {
                    ClientId = "implicit-client",
                    ClientName="Implicit Client",
                    ClientUri = "http://localhost:4200",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    AccessTokenLifetime = 180,
                    RedirectUris =
                    {
                        "http://localhost:4200/home"
                    },
                    PostLogoutRedirectUris = { "http://localhost:4200/home" },
                    AllowedCorsOrigins = { "http://localhost:4200" },
                    // ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "openid","profile","resapi" },
                    AllowOfflineAccess = true,
                    RequireClientSecret = false
                }
            };
        }
    }
}