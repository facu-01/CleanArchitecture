using System;
using CleanArchitecture.Application.Abstractions.Behaviors;
using CleanArchitecture.Domain.Alquileres;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application;

public static class ServiceRegistratrion
{

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(ServiceRegistratrion).Assembly);
            config.AddOpenBehavior(typeof(LogginBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ServiceRegistratrion).Assembly);

        services.AddTransient<PrecioService>();

        return services;
    }

}
