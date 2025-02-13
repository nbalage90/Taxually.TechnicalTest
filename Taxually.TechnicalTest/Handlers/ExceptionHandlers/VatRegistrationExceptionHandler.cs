using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Exceptions;

namespace Taxually.TechnicalTest.Handlers.ExceptionHandlers;

public class VatRegistrationExceptionHandler(ILogger<VatRegistrationExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("Error Message: {ExceptionMessage}, Time of occurrence {time}", exception.Message, DateTime.UtcNow);

        (string Details, string Title, int StatusCode) = exception switch
        {
            CountryNotSupportedException =>
            (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status404NotFound
            ),
            _ =>
            (
               exception.Message,
                exception.GetType().Name,
                StatusCodes.Status500InternalServerError
            )
        };

        var problemDetails = new ProblemDetails
        {
            Detail = Details,
            Title = Title,
            Status = StatusCode
        };

        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
