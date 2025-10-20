using CleanArchitecture.Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infraestructure.DataAccess.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
        .HasConversion(userId => userId.Value, value => new UserId(value));

        builder.Property(u => u.Nombre)
            .HasMaxLength(200)
            .HasConversion(nombre => nombre.Value, value => new Nombre(value));

        builder.Property(u => u.Apellido)
            .HasMaxLength(200)
            .HasConversion(apellido => apellido.Value, value => new Apellido(value));

        builder.Property(u => u.Email)
            .HasMaxLength(400)
            .HasConversion(email => email.Value, value => new Domain.Users.Email(value));

        builder.Property(u => u.PasswordHash)
            .HasMaxLength(2000)
            .HasConversion(passwordHash => passwordHash.Value, value => new PasswordHash(value));

        builder.HasIndex(user => user.Email).IsUnique();

        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<UserRole>();

    }
}
