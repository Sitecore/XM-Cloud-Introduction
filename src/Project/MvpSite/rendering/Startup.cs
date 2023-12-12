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
using Mvp.Feature.Navigation.Extensions;
using Mvp.Feature.People.Exntesions;
using Mvp.Feature.Selections.Extensions;
using Mvp.Feature.Social.Extensions;
using Mvp.Feature.User.Extensions;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using Mvp.Project.MvpSite.Extensions;
using Sitecore.AspNet.ExperienceEditor;
using Sitecore.AspNet.RenderingEngine.Extensions;
using Sitecore.AspNet.RenderingEngine.Localization;
using Sitecore.LayoutService.Client.Extensions;
using Sitecore.LayoutService.Client.Newtonsoft.Extensions;

namespace Mvp.Project.MvpSite.Rendering
{
    public class Startup
    {
        private const string DefaultLanguage = "en";

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
              .AddGraphQlHandler("default", Configuration.DefaultSiteName!, Configuration.ExperienceEdgeToken!, Configuration.LayoutServiceUri!)
              .AsDefaultHandler();

            services.AddHttpContextAccessor();
            services.AddSitemap();
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
                    .AddFeatureSelections()
                    .AddDefaultPartialView("_ComponentNotFound");
              })
              // Includes forwarding of Scheme as X-Forwarded-Proto to the Layout Service, so that
              // Sitecore Media and other links have the correct scheme.
              .ForwardHeaders()

              // Enable support for the Experience Editor.
              .WithExperienceEditor(options => { options.JssEditingSecret = Configuration.JssEditingSecret; });

            // Register MVP Functionality specific services
            services.AddFeatureSocialServices()
                    .AddFeaturePeopleServices()
                    .AddFeatureSelectionsServices()
                    .AddFoundationDataFetchingServices();
            
            services.AddSession();

            // The following line enables Application Insights telemetry collection.
            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // When running behind HTTPS termination, set the request scheme according to forwarded protocol headers.
            // Also set the Request IP, so that it can be passed on to the Sitecore Layout Service for tracking and personalization.
            app.UseForwardedHeaders(ConfigureForwarding());
            app.UseSession();

            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Add redirects for old mvp pages
            RewriteOptions rewriteOptions = new RewriteOptions()
              .AddRedirect("mvps/(.*)", "Directory?fc_Year=$1")
              .AddRedirect("mvps$", "Directory")
              .AddRedirect("search(.*)", "Directory$1")
              .AddRedirect("Search(.*)", "Directory$1");
            app.UseRewriter(rewriteOptions);

            // The Experience Editor endpoint should not be enabled in production DMZ.
            // See the SDK documentation for details.
            if (Configuration.EnableExperienceEditor)
                // Enable the Sitecore Experience Editor POST endpoint.
                app.UseSitecoreExperienceEditor();

            // Standard ASP.NET Core routing and static file support.
            app.EnsureAcceptLanguageHeader();
            app.UseRouting();
            app.UseStaticFiles();

            // Enable ASP.NET Core Localization, which is required for Sitecore content localization.
            app.UseRequestLocalization(options =>
            {
                // If you add languages in Sitecore which this site / Rendering Host should support, add them here.
                List<CultureInfo> supportedCultures = new () { new CultureInfo(DefaultLanguage) };
                options.DefaultRequestCulture = new RequestCulture(DefaultLanguage, DefaultLanguage);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                // Allow culture to be resolved via standard Sitecore URL prefix and query string (sc_lang).
                options.UseSitecoreRequestLocalization();
            });

            // Enable Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();            
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
                    "Sitemap",
                    "sitemap.xml",
                    new { controller = "Default", action = "SiteMap" }
                );

                endpoints.MapOktaSigninRoute();

                // Enables the default Sitecore URL pattern with a language prefix.
                endpoints.MapSitecoreLocalizedRoute("sitecore", "Index", "Default");

                // Fall back to language-less routing as well, and use the default culture (en).
                endpoints.MapFallbackToController("Index", "Default");
            });
        }

        private static ForwardedHeadersOptions ConfigureForwarding()
        {
            ForwardedHeadersOptions options = new ()
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
