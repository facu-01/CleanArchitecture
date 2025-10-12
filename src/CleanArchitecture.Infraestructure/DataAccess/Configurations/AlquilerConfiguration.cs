using System;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infraestructure.DataAccess.Configurations;

internal sealed class AlquilerConfiguration : IEntityTypeConfiguration<Alquiler>
{
    public void Configure(EntityTypeBuilder<Alquiler> builder)
    {
        builder.ToTable("alquileres");
        builder.HasKey(a => a.Id);

        builder.OwnsOne(a => a.PrecioPorPeriodo, precioBuilder =>
        {
            precioBuilder.Property(moneda => moneda.TipoMoneda)
            .HasConversion(tipoMoneda => tipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo).Value);
        });

        builder.OwnsOne(a => a.PrecioMantenimiento, precioBuilder =>
        {
            precioBuilder.Property(moneda => moneda.TipoMoneda)
            .HasConversion(tipoMoneda => tipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo).Value);
        });

        builder.OwnsOne(a => a.PrecioAccesorios, precioBuilder =>
        {
            precioBuilder.Property(moneda => moneda.TipoMoneda)
            .HasConversion(tipoMoneda => tipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo).Value);
        });

        builder.OwnsOne(a => a.PrecioTotal, precioBuilder =>
        {
            precioBuilder.Property(moneda => moneda.TipoMoneda)
            .HasConversion(tipoMoneda => tipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo).Value);
        });

        builder.OwnsOne(a => a.Periodo);

        builder.HasOne<Vehiculo>()
            .WithMany()
            .HasForeignKey(a => a.VehiculoId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(a => a.UserId);


    }
}
