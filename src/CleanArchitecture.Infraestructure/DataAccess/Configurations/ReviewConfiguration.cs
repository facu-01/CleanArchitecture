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

        builder.Property(r => r.Id)
        .HasConversion(reviewId => reviewId.Value, value => new ReviewId(value));

        builder.OwnsOne(r => r.Rating,
            rB => rB.Property(ra => ra.Value)
                    .HasColumnName("rating")
            );


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
