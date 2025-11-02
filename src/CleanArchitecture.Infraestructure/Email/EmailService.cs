using CleanArchitecture.Application.Abstractions.Email;

using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infraestructure.Email;

internal sealed class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(Domain.Users.Email recipient, string subject, string body)
    {
        _logger.LogInformation("Sending email to {Email} with subject {Subject}", recipient.Value, subject);
        return Task.CompletedTask;
    }
}
