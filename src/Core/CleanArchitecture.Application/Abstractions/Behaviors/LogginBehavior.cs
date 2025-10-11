using System;
using CleanArchitecture.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Abstractions.Behaviors;

public class LogginBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IBaseCommand
{
    private readonly ILogger<TRequest> _logger;

    public LogginBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var commandName = request.GetType().Name;

        try
        {
            _logger.LogInformation("Ejecutando el command request: {name}", commandName);
            var result = await next();
            _logger.LogInformation("El command: {name}, se ejecuto correctamente", commandName);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al ejecutar el command request: {name}", commandName);
            throw;
        }

    }
}
