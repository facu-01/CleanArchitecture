using CleanArchitecture.Domain.Roles;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infraestructure.DataAccess.Configurations;
internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.Property(r => r.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasData(Role.GetValues());

        builder.HasMany(r => r.Permissions)
            .WithMany()
            .UsingEntity<RolePermissionConfiguration>();

    }
}
