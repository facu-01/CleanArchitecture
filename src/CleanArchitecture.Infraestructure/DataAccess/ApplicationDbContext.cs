using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Reviews;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infraestructure.DataAccess;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{

    private readonly IPublisher _publisher;

    public ApplicationDbContext(DbContextOptions options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }
    public DbSet<Alquiler> Alquileres { get; set; }

    public DbSet<Review> Reviews { get; set; }

    public DbSet<Vehiculo> Vehiculos { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {

            var result = await base.SaveChangesAsync(cancellationToken);

            await PublishDomainEventsAsync();

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Ocurrio un error de concurrencia", ex);
        }

    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<IEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();
                entity.ClearDomainEvents();
                return domainEvents;
            }).ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }

    }
}
