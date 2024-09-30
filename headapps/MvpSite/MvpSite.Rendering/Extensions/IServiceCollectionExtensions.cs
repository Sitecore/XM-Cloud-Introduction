using Mvp.Selections.Client.Extensions;
using Mvp.Selections.Client.Interfaces;
using MvpSite.Rendering.Configuration;
using MvpSite.Rendering.FeedReader;
using MvpSite.Rendering.Providers;

namespace MvpSite.Rendering.Extensions;

// ReSharper disable once InconsistentNaming - This class extends the interface, not the type
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddFeaturePeopleServices(this IServiceCollection services)
    {
        services.AddOptions<MvpPeopleOptions>().Configure<IConfiguration>((options, configuration) =>
            configuration.GetSection(MvpPeopleOptions.MvpPeople).Bind(options));

        return services;
    }

    public static IServiceCollection AddFeatureSocialServices(this IServiceCollection services)
    {
        services.AddTransient<IFeedReader, RssFeedReader>();
        return services;
    }

    public static IServiceCollection AddFeatureSelectionsServices(this IServiceCollection services)
    {
        services.AddOptions<MvpSelectionsOptions>().Configure<IConfiguration>((options, configuration) =>
            configuration.GetSection(MvpSelectionsOptions.MvpSelections).Bind(options));
        services.AddScoped<ITokenProvider, HttpContextTokenProvider>();
        services.AddMvpSelectionsApiClient();
        return services;
    }
}