using CleanArchitecture.Domain.Abstractions;

using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Extensions;

public static class ErrorsExtensions
{

    public static ProblemDetails ToProblemDetails(this Error error)
    {
        return new ProblemDetails
        {
            Type = error.Detail,
            Title = error.Code,
            Detail = error.Message
        };
    }

}
