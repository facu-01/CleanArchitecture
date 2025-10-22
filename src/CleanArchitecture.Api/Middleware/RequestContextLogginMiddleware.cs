
using Serilog.Context;

namespace CleanArchitecture.Api.Middleware;

public class RequestContextLogginMiddleware
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";

    private readonly RequestDelegate _next;

    public RequestContextLogginMiddleware(RequestDelegate next)
    {
        _next = next;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        using (LogContext.PushProperty("CorrelationId", GetCorrelationId(context)))
        {
            await _next(context);
        }
    }

    private string GetCorrelationId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId);

        return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
    }
}
