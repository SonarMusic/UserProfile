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
    /// <param name="userRegisterDto">Contains parameters for new user: email, password.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Token (ID of generated token to be precise).</returns>
    [HttpPost("register")]
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
    /// <param name="userLoginDto">Contains parameters to identify user: email, password</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Token (ID of generated token to be precise).</returns>
    /// <exception cref="InvalidPasswordException">Throw if password didn't match.</exception>
    /// <exception cref="UserNotFoundException">Throw if user with such ID doesn't exist.</exception>
    [HttpPatch("login")]
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
    /// Expire token (delete it).
    /// </summary>
    /// <param name="tokenHeader">Contains token (ID of token to be precise).</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Task</returns>
    /// <exception cref="InvalidRequestException">Throw if header doesn't contain token.</exception>
    /// <exception cref="ExpiredTokenException">Throw if token already has expired.</exception>
    /// <exception cref="UserNotFoundException">Throw if user with such ID doesn't exist.</exception>
    /// <exception cref="TokenNotFoundException">Throw if token with such ID doesn't exist.</exception>
    [HttpPatch("logout")]
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
    /// <param name="cancellationToken"></param>
    /// <returns>User model which contains: ID, email, password.</returns>
    /// <exception cref="InvalidRequestException">Throw if header doesn't contain token.</exception>
    /// <exception cref="ExpiredTokenException">Throw if token already has expired.</exception>
    /// <exception cref="UserNotFoundException">Throw if user with such ID doesn't exist.</exception>
    /// <exception cref="TokenNotFoundException">Throw if token with such ID doesn't exist.</exception>
    [HttpGet("get")]
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
            Email = user.Email,
            Password = user.Password,
        };
    }
}