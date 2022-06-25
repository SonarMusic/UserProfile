using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace Sonar.UserProfile.Web.Filters;

public class AuthorizationFilter : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var encodedToken = context.HttpContext.Request.Headers["Token"].FirstOrDefault();
        if (encodedToken is null)
        {
            context.Result = new JsonResult("Header must contain token.")
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            return;
        }

        JwtSecurityToken jwtToken;
        try
        {
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var securityKey = configuration["Secret"];
            SecurityToken token;
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(encodedToken, new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey))
            }, out token);
            jwtToken = (JwtSecurityToken)token;
        }
        catch (Exception _)
        {
            context.Result = new JsonResult("Invalid token.")
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            return;
        }

        var expiration = jwtToken.Payload.Exp;
        if (expiration is null)
        {
            context.Result = new JsonResult("Payload must have exp field.")
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            return;
        }

        if (DateTime.UnixEpoch.AddSeconds((double)expiration) < DateTime.UtcNow)
        {
            context.Result = new JsonResult($"Your token has expired on {DateTime.UnixEpoch.AddSeconds((double)expiration)}.")
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        var userId = jwtToken.Payload.Claims.FirstOrDefault(c => c.Type == "nameid");
        if (userId is null)
        {
            context.Result = new JsonResult("Payload must has claim with name identifier.")
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            return;
        }
        
        context.HttpContext.Items.Add("SenderUserId", Guid.Parse(userId.Value));
    }
}