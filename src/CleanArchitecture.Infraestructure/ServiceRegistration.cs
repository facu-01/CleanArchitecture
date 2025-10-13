using System;
using CleanArchitecture.Application.Abstractions.DataAccess;
using CleanArchitecture.Application.Abstractions.Email;
using CleanArchitecture.Infraestructure.DataAccess;
using CleanArchitecture.Infraestructure.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infraestructure;


public static class ServiceRegistration
{

    public static IServiceCollection AddInfraestructure(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        services.AddTransient<IEmailService, EmailService>();

        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new ArgumentNullException(nameof(configuration));


        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention();

        }
        );

        services.AddScoped<IApplicationDbContext>(
            provider => provider.GetRequiredService<ApplicationDbContext>()
        );

        return services;
    }

}
