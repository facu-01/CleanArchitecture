using System;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infraestructure.DataAccess.Configurations;

internal sealed class VehiculoConfiguration : IEntityTypeConfiguration<Vehiculo>
{
    public void Configure(EntityTypeBuilder<Vehiculo> builder)
    {
        builder.ToTable("vehiculos");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
        .HasConversion(vehiculoId => vehiculoId.Value, value => new VehiculoId(value));


        builder.OwnsOne(v => v.Direccion);

        builder.Property(v => v.Modelo)
            .HasMaxLength(200)
            .HasConversion(modelo => modelo.Value,
                           value => new Modelo(value));

        builder.Property(v => v.Vin)
            .HasMaxLength(500)
            .HasConversion(vin => vin.Value,
                           value => new Vin(value));

        builder.OwnsOne(v => v.Precio,
        priceBuilder =>
        {
            priceBuilder.Property(moneda => moneda.TipoMoneda)
                        .HasConversion(tipoMoneda => tipoMoneda.Codigo,
                                       codigo => TipoMoneda.FromCodigo(codigo).Value);
        });


        builder.OwnsOne(v => v.Mantenimiento,
        priceBuilder =>
        {
            priceBuilder.Property(moneda => moneda.TipoMoneda)
                        .HasConversion(tipoMoneda => tipoMoneda.Codigo,
                                       codigo => TipoMoneda.FromCodigo(codigo).Value);
        });

        builder.Property<uint>("Version").IsRowVersion();

    }
}
