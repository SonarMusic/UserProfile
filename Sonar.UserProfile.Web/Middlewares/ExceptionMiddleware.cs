using System.Net;
using Sonar.UserProfile.Core.Domain.Exceptions;

namespace Sonar.UserProfile.Web.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (InvalidRequestException exception)
        {
            await SendStatusCodeWithMessageAsync(httpContext, HttpStatusCode.BadRequest, exception);
        }
        catch (EmailOccupiedException exception)
        {
            await SendStatusCodeWithMessageAsync(httpContext, HttpStatusCode.BadRequest, exception);
        }
        catch (InvalidPasswordException exception)
        {
            await SendStatusCodeWithMessageAsync(httpContext, HttpStatusCode.Unauthorized, exception);
        }
        catch (ExpiredTokenException exception)
        {
            await SendStatusCodeWithMessageAsync(httpContext, HttpStatusCode.Forbidden, exception);
        }
        catch (TokenNotFoundException exception)
        {
            await SendStatusCodeWithMessageAsync(httpContext, HttpStatusCode.NotFound, exception);
        }
        catch (UserNotFoundException exception)
        {
            await SendStatusCodeWithMessageAsync(httpContext, HttpStatusCode.NotFound, exception);
        }
        catch (Exception _)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }

    private Task SendStatusCodeWithMessageAsync(
        HttpContext httpContext, 
        HttpStatusCode httpStatusCode,
        Exception exception)
    {
        httpContext.Response.StatusCode = (int)httpStatusCode;
        return httpContext.Response.WriteAsJsonAsync(new
        {
            exception.Message
        });
    }
}