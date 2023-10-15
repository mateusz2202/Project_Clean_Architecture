using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BlazorHero.CleanArchitecture.Application;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());     
    }

}