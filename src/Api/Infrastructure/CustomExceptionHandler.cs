﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using QuestSystem.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuestSystem.Web.Infrastructure;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

    public CustomExceptionHandler()
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new()
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(EntityNotFoundException), HandleNotFoundException },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
                { typeof(ExternalServiceException), HandleIntegrationErrorException },
                { typeof(PlatformQuestNotCompletedException), HandlePlatformQuestNotCompleted },
            };
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();

        if (_exceptionHandlers.ContainsKey(exceptionType))
        {
            await _exceptionHandlers[exceptionType].Invoke(httpContext, exception);
            return true;
        }

        return false;
    }

    private async Task HandleValidationException(HttpContext httpContext, Exception ex)
    {
        var exception = (ValidationException)ex;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(exception.Errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        });
    }
    
    private async Task HandlePlatformQuestNotCompleted(HttpContext httpContext, Exception ex)
    {
        var exception = (PlatformQuestNotCompletedException)ex;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "User didn't complete the quest",
            Detail = exception.Message
        });
    }

    private async Task HandleNotFoundException(HttpContext httpContext, Exception ex)
    {
        var exception = (NotFoundException)ex;

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
        {
            Status = StatusCodes.Status404NotFound,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = exception.Message
        });
    }

    private async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
        });
    }

    private async Task HandleForbiddenAccessException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Forbidden",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
        });
    }
    
    private async Task HandleIntegrationErrorException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status502BadGateway;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status502BadGateway,
            Title = "Integration Error",
            Detail = "There was an error communicating with the external platform. Please try again later.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.3",
            Instance = httpContext.Request.Path,
            Extensions = 
            {
                { "traceId", httpContext.TraceIdentifier }
            }
        });
    }
}
