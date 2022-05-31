using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sonar.UserProfile.Core.Domain.Exceptions;

namespace Sonar.UserProfile.Web.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {

        context.Result = new JsonResult(context.Exception.Message)
        {
            StatusCode = context.Exception switch
            {
                InvalidRequestException => StatusCodes.Status400BadRequest,
                EmailOccupiedException => StatusCodes.Status400BadRequest,
                InvalidPasswordException => StatusCodes.Status401Unauthorized,
                ExpiredTokenException => StatusCodes.Status403Forbidden,
                TokenNotFoundException => StatusCodes.Status404NotFound,
                UserNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            }
        };
        context.ExceptionHandled = true;
    }
}