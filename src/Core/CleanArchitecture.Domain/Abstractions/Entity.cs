using System;

namespace CleanArchitecture.Domain.Abstractions;

public abstract class Entity
{
    protected Entity() { }

    public Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; init; }

    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }


}
