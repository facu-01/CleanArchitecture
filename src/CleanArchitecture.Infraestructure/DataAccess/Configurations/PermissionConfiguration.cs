using CleanArchitecture.Domain.Permissions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infraestructure.DataAccess.Configurations;
public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                permissionId => permissionId!.Value,
                value => new PermissionId(value));

        builder.Property(p => p.Nombre)
            .HasConversion(
                nombre => nombre!.Value,
                value => new Nombre(value));

        IEnumerable<Permission> permissions = Enum.GetValues<PermissionEnum>()
            .Select(pe => new Permission(
                new PermissionId((int)pe),
                new Nombre(pe.ToString())
            ));

        builder.HasData(permissions);
    }
}
