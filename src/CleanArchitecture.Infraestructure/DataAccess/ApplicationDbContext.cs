using System.Text.Json;

using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Reviews;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Infraestructure.Outbox;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infraestructure.DataAccess;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{


    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Alquiler> Alquileres { get; set; }

    public DbSet<Review> Reviews { get; set; }

    public DbSet<Vehiculo> Vehiculos { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {

            await RegisterOutboxMessages();

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Ocurrio un error de concurrencia", ex);
        }

    }

    private async Task RegisterOutboxMessages()
    {
        var domainEvents = ChangeTracker
            .Entries<IEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();
                entity.ClearDomainEvents();
                return domainEvents;
            }).Select(
                domainEvent =>
                {
                    var content = JsonSerializer.Serialize(
                            domainEvent,
                            OutboxMessageContentJsonSerializer.Options
                        );

                    return new OutboxMessage(
                        Guid.NewGuid(),
                        DateTime.UtcNow,
                        domainEvent.MaxRetries,
                        0,
                        domainEvent.GetType().Name,
                        content
                    );
                }
            ).ToList();

        await AddRangeAsync(domainEvents);
    }
}
