using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Okta.AspNetCore;

namespace Mvp.Feature.User.Extensions
{
    public static class StartUpExtensions
    {
        public static void AddFeatureUser(this IServiceCollection services, IConfiguration configuration)
        {
            var okta = configuration.GetSection("Okta").Get<OktaMvcOptions>();
                okta.Scope = new List<string> { "openid", "profile", "email" };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = new PathString("/user/signin");
            })            
            .AddOktaMvc(okta);
        }


        public static void UseFeatureUser(this IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "OktaSignin",
                    pattern: "user/{action=SignIn}",
                    defaults: new { controller = "User" });
            });
        }
    }
}