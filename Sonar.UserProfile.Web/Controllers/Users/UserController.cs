using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Sonar.UserProfile.Core.Domain.Users;
using Sonar.UserProfile.Core.Domain.Users.Services.Interfaces;
using Sonar.UserProfile.Web.Controllers.Users.Dto;
using Sonar.UserProfile.Web.Filters;

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
    /// <param name="userAuthDto">DTO which contains parameters for new user: email, password.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>New token.</returns>
    [HttpPost("register")]
    public Task<string> Register(
        [Required] UserAuthDto userAuthDto,
        CancellationToken cancellationToken = default)
    {
        var user = new User
        {
            Email = userAuthDto.Email,
            Password = userAuthDto.Password
        };

        return _userService.RegisterAsync(user, cancellationToken);
    }

    /// <summary>
    /// Generate new token to user if password matched. Token will expire in 7 days.
    /// </summary>
    /// <param name="userAuthDto">DTO which contains parameters to identify user: email, password</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>New token.</returns>
    [HttpPatch("login")]
    public Task<string> Login(
        [Required] UserAuthDto userAuthDto,
        CancellationToken cancellationToken = default)
    {
        var user = new User
        {
            Email = userAuthDto.Email,
            Password = userAuthDto.Password
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
    public async Task<UserDto> Get(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpExtensions.GetIdFromItems(HttpContext);
        var user = await _userService.GetByIdAsync(userId, cancellationToken);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email
        };
    }

    [HttpPut("put")]
    [AuthorizationFilter]
    public Task Update(
        UserAuthDto userDto,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpExtensions.GetIdFromItems(HttpContext);
    
        return _userService.UpdateUserAsync(new User
            {
                Id = userId,
                Email = userDto.Email,
                Password = userDto.Password
            },
            cancellationToken);
    }
}