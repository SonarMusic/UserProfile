using Microsoft.AspNetCore.Mvc;
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
    
    [HttpPost("register")]
    public Task<Guid> Register(UserRegisterDto userRegister, CancellationToken cancellationToken = default)
    {
        var user = new User
        {
            Password = userRegister.Password
        };

        return _userService.RegisterAsync(user, cancellationToken);
    }

    [HttpPatch("login")]
    public Task<Guid> Login(UserLoginDto userLoginDto, CancellationToken cancellationToken = default)
    {
        var user = new User
        {
            Id = userLoginDto.Id,
            Password = userLoginDto.Password
        };

        return _userService.LoginAsync(user, cancellationToken);
    }

    [HttpPatch("logout/{tokenId:guid}")]
    public Task Logout(Guid tokenId, CancellationToken cancellationToken = default)
    {
        return _userService.Logout(tokenId, cancellationToken);
    }

    [HttpGet("get/{tokenId:guid}")]
    public async Task<UserGetDto> Get(Guid tokenId, CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetByIdAsync(tokenId, cancellationToken);

        return new UserGetDto
        {
            Id = user.Id,
            Password = user.Password,
        };
    }
}