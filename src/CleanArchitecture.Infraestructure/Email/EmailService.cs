using System;
using CleanArchitecture.Application.Abstractions.Email;

namespace CleanArchitecture.Infraestructure.Email;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync(Domain.Users.Email recipient, string subject, string body)
    {
        return Task.CompletedTask;
    }
}
