using System;

namespace CleanArchitecture.Domain.Abstractions;

public interface IEntity
{
    public IReadOnlyList<IDomainEvent> GetDomainEvents();
    public void ClearDomainEvents();
}
