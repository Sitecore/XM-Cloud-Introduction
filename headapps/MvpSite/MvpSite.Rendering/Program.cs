using System.Globalization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Rewrite;
using MvpSite.Rendering.AppSettings;
using MvpSite.Rendering.Extensions;
using Sitecore.AspNetCore.SDK.ExperienceEditor.Extensions;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Extensions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

const string DefaultLanguage = "en";

// ReSharper disable once CommentTypo - Well known name of the file
// Example of using ASP.NET Core configuration binding for various Sitecore Rendering Engine settings.
// Values can originate in appsettings.json, from environment variables, and more.
// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1
MvpSiteSettings? sitecoreSettings = builder.Configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>();
ArgumentNullException.ThrowIfNull(sitecoreSettings);

// This method gets called by the runtime. Use this method to add services to the container.
// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
builder.Services
    .AddRouting()

    // You must enable ASP.NET Core localization to utilize localized Sitecore content.
    .AddLocalization()
    .AddMvc();

if (sitecoreSettings.EnableLocalContainer)
{
    // Register the GraphQL version of the Sitecore Layout Service Client for use against local container endpoint
    builder.Services.AddSitecoreLayoutService()
        .AddGraphQlHandler("default", sitecoreSettings.DefaultSiteName!, sitecoreSettings.EdgeContextId!, sitecoreSettings.LocalContainerLayoutUri!)
        .AsDefaultHandler();
}
else
{
    // Register the GraphQL version of the Sitecore Layout Service Client for use against experience edge
    builder.Services.AddSitecoreLayoutService()
        .AddGraphQlWithContextHandler("default", sitecoreSettings.EdgeContextId!, siteName: sitecoreSettings.DefaultSiteName!)
        .AsDefaultHandler();
}

builder.Services.AddFeatureUser(builder.Configuration);

// Register the Sitecore Rendering Engine services.
builder.Services.AddSitecoreRenderingEngine(options =>
    {
        // Register your components here
        options
            .AddFeatureUser()
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
    .WithExperienceEditor(options => { options.JssEditingSecret = sitecoreSettings.EditingSecret; });

// Register MVP Functionality specific services
builder.Services.AddFeatureSocialServices()
    .AddFeaturePeopleServices()
    .AddFeatureSelectionsServices();

builder.Services.AddSession();

// The following line enables Application Insights telemetry collection.
builder.Services.AddApplicationInsightsTelemetry();

WebApplication app = builder.Build();

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
// When running behind HTTPS termination, set the request scheme according to forwarded protocol headers.
// Also set the Request IP, so that it can be passed on to the Sitecore Layout Service for tracking and personalization.
app.UseForwardedHeaders(new ForwardedHeadersOptions()
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,

    // ReSharper disable once CommentTypo - Actual product name
    // Allow forwarding of headers from Traefik in development & NGINX in k8s
    KnownNetworks = { },
    KnownProxies = { }
});
app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// ReSharper disable StringLiteralTypo - Uri segments
// Add redirects for old mvp pages
RewriteOptions rewriteOptions = new RewriteOptions()
    .AddRedirect("mvps/(.*)", "Directory?fc_year=$1")
    .AddRedirect("mvps$", "Directory")
    .AddRedirect("search(.*)", "Directory$1")
    .AddRedirect("Search(.*)", "Directory$1");
app.UseRewriter(rewriteOptions);
// ReSharper restore StringLiteralTypo - Uri segments

// The Experience Editor endpoint should not be enabled in production DMZ.
// See the SDK documentation for details.
if (sitecoreSettings.EnableEditingMode)
{
    // Enable the Sitecore Experience Editor POST endpoint.
    app.UseSitecoreExperienceEditor();
}

// Standard ASP.NET Core routing and static file support.
app.EnsureAcceptLanguageHeader();
app.UseRouting();
app.UseStaticFiles();

// Enable ASP.NET Core Localization, which is required for Sitecore content localization.
app.UseRequestLocalization(options =>
{
    // If you add languages in Sitecore which this site / Rendering Host should support, add them here.
    List<CultureInfo> supportedCultures = [new(DefaultLanguage)];
    options.DefaultRequestCulture = new RequestCulture(DefaultLanguage, DefaultLanguage);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    // Allow culture to be resolved via standard Sitecore URL prefix and query string (sc_lang).
    options.UseSitecoreRequestLocalization();
});

// Enable Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// ReSharper disable StringLiteralTypo - Uri segments
app.MapControllerRoute(
    "error",
    "error",
    new { controller = "Default", action = "Error" });

app.MapControllerRoute(
    "healthz",
    "healthz",
    new { controller = "Default", action = "Healthz" });

app.MapOktaSigninRoute();

app.MapControllerRoute(
    "MvpProfileWithCulture",
    "{culture?}/directory/profile/{name}",
    new { controller = "MvpProfile", action = "Index" });
app.MapControllerRoute(
    "MvpProfileCulturelessFallback",
    "directory/profile/{name}",
    new { controller = "MvpProfile", action = "Index" });

// Enables the default Sitecore URL pattern with a language prefix.
app.MapSitecoreLocalizedRoute("sitecore", "Index", "Default");

// Fall back to language-less routing as well, and use the default culture (en).
app.MapFallbackToController("Index", "Default");

// ReSharper restore StringLiteralTypo - Uri segments
app.Run();