using DekanPlus.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DekanPlus.WebApi.Middlewares;

public class GlobalExceptionsHandler(ILogger<GlobalExceptionsHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is OperationCanceledException && httpContext.RequestAborted.IsCancellationRequested)
            return true;

        var problemDetails = new ProblemDetails { Instance = httpContext.Request.Path };

        switch (exception)
        {
            case KeyNotFoundException:
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = "Key not found";
                problemDetails.Detail = exception.Message;
                break;

            case InvalidOperationException:
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Invalid operation";
                problemDetails.Detail = exception.Message;
                break;

            case ArgumentException:
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Invalid argument";
                problemDetails.Detail = exception.Message;
                break;

            case DuplicateValueException:
                problemDetails.Status = StatusCodes.Status409Conflict;
                problemDetails.Title = "Duplicate value";
                problemDetails.Detail = exception.Message;
                break;

            case ConcurrencyConflictException:
                problemDetails.Status = StatusCodes.Status409Conflict;
                problemDetails.Title = "Concurrency conflict";
                problemDetails.Detail = exception.Message;
                break;

            case EntityInUseException:
                problemDetails.Status = StatusCodes.Status409Conflict;
                problemDetails.Title = "Entity in use";
                problemDetails.Detail = exception.Message;
                break;

            default:
                logger.LogError(exception, "Unhandled exception while processing {Path}", httpContext.Request.Path);
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Internal server error";
                problemDetails.Detail = "An unexpected exception occured";
                break;
        }

        if (problemDetails.Status < StatusCodes.Status500InternalServerError)
            logger.LogWarning("{Title}: {Detail} ({Path})", problemDetails.Title, problemDetails.Detail, httpContext.Request.Path);

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
