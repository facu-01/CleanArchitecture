using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Users.Events;

public sealed record  UserRegisteredDomainEvent(Guid Id):IDomainEvent;
