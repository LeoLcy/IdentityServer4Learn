using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Leo.LearnIdentityServer4.Server
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>

            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };


        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                },
                new Client
                {
                    ClientId = "mvc_client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    //为ture，请求参数需要code_challenge和code_challenge_method值
                    //RequirePkce = true,
                    //AllowPlainTextPkce=true,

                    // where to redirect to after login
                    //
                    //RedirectUris = { "https://localhost:8001/signin-oidc" },
                    // where to redirect to after logout
                    //PostLogoutRedirectUris = { "https://localhost:8001/signout-callback-oidc" },
                    //需要跳回的客户端地址
                    RedirectUris = { "http://localhost:8000/signin-oidc","https://localhost:8001/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:8000/signout-callback-oidc", "https://localhost:8001/signout-callback-oidc"  },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                    ,AllowOfflineAccess=true
                },
                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =           { "https://localhost:8001/callback.html" },
                    PostLogoutRedirectUris = { "https://localhost:8001/index.html" },
                    AllowedCorsOrigins =     { "https://localhost:8001" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                },
                new Client
                {
                    ClientId = "authorization_code",
                    // secret for authentication
                    ClientSecrets ={new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    //是否显示授权页
                    RequireConsent = true,
                    //为ture，请求参数需要code_challenge和code_challenge_method值
                    //RequirePkce = true,
                    //AllowPlainTextPkce=true,
                    //需要跳回的客户端地址
                    RedirectUris = { "http://localhost:8000/signin-oidc","https://localhost:8001/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:8000/signout-callback-oidc", "https://localhost:8001/signout-callback-oidc"  },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                    ,AllowOfflineAccess=true
                },
                new Client
                {
                    ClientId = "implicit",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    //是否显示授权页
                    RequireConsent = true,
                    AllowAccessTokensViaBrowser=true,
                    //需要跳回的客户端地址
                    RedirectUris = { "https://localhost:8001/implicit.html" },
                    AllowedScopes = new List<string>
                    {
                        "api1"
                    }
                    ,AllowOfflineAccess=true
                },
                new Client
                {
                    ClientId = "password_grant",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    ClientName = "password_grant Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api1"
                    },
                     AllowOfflineAccess=true
                },
                new Client
                {
                    ClientId = "client_credentials",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    ClientName = "client_credentials Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    //RefreshTokenExpiration = TokenExpiration.Absolute,//刷新令牌将在固定时间点到期
                    AbsoluteRefreshTokenLifetime = 2592000,//RefreshToken的最长生命周期,默认30天
                    RefreshTokenExpiration = TokenExpiration.Sliding,//刷新令牌时，将刷新RefreshToken的生命周期。RefreshToken的总生命周期不会超过AbsoluteRefreshTokenLifetime。
                    SlidingRefreshTokenLifetime = 3600,//以秒为单位滑动刷新令牌的生命周期。
                    AllowedScopes =
                    {
                        ////如果要获取id_token,必须在scopes中加上OpenId和Profile
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        //如果要获取refresh_tokens ,必须在scopes中加上OfflineAccess
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api1"
                    },
                    RefreshTokenUsage=TokenUsage.OneTimeOnly,
                    //如果要获取refresh_tokens ,必须把AllowOfflineAccess设置为true
                    AllowOfflineAccess=true
                }
            };

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "1",
                        Username = "alice",
                        Password = "password",
                        Claims = new []
                        {
                            new Claim("name", "Alice"),
                            new Claim("website", "https://alice.com")
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "2",
                        Username = "bob",
                        Password = "password",
                        Claims = new []
                        {
                            new Claim("name", "Bob"),
                            new Claim("website", "https://bob.com")
                        }
                    }
                };
        }
    }
}
