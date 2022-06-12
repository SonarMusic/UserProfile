using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sonar.UserProfile.Web.Filters;

public class AuthorizationFilter : Attribute, IResourceFilter
{
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        var encodedToken = context.HttpContext.Request.Headers["Token"].FirstOrDefault();
        if (encodedToken is null)
        {
            context.Result = new ContentResult
            {
                Content = "Header must contain token."
            };
            return;
        }

        JwtSecurityToken token;
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            token = tokenHandler.ReadJwtToken(encodedToken);
        }
        catch (Exception _)
        {
            context.Result = new ContentResult
            {
                Content = "Incorrect token."
            };
            return;
        }

        var expiration = token.Payload.Exp;
        if (expiration is null)
        {
            context.Result = new ContentResult
            {
                Content = "Payload must has exp field."
            };
            return;
        }

        if (DateTime.UnixEpoch.AddSeconds((double)expiration) < DateTime.UtcNow)
        {
            context.Result = new ContentResult
            {
                Content = $"Your token has expired on {DateTime.UnixEpoch.AddSeconds((double)expiration)}."
            };
            return;
        }

        var userId = token.Payload.Claims.FirstOrDefault(c => c.Type == "nameid");
        if (userId is null)
        {
            context.Result = new ContentResult
            {
                Content = "Payload must has claim with name identifier."
            };
            return;
        }
        
        context.HttpContext.Items.Add("UserId", Guid.Parse(userId.Value));
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }
}