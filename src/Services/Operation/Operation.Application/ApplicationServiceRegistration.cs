using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Operation.Application.Common.Behaviours;
using System.Reflection;

namespace Operation.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddMediatR(x => x.RegisterServicesFromAssemblyContaining(typeof(ApplicationServiceRegistration)))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
}
