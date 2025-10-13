using Bogus;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Infraestructure.DataAccess;
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
        catch (Exception ex)
        {

            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "Error en data seed");
        }
    }
}
