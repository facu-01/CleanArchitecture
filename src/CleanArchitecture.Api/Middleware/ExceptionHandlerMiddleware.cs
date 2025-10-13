using System;
using System.Net;
using CleanArchitecture.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Middleware;

public class ExceptionHandlerMiddleware
{

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {

            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrio una excepcion: {Message}", ex.Message);

            var exceptionDetails = GetExceptionDetails(ex);
            var problemDetails = new ProblemDetails
            {
                Status = (int?)exceptionDetails.StatusCode,
                Type = exceptionDetails.Type,
                Detail = exceptionDetails.Detail,
                Title = exceptionDetails.Title,
            };

            if (exceptionDetails.Errors is not null)
            {
                problemDetails.Extensions["errors"] = exceptionDetails.Errors;
            }

            httpContext.Response.StatusCode = (int)exceptionDetails.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private static ExceptionDetails GetExceptionDetails(Exception ex)
    {
        return ex switch
        {
            ValidationException validationException => new ExceptionDetails(
                HttpStatusCode.BadRequest,
                "ValidationFailure",
                "Error de Validacion",
                "han ocurrido errores de validacion",
                validationException.Errors
                ),
            _ => new ExceptionDetails(
                HttpStatusCode.InternalServerError,
                "InternalServerError",
                "Error de servidor",
                "han ocurrido un error en la app",
                null
            )
        };
    }

    internal record ExceptionDetails(HttpStatusCode StatusCode,
        string Type,
        string Title,
        string Detail,
        IEnumerable<object>? Errors
    );
}

