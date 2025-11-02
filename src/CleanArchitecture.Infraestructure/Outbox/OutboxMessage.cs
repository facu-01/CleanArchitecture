namespace CleanArchitecture.Infraestructure.Outbox;
public sealed class OutboxMessage
{
    public OutboxMessage(
        Guid id,
        DateTime occurredOnUtc,
        int maxRetryCount,
        int retryCount,
        string type,
        string content
        )
    {
        Id = id;
        OccurredOnUtc = occurredOnUtc;
        MaxRetryCount = maxRetryCount;
        RetryCount = retryCount;
        Type = type;
        Content = content;
    }

    public Guid Id { get; init; }
    public DateTime OccurredOnUtc { get; init; }
    public int MaxRetryCount { get; init; }
    public DateTime? ProcessedOnUtc { get; set; }
    public int RetryCount { get; set; }
    public string Type { get; init; }
    public string Content { get; init; }
    public string Error { get; set; } = string.Empty;
}
