using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace Gooios.UserService.Configurations
{
    public class DataConfiguration
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",

                  Claims = new List<Claim>(){new Claim(JwtClaimTypes.Role,"superadmin") }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com")
                    },
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {

            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "cookapp",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = new List<string>(){GrantTypes.ResourceOwnerPasswordAndClientCredentials.FirstOrDefault(), "verify_code","partner_auth_code","wechat_applet" },//GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = {   "gooiosapi" ,
                                        StandardScopes.OfflineAccess, //如果要获取refresh_tokens ,必须在scopes中加上OfflineAccess
                                        StandardScopes.OpenId,        //如果要获取id_token,必须在scopes中加上OpenId和Profile，id_token需要通过refresh_tokens获取AccessToken的时候才能拿到（还未找到原因）
                                        StandardScopes.Profile        //如果要获取id_token,必须在scopes中加上OpenId和Profile
                    },
                    AccessTokenLifetime = 3600 * 24 * 7,
                    AllowOfflineAccess =true,                         //如果要获取refresh_tokens ,必须把AllowOfflineAccess设置为true
                    AbsoluteRefreshTokenLifetime = 2592000,           //RefreshToken的最长生命周期,默认30天
                    RefreshTokenExpiration = TokenExpiration.Sliding, //刷新令牌时，将刷新RefreshToken的生命周期。RefreshToken的总生命周期不会超过AbsoluteRefreshTokenLifetime。
                    SlidingRefreshTokenLifetime = 3600 * 24 * 14,     //以秒为单位滑动刷新令牌的生命周期。
                                                                      //按照现有的设置，如果3600内没有使用RefreshToken，那么RefreshToken将失效。即便是在3600内一直有使用RefreshToken，RefreshToken的总生命周期不会超过30天。所有的时间都可以按实际需求调整。
                },
                 new Client
                {
                    ClientId = "cookapp2",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = {   "gooiosapi" ,
                                        StandardScopes.OfflineAccess, //如果要获取refresh_tokens ,必须在scopes中加上OfflineAccess
                                        StandardScopes.OpenId,        //如果要获取id_token,必须在scopes中加上OpenId和Profile，id_token需要通过refresh_tokens获取AccessToken的时候才能拿到（还未找到原因）
                                        StandardScopes.Profile        //如果要获取id_token,必须在scopes中加上OpenId和Profile
                    },
                    AccessTokenLifetime = 3600 * 24 * 7,
                    AllowOfflineAccess =true,                         //如果要获取refresh_tokens ,必须把AllowOfflineAccess设置为true
                    AbsoluteRefreshTokenLifetime = 2592000,           //RefreshToken的最长生命周期,默认30天
                    RefreshTokenExpiration = TokenExpiration.Sliding, //刷新令牌时，将刷新RefreshToken的生命周期。RefreshToken的总生命周期不会超过AbsoluteRefreshTokenLifetime。
                    SlidingRefreshTokenLifetime = 3600 * 24 * 14,     //以秒为单位滑动刷新令牌的生命周期。
                                                                      //按照现有的设置，如果3600内没有使用RefreshToken，那么RefreshToken将失效。即便是在3600内一直有使用RefreshToken，RefreshToken的总生命周期不会超过30天。所有的时间都可以按实际需求调整。
                },

                //,new Client
                //{
                //    ClientId = "mvc_code",
                //    ClientName = "MVC Code Client",
                //    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    RedirectUris = { "http://localhost:5002/signin-oidc" },
                //    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                //    AllowedScopes = new List<string>
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile,
                //        IdentityServerConstants.StandardScopes.Email,
                //        "gooiosapi"
                //    },
                //    AllowOfflineAccess = true,
                //    AllowAccessTokensViaBrowser = true
                //}
            };
        }
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
                new ApiResource("gooiosapi", "GOOIOS-API")
            };
        }
    }
}
