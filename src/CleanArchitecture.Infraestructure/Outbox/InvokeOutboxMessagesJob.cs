using System.Text.Json;

using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Infraestructure.DataAccess;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace CleanArchitecture.Infraestructure.Outbox;
internal sealed class InvokeOutboxMessagesJob : BackgroundService
{

    private readonly ILogger<InvokeOutboxMessagesJob> _logger;
    private readonly OutboxOptions _options;
    private readonly IServiceProvider _serviceProvider;

    public InvokeOutboxMessagesJob(
        ILogger<InvokeOutboxMessagesJob> logger,
        IPublisher publisher,
        IOptions<OutboxOptions> options,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _options = options.Value;
        _serviceProvider = serviceProvider;
    }



    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {

        using PeriodicTimer timer = new(TimeSpan.FromSeconds(_options.IntervalInSeconds));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await ProcessOutboxMessages();
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
        }

    }

    private async Task ProcessOutboxMessages()
    {
        _logger.LogInformation("Iniciando el proceso de outbox messages");

        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        var sql = $@"
            select *
            from outbox_messages
            where processed_on_utc is null
                    and retry_count < max_retry_count
            order by occurred_on_utc  
            limit {_options.BatchSize}
            for update
        ";

        var outboxMessages = await dbContext.OutboxMessages
            .FromSqlRaw(sql)
            .ToListAsync();

        foreach (var message in outboxMessages)
        {
            var exception = null as Exception;

            try
            {
                var domainEvent = JsonSerializer.Deserialize<IDomainEvent>(
                    message.Content,
                    OutboxMessageContentJsonSerializer.Options);
                await mediator.Publish(domainEvent!);

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ex, "Error processing outbox message {OutboxMessageId}", message.Id);
            }

            UpdateOutboxMessage(message, exception);

        }

        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        _logger.LogInformation("Se ha completado el procesamiento del outbox");

    }

    private static void UpdateOutboxMessage(
        OutboxMessage outboxMessage,
        Exception? ex
        )
    {
        outboxMessage.RetryCount += 1;

        if (ex is null)
        {
            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
            outboxMessage.Error = string.Empty;
        }
        else
        {
            var stringBuilder = new System.Text.StringBuilder(outboxMessage.Error);
            stringBuilder.AppendLine("-- retry error");
            stringBuilder.AppendLine(ex.ToString());
            outboxMessage.Error = stringBuilder.ToString();
        }

    }

}
