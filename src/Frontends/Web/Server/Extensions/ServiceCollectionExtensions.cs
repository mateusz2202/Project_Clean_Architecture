using BlazorHero.CleanArchitecture.Application.Interfaces.Serialization.Options;
using BlazorHero.CleanArchitecture.Application.Interfaces.Serialization.Serializers;
using BlazorHero.CleanArchitecture.Application.Interfaces.Serialization.Settings;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Serialization.JsonConverters;
using BlazorHero.CleanArchitecture.Application.Serialization.Options;
using BlazorHero.CleanArchitecture.Application.Serialization.Serializers;
using BlazorHero.CleanArchitecture.Application.Serialization.Settings;
using BlazorHero.CleanArchitecture.Server.Localization;
using BlazorHero.CleanArchitecture.Server.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using System.Linq;


namespace BlazorHero.CleanArchitecture.Server.Extensions;

internal static class ServiceCollectionExtensions
{       

    internal static IServiceCollection AddServerLocalization(this IServiceCollection services)
    {
        services.TryAddTransient(typeof(IStringLocalizer<>), typeof(ServerLocalizer<>));
        return services;
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