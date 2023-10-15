using BlazorHero.CleanArchitecture.Application.Configurations;
using BlazorHero.CleanArchitecture.Application.Interfaces.Serialization.Options;
using BlazorHero.CleanArchitecture.Application.Interfaces.Serialization.Serializers;
using BlazorHero.CleanArchitecture.Application.Interfaces.Serialization.Settings;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Serialization.JsonConverters;
using BlazorHero.CleanArchitecture.Application.Serialization.Options;
using BlazorHero.CleanArchitecture.Application.Serialization.Serializers;
using BlazorHero.CleanArchitecture.Application.Serialization.Settings;
using BlazorHero.CleanArchitecture.Server.Localization;
using BlazorHero.CleanArchitecture.Server.Managers.Preferences;
using BlazorHero.CleanArchitecture.Server.Services;
using BlazorHero.CleanArchitecture.Server.Settings;
using BlazorHero.CleanArchitecture.Shared.Constants.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static async Task<IStringLocalizer> GetRegisteredServerLocalizerAsync<T>(this IServiceCollection services) where T : class
    {
        var serviceProvider = services.BuildServiceProvider();
        await SetCultureFromServerPreferenceAsync(serviceProvider);
        var localizer = serviceProvider.GetService<IStringLocalizer<T>>();
        await serviceProvider.DisposeAsync();
        return localizer;
    }

    internal static IServiceCollection AddForwarding(this IServiceCollection services, IConfiguration configuration)
    {
        var applicationSettingsConfiguration = configuration.GetSection(nameof(AppConfiguration));
        var config = applicationSettingsConfiguration.Get<AppConfiguration>();     
        
        return services;
    }

    private static async Task SetCultureFromServerPreferenceAsync(IServiceProvider serviceProvider)
    {
        var storageService = serviceProvider.GetService<ServerPreferenceManager>();
        if (storageService != null)
        {
            // TODO - should implement ServerStorageProvider to work correctly!
            CultureInfo culture;
            if (await storageService.GetPreference() is ServerPreference preference)
                culture = new(preference.LanguageCode);
            else
                culture = new(LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }
    }

    internal static IServiceCollection AddServerLocalization(this IServiceCollection services)
    {
        services.TryAddTransient(typeof(IStringLocalizer<>), typeof(ServerLocalizer<>));
        return services;
    }

    internal static AppConfiguration GetApplicationSettings(
       this IServiceCollection services,
       IConfiguration configuration)
    {
        var applicationSettingsConfiguration = configuration.GetSection(nameof(AppConfiguration));
        services.Configure<AppConfiguration>(applicationSettingsConfiguration);
        return applicationSettingsConfiguration.Get<AppConfiguration>();
    }



    internal static IServiceCollection AddSerialization(this IServiceCollection services)
    {
        services
            .AddScoped<IJsonSerializerOptions, SystemTextJsonOptions>()
            .Configure<SystemTextJsonOptions>(configureOptions =>
            {
                if (!configureOptions.JsonSerializerOptions.Converters.Any(c => c.GetType() == typeof(TimespanJsonConverter)))
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
            });
        services.AddScoped<IJsonSerializerSettings, NewtonsoftJsonSettings>();

        services.AddScoped<IJsonSerializer, SystemTextJsonSerializer>(); // you can change it
        return services;
    }


 

    internal static IServiceCollection AddCurrentUserService(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }
   

 
}