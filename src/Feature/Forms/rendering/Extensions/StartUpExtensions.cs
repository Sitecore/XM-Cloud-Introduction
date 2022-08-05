using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mvp.Feature.Forms.ApplicationData;

namespace Mvp.Feature.Forms.Extensions
{
    public static class StartUpExtensions
    {

        public static void UseFeatureForms(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                      name: "getApplicationlists",
                      pattern: "application/getapplicationlists",
                      new { controller = "Application", action = "GetApplicationLists" }
                 );

                endpoints.MapControllerRoute(
                      name: "getApplicationInfo",
                      pattern: "application/getapplicationinfo",
                      new { controller = "Application", action = "GetApplicationInfo" }
                 );
            });
        }

        public static IServiceCollection AddFeatureFormsServices(this IServiceCollection services)
        {
            services.AddTransient<IApplicationDataService, ApplicationDataService>();
            return services;
        }
    }
}