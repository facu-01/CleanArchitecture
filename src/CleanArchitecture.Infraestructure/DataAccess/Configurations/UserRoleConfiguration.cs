using CleanArchitecture.Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infraestructure.DataAccess.Configurations;
public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{

    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_roles");

        builder.HasKey(ur => new { ur.UserId, ur.RoleId });

        builder.Property(ur => ur.UserId)
            .HasConversion(userId => userId!.Value, value => new UserId(value));
    }
}
