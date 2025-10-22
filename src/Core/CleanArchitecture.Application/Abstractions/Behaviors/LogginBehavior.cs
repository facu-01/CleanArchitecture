using CleanArchitecture.Domain.Abstractions;

using MediatR;

using Microsoft.Extensions.Logging;

using Serilog.Context;

namespace CleanArchitecture.Application.Abstractions.Behaviors;

public class LogginBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IBaseRequest
where TResponse : Result
{
    private readonly ILogger<LogginBehavior<TRequest, TResponse>> _logger;

    public LogginBehavior(ILogger<LogginBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = request.GetType().FullName;

        try
        {
            _logger.LogInformation("Ejecutando el request: {name}", requestName);
            var result = await next();

            if (result.IsSuccess)
            {
                _logger.LogInformation("El request: {name}, se ejecuto correctamente", requestName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    _logger.LogError("El request: {name}, se dio error", requestName);
                }

            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al ejecutar el request: {name}", requestName);
            throw;
        }

    }
}
