using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews.Events;

public record ComentarioCreatedDomainEvent(Guid Id) : IDomainEvent;
