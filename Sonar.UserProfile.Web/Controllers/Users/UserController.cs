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


    [HttpPatch("login")]
    public Task Login(UserDto userDto, CancellationToken cancellationToken = default)
    {
        var user = new User
        {
            Id = userDto.Id,
            Password = userDto.Password
        };

        return _userService.Login(user, cancellationToken);
    }

    [HttpPatch("logout/{id:guid}")]
    public Task Logout(Guid id, CancellationToken cancellationToken = default)
    {
        return _userService.Logout(id, cancellationToken);
    }

    [HttpGet("{id:guid}")]
    public async Task<UserDto> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var userProfileDto = await _userService.GetById(id, cancellationToken);

        return new UserDto
        {
            Id = userProfileDto.Id,
            Password = userProfileDto.Password
        };
    }
}