using Bogus;

using CleanArchitecture.Domain.Roles;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Infraestructure.DataAccess;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Api.Extensions;

public static class SeedDataExtensions
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();

        try
        {
            var context = service.GetRequiredService<ApplicationDbContext>();

            if (!context.Vehiculos.Any())
            {
                var faker = new Faker();

                List<object> vehiculos = new();

                foreach (var item in Enumerable.Range(1, 51))
                {
                    var Id = Guid.NewGuid();
                    var Vin = faker.Vehicle.Vin();
                    var Modelo = faker.Vehicle.Model();
                    var Pais = faker.Address.Country();
                    var Departamento = faker.Address.State();
                    var Provincia = faker.Address.County();
                    var Ciudad = faker.Address.City();
                    var Calle = faker.Address.StreetAddress();
                    var PrecioMonto = faker.Random.Decimal(1000, 20000);
                    var PrecioTipoMoneda = "USD";
                    var PrecioMantenimiento = faker.Random.Decimal(100, 200);
                    var PrecioMantenimientoTipoMoneda = "USD";
                    var Accesorios = new List<int> { (int)Accesorio.Wifi, (int)Accesorio.AppleCar };
                    var FechaUltimo = DateTime.MinValue;

                    context.Database.ExecuteSql($"""
                    INSERT INTO public.vehiculos
                        (id, vin, modelo, direccion_pais, direccion_departamento, direccion_provincia, direccion_ciudad, direccion_calle, precio_monto, precio_tipo_moneda, mantenimiento_monto, mantenimiento_tipo_moneda, accesorios, fecha_ultimo_alquiler)
                        values({Id}, {Vin}, {Modelo}, {Pais}, {Departamento}, {Provincia}, {Ciudad}, {Calle}, {PrecioMonto}, {PrecioTipoMoneda}, {PrecioMantenimiento}, {PrecioMantenimientoTipoMoneda}, {Accesorios}, {FechaUltimo})
                """);
                }
            }


        }
        catch (Exception ex)
        {

            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "Error en data seed");
        }
    }


    public static void SeedDataUsers(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();

        try
        {
            var context = service.GetRequiredService<ApplicationDbContext>();

            if (!context.Users.Any())
            {
                var passwordHasher = new PasswordHasher<object>();

                var user = User.Registrar(
                    new Nombre("test"),
                    new Apellido("test"),
                    new Email("test@test.com"),
                    new PasswordHash(passwordHasher.HashPassword(null!, "test"))
                );

                var roleUser = context.Set<Role>().First(r => r.Id == Role.User.Id);

                user.Roles =
                [
                    roleUser
                ];

                context.Users.Add(user);

                var admin = User.Registrar(
                    new Nombre("admin"),
                    new Apellido("admin"),
                    new Email("admin@test.com"),
                    new PasswordHash(passwordHasher.HashPassword(null!, "admin"))
                );

                var roleAdmin = context.Set<Role>().First(r => r.Id == Role.Administrator.Id);

                admin.Roles =
                [
                    roleAdmin
                ];


                context.Users.Add(admin);

                context.SaveChanges();


            }

        }
        catch (Exception ex)
        {

            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "Error en data seed");
        }

    }
}
