using Microsoft.AspNetCore.Mvc;
using Sonar.UserProfile.Web.Controllers.UserProfile.Dto;

namespace Sonar.UserProfile.Web.Controllers.UserProfile;

[ApiController]
[Route("[controller]")]
public class UserProfileController : ControllerBase
{
    [HttpPatch("login/{id:guid}")]
    public Task Login(Guid id, string password, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
    [HttpPatch("logout/{id:guid}")]
    public Task Logout(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("{id:guid}")]
    public Task<UserProfileDto> Get(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}