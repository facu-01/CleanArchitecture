using MediatR;

namespace CleanArchitecture.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    public int MaxRetries => 5;
}


