using Microsoft.AspNetCore.Mvc;
using Sonar.UserProfile.Core.Domain.Users.Services;
using Sonar.UserProfile.Core.Domain.Users;
using Sonar.UserProfile.Web.Controllers.Users.Dto;
using Sonar.UserProfile.Web.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace Sonar.UserProfile.Web.Controllers.Users;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Create new user with new token which will expire in 7 days.
    /// </summary>
    /// <param name="userRegisterDto">DTO which contains parameters for new user: email, password.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>New token.</returns>
    [HttpPost("register")]
    [SwaggerResponse(200)]
    [SwaggerResponse(400)]
    [SwaggerResponse(500)]
    public Task<string> Register(UserRegisterDto userRegisterDto, CancellationToken cancellationToken = default)
    {
        var user = new User
        {
            Email = userRegisterDto.Email,
            Password = userRegisterDto.Password
        };

        return _userService.RegisterAsync(user, cancellationToken);
    }

    /// <summary>
    /// Generate new token to user if password matched. Token will expire in 7 days.
    /// </summary>
    /// <param name="userLoginDto">DTO which contains parameters to identify user: email, password</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>New token.</returns>
    [HttpPatch("login")]
    [SwaggerResponse(200)]
    [SwaggerResponse(401)]
    [SwaggerResponse(404)]
    [SwaggerResponse(500)]
    public Task<string> Login(UserLoginDto userLoginDto, CancellationToken cancellationToken = default)
    {
        var user = new User
        {
            Email = userLoginDto.Email,
            Password = userLoginDto.Password
        };

        return _userService.LoginAsync(user, cancellationToken);
    }

    /// <summary>
    /// Return a user model if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>User model which contains: ID, email.</returns>
    [HttpGet("get")]
    [AuthorizationFilter]
    [SwaggerResponse(200)]
    [SwaggerResponse(400)]
    [SwaggerResponse(403)]
    [SwaggerResponse(404)]
    [SwaggerResponse(500)]
    public async Task<UserGetDto> Get(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var userIdItem = HttpContext.Items["UserId"];

        if (userIdItem is null)
        {
            throw new Exception("Incorrect user id item");
        }

        var userId = (Guid)userIdItem;
        var user = await _userService.GetByIdAsync(userId, cancellationToken);

        return new UserGetDto
        {
            Id = user.Id,
            Email = user.Email,
            Friends = user.Friends.Select(f => new UserGetDto
            {
                Id = f.Id,
                Email = f.Email
            }).ToList()
        };
    }

    /// <summary>
    /// Add a friend to user if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="friendEmail">An email if friend who you want to add.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    [HttpPatch("add-friend")]
    [AuthorizationFilter]
    [SwaggerResponse(200)]
    [SwaggerResponse(400)]
    [SwaggerResponse(403)]
    [SwaggerResponse(404)]
    [SwaggerResponse(500)]
    public Task AddFriend(
        [FromHeader(Name = "Token")] string token,
        string friendEmail, 
        CancellationToken cancellationToken = default)
    {
        var userIdItem = HttpContext.Items["UserId"];

        if (userIdItem is null)
        {
            throw new Exception("Incorrect user id item");
        }

        var userId = (Guid)userIdItem;
        return _userService.AddFriend(userId, friendEmail, cancellationToken);
    }
}