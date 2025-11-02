using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Users.Events;

public sealed record UserRegisteredDomainEvent(UserId Id) : IDomainEvent
{
    public int MaxRetries => 2;
}
