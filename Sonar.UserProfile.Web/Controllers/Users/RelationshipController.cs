using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Sonar.UserProfile.Core.Domain.Users.Services;
using Sonar.UserProfile.Core.Domain.Users.Services.Interfaces;
using Sonar.UserProfile.Web.Controllers.Users.Dto;
using Sonar.UserProfile.Web.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace Sonar.UserProfile.Web.Controllers.Users;

[ApiController]
[Route("relationship")]
public class RelationshipController : ControllerBase
{
    private readonly IRelationshipService _relationshipService;

    public RelationshipController(IRelationshipService relationshipService)
    {
        _relationshipService = relationshipService;
    }
    
    /// <summary>
    /// Add a friend to user if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="friendEmail">An email if friend who you want to add.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    [HttpPost("add-friend")]
    [AuthorizationFilter]
    [SwaggerResponse(200)]
    [SwaggerResponse(400)]
    [SwaggerResponse(403)]
    [SwaggerResponse(404)]
    [SwaggerResponse(500)]
    public Task AddFriend(
        [FromHeader(Name = "Token")] string token,
        [FromBody] [Required] string friendEmail,
        CancellationToken cancellationToken = default)
    {
        var userId = GetIdFromItems();
        return _relationshipService.AddFriend(userId, friendEmail, cancellationToken);
    }

    /// <summary>
    /// Return a friend list if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>List of user's friends. Every friend is UserGetDto which contains: Id, Email.</returns>
    [HttpGet("get-friends")]
    [AuthorizationFilter]
    [SwaggerResponse(200)]
    [SwaggerResponse(400)]
    [SwaggerResponse(403)]
    [SwaggerResponse(404)]
    [SwaggerResponse(500)]
    public async Task<IReadOnlyList<UserGetDto>> GetFriends(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var userId = GetIdFromItems();
        var friends = await _relationshipService.GetFriendsById(userId, cancellationToken);

        return friends.Select(f => new UserGetDto
        {
            Id = f.Id,
            Email = f.Email
        }).ToList();
    }

    private Guid GetIdFromItems()
    {
        var userIdItem = HttpContext.Items["UserId"];

        if (userIdItem is null)
        {
            throw new Exception("Incorrect user id item");
        }

        return (Guid)userIdItem;
    }
}