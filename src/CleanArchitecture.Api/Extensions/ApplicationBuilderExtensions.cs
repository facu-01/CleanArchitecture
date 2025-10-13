using System;
using System.Threading.Tasks;
using CleanArchitecture.Api.Middleware;
using CleanArchitecture.Application.Abstractions.DataAccess;
using CleanArchitecture.Infraestructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void ApplyMigration(this IApplicationBuilder app)
    {

        using var scope = app.ApplicationServices.CreateScope();

        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();

        try
        {
            var context = service.GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();

        }
        catch (Exception ex)
        {

            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "Error en migracion");
        }

    }

    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
    }

}
