using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mvp.Feature.BasicContent.Extensions;
using Mvp.Feature.Forms.Extensions;
using Mvp.Feature.Navigation.Extensions;
using Mvp.Feature.People.Exntesions;
using Mvp.Feature.Social.Extensions;
using Mvp.Feature.User.Extensions;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using Sitecore.AspNet.ExperienceEditor;
using Sitecore.AspNet.RenderingEngine.Extensions;
using Sitecore.AspNet.RenderingEngine.Localization;
using Sitecore.LayoutService.Client.Extensions;
using Sitecore.LayoutService.Client.Newtonsoft.Extensions;

namespace Mvp.Project.MvpSite.Rendering
{
    public class Startup
    {
        private static readonly string _defaultLanguage = "en";
        public IConfiguration DotNetConfiguration { get; }
        private MvpSiteSettings Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            // Example of using ASP.NET Core configuration binding for various Sitecore Rendering Engine settings.
            // Values can originate in appsettings.json, from environment variables, and more.
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1
            Configuration = configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>();
            DotNetConfiguration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
              .AddRouting()
              // You must enable ASP.NET Core localization to utilize localized Sitecore content.
              .AddLocalization()
              .AddMvc()
              // At this time the Layout Service Client requires Json.NET due to limitations in System.Text.Json.
              .AddNewtonsoftJson(o => o.SerializerSettings.SetDefaults());

            // Register the GraphQL version of the Sitecore Layout Service Client for use against experience edge & local edge endpoint
            services.AddSitecoreLayoutService()
              .AddGraphQlHandler("default", Configuration.DefaultSiteName, Configuration.ExperienceEdgeToken, Configuration.LayoutServiceUri)
              .AsDefaultHandler();

            services.AddFeatureUser(DotNetConfiguration);

            // Register the Sitecore Rendering Engine services.
            services.AddSitecoreRenderingEngine(options =>
              {
                  //Register your components here
                  options
                    .AddFoundationUser()
                    .AddFeatureNavigation()
                    .AddFeatureBasicContent()
                    .AddFeatureSocial()
                    .AddFeaturePeople()
                    .AddFeatureForms()
                    .AddDefaultPartialView("_ComponentNotFound");
              })
              // Includes forwarding of Scheme as X-Forwarded-Proto to the Layout Service, so that
              // Sitecore Media and other links have the correct scheme.
              .ForwardHeaders()

              // Enable support for the Experience Editor.
              .WithExperienceEditor();

            // Register MVP Functionality specific services
            services.AddFeatureSocialServices()
                    .AddFeaturePeopleServices()
                    .AddFeatureFormsServices()
                    .AddFoundationDataFetchingServices();
            
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // When running behind HTTPS termination, set the request scheme according to forwarded protocol headers.
            // Also set the Request IP, so that it can be passed on to the Sitecore Layout Service for tracking and personalization.
            app.UseForwardedHeaders(ConfigureForwarding(env));
            app.UseSession();

            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Add recirects for old mvp pages
            var options = new RewriteOptions()
              .AddRedirect("mvps/(.*)", "Directory?fc_personyear=$1")
              .AddRedirect("mvps$", "Directory")
              .AddRedirect("Search(.*)", "Directory$1");
            app.UseRewriter(options);

            // The Experience Editor endpoint should not be enabled in production DMZ.
            // See the SDK documentation for details.
            if (Configuration.EnableExperienceEditor)
                // Enable the Sitecore Experience Editor POST endpoint.
                app.UseSitecoreExperienceEditor();

            // Standard ASP.NET Core routing and static file support.
            app.UseRouting();
            app.UseStaticFiles();

            // Enable ASP.NET Core Localization, which is required for Sitecore content localization.
            app.UseRequestLocalization(options =>
            {
                // If you add languages in Sitecore which this site / Rendering Host should support, add them here.
                var supportedCultures = new List<CultureInfo> { new CultureInfo(_defaultLanguage) };
                options.DefaultRequestCulture = new RequestCulture(_defaultLanguage, _defaultLanguage);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                // Allow culture to be resolved via standard Sitecore URL prefix and query string (sc_lang).
                options.UseSitecoreRequestLocalization();
            });

            // Configure Okta Integration
            app.UseFeatureUser();
            app.UseFeatureForms();

            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                  "error",
                  "error",
                  new { controller = "Default", action = "Error" }
                );

                    endpoints.MapControllerRoute(
                  "healthz",
                  "healthz",
                  new { controller = "Default", action = "Healthz" }
                );
                    endpoints.MapControllerRoute(
                  "getCategories",
                  "getCategories",
                  new { controller = "Application", action = "GetCategories" }
                );

                // Enables the default Sitecore URL pattern with a language prefix.
                endpoints.MapSitecoreLocalizedRoute("sitecore", "Index", "Default");

                // Fall back to language-less routing as well, and use the default culture (en).
                endpoints.MapFallbackToController("Index", "Default");
            });
        }

        private ForwardedHeadersOptions ConfigureForwarding(IWebHostEnvironment env)
        {
            var options = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };

            // Allow forwarding of headers from Traefik in development & NGINX in k8s
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();

            return options;
        }
    }
}