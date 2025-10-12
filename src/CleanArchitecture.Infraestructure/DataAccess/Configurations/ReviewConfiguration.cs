using System;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Reviews;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infraestructure.DataAccess.Configurations;

internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Rating)
            .HasConversion(rating => rating.Value, value => Rating.Create(value).Value);

        builder.Property(r => r.Comentario)
            .HasMaxLength(200)
            .HasConversion(comentario => comentario.Value, value => new Comentario(value));

        builder.HasOne<Vehiculo>()
        .WithMany()
        .HasForeignKey(r => r.VehiculoId);

        builder.HasOne<Alquiler>()
        .WithMany()
        .HasForeignKey(r => r.AlquilerId);

        builder.HasOne<User>()
        .WithMany()
        .HasForeignKey(r => r.UserId);



    }
}
