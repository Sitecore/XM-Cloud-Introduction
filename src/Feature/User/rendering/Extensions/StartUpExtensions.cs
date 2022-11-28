using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mvp.Feature.User.Helpers;
using Okta.AspNetCore;

namespace Mvp.Feature.User.Extensions
{
    public static class StartUpExtensions
    {
        public static void AddFeatureUser(this IServiceCollection services, IConfiguration configuration)
        {
            OktaMvcOptions okta = configuration.GetSection("Okta").Get<OktaMvcOptions>();
                okta.Scope = new List<string> { "openid", "profile", "email", "offline_access" };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = new PathString("/user/signin");
                options.Events.OnValidatePrincipal += new RefreshTokenHelper(okta).OnValidatePrincipal;
            })            
            .AddOktaMvc(okta);
        }

        public static void MapOktaSigninRoute(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute(
                name: "OktaSignin",
                pattern: "user/{action=SignIn}",
                defaults: new { controller = "User" });
        }
    }
}