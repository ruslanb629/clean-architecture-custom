using Application.Common.Exceptions;
using Application.Common.Extentions;
using Application.Common.Models;
using System.Net;

namespace Api.Middlewares;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IDictionary<Type, Func<Exception, (BaseResponseVmException, HttpStatusCode)>> _exceptionHandlers;

    public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory
                  .CreateLogger<ExceptionMiddleware>();

        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Func<Exception, (BaseResponseVmException, HttpStatusCode)>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(DataNotFoundException), HandleDataNotFoundException },
                //{ typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                //{ typeof(ForbiddenAccessException), HandleForbiddenAccessException },
            };
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        (BaseResponseVmException, HttpStatusCode) result;

        context.Response.ContentType = "application/json";

        var type = exception.GetType();

        if (_exceptionHandlers.ContainsKey(type))
        {
            result = _exceptionHandlers[type].Invoke(exception);
        }
        else
        {
            result = HandleUnknownException(exception);
        }

        context.Response.StatusCode = (int)result.Item2;

        var response = new BaseResponseVm
        {
            Exception = result.Item1,
            Transaction = context.TraceIdentifier
        };

        _logger.LogError("Error: {@Error}", response.ToLogJson());

        await context.Response.WriteAsync(response.ToJson());
    }

    private (BaseResponseVmException, HttpStatusCode) HandleValidationException(Exception exception)
    {
        var validationException = (ValidationException)exception;

        return (new BaseResponseVmException
        {
            Code = ExceptionCode.ValidationError,
            Message = string.Join(", ", validationException.Errors.SelectMany(e => e.Value))
        }, HttpStatusCode.BadRequest);
    }

    private (BaseResponseVmException, HttpStatusCode) HandleDataNotFoundException(Exception exception)
    {
        return (new BaseResponseVmException
        {
            Code = ExceptionCode.BadRequest,
            Message = exception.Message
        }, HttpStatusCode.BadRequest);
    }

    //private void HandleInvalidModelStateException(ExceptionContext context)
    //{
    //    var details = new ValidationProblemDetails(context.ModelState)
    //    {
    //        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
    //    };

    //    context.Result = new BadRequestObjectResult(details);

    //    context.ExceptionHandled = true;
    //}

    //private void HandleUnauthorizedAccessException(ExceptionContext context)
    //{
    //    var details = new ProblemDetails
    //    {
    //        Status = StatusCodes.Status401Unauthorized,
    //        Title = "Unauthorized",
    //        Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
    //    };

    //    context.Result = new ObjectResult(details)
    //    {
    //        StatusCode = StatusCodes.Status401Unauthorized
    //    };

    //    context.ExceptionHandled = true;
    //}

    //private void HandleForbiddenAccessException(ExceptionContext context)
    //{
    //    var details = new ProblemDetails
    //    {
    //        Status = StatusCodes.Status403Forbidden,
    //        Title = "Forbidden",
    //        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
    //    };

    //    context.Result = new ObjectResult(details)
    //    {
    //        StatusCode = StatusCodes.Status403Forbidden
    //    };

    //    context.ExceptionHandled = true;
    //}

    private (BaseResponseVmException, HttpStatusCode) HandleUnknownException(Exception exception)
    {
        return (new BaseResponseVmException
        {
            Code = ExceptionCode.InternalServerError,
            Message = exception.GetAllMessages(),
            ErrorStack = exception.StackTrace
        }, HttpStatusCode.InternalServerError);
    }
}


public static class ExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}