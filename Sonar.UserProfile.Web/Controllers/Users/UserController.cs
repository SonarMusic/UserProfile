using Microsoft.AspNetCore.Mvc;
using Sonar.UserProfile.Core.Domain.Exceptions;
using Sonar.UserProfile.Core.Domain.Users.Services;
using Sonar.UserProfile.Core.Domain.Users;
using Sonar.UserProfile.Web.Controllers.Users.Dto;

namespace Sonar.UserProfile.Web.Controllers.Users;

[ApiController]
[Route("[controller]")]
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
    /// <returns>New token (ID of generated token to be precise).</returns>
    /// <response code="200">OK</response>
    /// <response code="500">Internal server exception.</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public Task<Guid> Register(UserRegisterDto userRegisterDto, CancellationToken cancellationToken = default)
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
    /// <returns>New token (ID of generated token to be precise).</returns>
    /// <response code="200">OK</response>
    /// <response code="401">Password didn't match.</response>
    /// <response code="404">User with such ID doesn't exist.</response>
    /// <response code="500">Internal server exception.</response>
    [HttpPatch("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public Task<Guid> Login(UserLoginDto userLoginDto, CancellationToken cancellationToken = default)
    {
        var user = new User
        {
            Email = userLoginDto.Email,
            Password = userLoginDto.Password
        };

        return _userService.LoginAsync(user, cancellationToken);
    }

    /// <summary>
    /// Delete token.
    /// </summary>
    /// <param name="tokenHeader">DTO which contains token (ID of token to be precise).</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>Task</returns>
    /// <response code="200">OK</response>
    /// <response code="400">Header doesn't contain token.</response>
    /// <response code="401">Password didn't match.</response>
    /// <response code="403">Token already has expired.</response>
    /// <response code="404">Entity with such ID doesn't exist.</response>
    /// <response code="500">Internal server exception.</response>
    [HttpPatch("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public Task Logout([FromHeader(Name = "Token")] string tokenHeader, CancellationToken cancellationToken = default)
    {
        if (tokenHeader is null)
        {
            throw new InvalidRequestException("Your header does not contain a token.");
        }

        var tokenId = Guid.Parse(tokenHeader);

        return _userService.Logout(tokenId, cancellationToken);
    }

    /// <summary>
    /// Return a user model if token hasn't expired yet.
    /// </summary>
    /// <param name="tokenHeader">Contains token (ID of token to be precise).</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>User model which contains: ID, email.</returns>
    /// <response code="200">OK</response>
    /// <response code="400">Header doesn't contain token.</response>
    /// <response code="403">Token already has expired.</response>
    /// <response code="404">Entity with such ID doesn't exist.</response>
    /// <response code="500">Internal server exception.</response>
    [HttpGet("get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<UserGetDto> Get([FromHeader(Name = "Token")] string tokenHeader, CancellationToken cancellationToken = default)
    {
        if (tokenHeader is null)
        {
            throw new InvalidRequestException("Your header does not contain a token.");
        }

        var tokenId = Guid.Parse(tokenHeader);

        var user = await _userService.GetByIdAsync(tokenId, cancellationToken);

        return new UserGetDto
        {
            Id = user.Id,
            Email = user.Email
        };
    }
}