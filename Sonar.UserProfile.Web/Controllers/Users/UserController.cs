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
    private readonly ILogger<UserController> _logger;
    private readonly IConfiguration _configuration;

    public UserController(IUserService userService, ILogger<UserController> logger, IConfiguration configuration)
    {
        _userService = userService;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Create new user with new token which will expire in 7 days.
    /// </summary>
    /// <param name="userAuthDto">DTO which contains parameters for new user: email, password.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>New token.</returns>
    [HttpPost("register")]
    public async Task<string> Register(
        [Required] UserAuthDto userAuthDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to register user");
        var user = new User
        {
            Email = userAuthDto.Email,
            Password = userAuthDto.Password
        };
        var str = await _userService.RegisterAsync(user, cancellationToken);
        _logger.LogInformation("User successfully registered");
        return str;
    }

    /// <summary>
    /// Generate new token to user if password matched. Token will expire in 7 days.
    /// </summary>
    /// <param name="userAuthDto">DTO which contains parameters to identify user: email, password</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>New token.</returns>
    [HttpPost("login")]
    public async Task<string> Login(
        [Required] UserAuthDto userAuthDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to login user");

        var user = new User
        {
            Email = userAuthDto.Email,
            Password = userAuthDto.Password,
        };

        var str = await _userService.LoginAsync(user, cancellationToken);
        _logger.LogInformation("User successfully logged in");
        return str;
    }

    /// <summary>
    /// Return a user model if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>User model which contains: Id, email, AccountType.</returns>
    [HttpGet("get")]
    [AuthorizationFilter]
    public async Task<UserDto> Get(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to get user");

        var userId = HttpExtensions.GetIdFromItems(HttpContext);
        var user = await _userService.GetByIdAsync(userId, cancellationToken);

        _logger.LogInformation("User successfully retrieved");
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            AccountType = user.AccountType,
            ConfirmStatus = user.ConfirmStatus
        };
    }

    /// <summary>
    /// Update a user model if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="userUpdateDto">User model which contains: Id, email, AccountType.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    [HttpPut("put")]
    [AuthorizationFilter]
    public async Task Update(
        [FromHeader(Name = "Token")] string token,
        [Required] UserUpdateDto userUpdateDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to update user");
        var userId = HttpExtensions.GetIdFromItems(HttpContext);

        await _userService.UpdateUserAsync(new User
            {
                Id = userId,
                Email = userUpdateDto.Email,
                Password = userUpdateDto.Password,
                AccountType = userUpdateDto.AccountType,
                ConfirmStatus = userUpdateDto.ConfirmStatus,
            },
            cancellationToken);

        _logger.LogInformation("User successfully updated");
    }

    /// <summary>
    /// Sends a new password to the email specified at registration if it is possible.
    /// </summary>
    /// <param name="userEmail">Mail for the password recovery.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    [HttpPut("recovery-password")]
    public async Task RecoveryPassword(
        [Required] string userEmail,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to recover password");

        await _userService.RecoverPasswordAsync(userEmail, cancellationToken);

        _logger.LogInformation("Password successfully recovered");
    }

    /// <summary>
    /// handles email confirmation links and updates user's status to confirmed if token is valid.
    /// </summary>
    /// <param name="confirmToken">token from route for mail confirmation.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    [HttpGet("confirm-mail/{confirmToken}")]
    public async Task ConfirmMail(
        [Required] string confirmToken,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to confirm mail");
        
        await _userService.ConfirmMailAsync(confirmToken, cancellationToken);
        
        _logger.LogInformation("Mail confirmation success");
    }
}