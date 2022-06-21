using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Sonar.UserProfile.Core.Domain.Users.Services.Interfaces;
using Sonar.UserProfile.Core.Domain.Users.ValueObjects;
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
    /// <param name="targetUserEmail">An email of user who you want to send request.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    [HttpPost("send-friendship-request")]
    [AuthorizationFilter]
    [SwaggerResponse(200)]
    [SwaggerResponse(400)]
    [SwaggerResponse(403)]
    [SwaggerResponse(404)]
    [SwaggerResponse(500)]
    public Task SendFriendshipRequest(
        [FromHeader(Name = "Token")] string token,
        [FromBody] [Required] string targetUserEmail,
        CancellationToken cancellationToken = default)
    {
        var userId = GetIdFromItems();
        return _relationshipService.SendFriendshipRequestAsync(userId, targetUserEmail, cancellationToken);
    }

    /// <summary>
    /// Return list of users who are in special relationship if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="relationshipStatus">Type of relationship. For example: friends.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>List of users. Every user is UserGetDto which contains: Id, Email.</returns>
    [HttpGet("get-relationships")]
    [AuthorizationFilter]
    [SwaggerResponse(200)]
    [SwaggerResponse(400)]
    [SwaggerResponse(403)]
    [SwaggerResponse(404)]
    [SwaggerResponse(500)]
    public async Task<IReadOnlyList<UserGetDto>> GetRelationships(
        [FromHeader(Name = "Token")] string token,
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken = default)
    {
        var userId = GetIdFromItems();
        var friends = await _relationshipService.GetRelationshipsAsync(
            userId,
            relationshipStatus,
            cancellationToken);

        return friends.Select(f => new UserGetDto
        {
            Id = f.Id,
            Email = f.Email
        }).ToList();
    }

    /// <summary>
    /// Accept friendship request if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="requestedEmail">An email of user who you want to accept.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    [HttpPost("accept-friendship-request")]
    [AuthorizationFilter]
    [SwaggerResponse(200)]
    [SwaggerResponse(400)]
    [SwaggerResponse(403)]
    [SwaggerResponse(404)]
    [SwaggerResponse(500)]
    public Task AcceptFriendshipRequest(
        [FromHeader(Name = "Token")] string token,
        [FromBody] [Required] string requestedEmail,
        CancellationToken cancellationToken = default)
    {
        var userId = GetIdFromItems();
        return _relationshipService.AcceptFriendshipRequestAsync(userId, requestedEmail, cancellationToken);
    }
    
    /// <summary>
    /// Reject friendship request if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="requestedEmail">An email of user who you want to reject.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    [HttpPost("reject-friendship-request")]
    [AuthorizationFilter]
    [SwaggerResponse(200)]
    [SwaggerResponse(400)]
    [SwaggerResponse(403)]
    [SwaggerResponse(404)]
    [SwaggerResponse(500)]
    public Task RejectFriendshipRequest(
        [FromHeader(Name = "Token")] string token,
        [FromBody] [Required] string requestedEmail,
        CancellationToken cancellationToken = default)
    {
        var userId = GetIdFromItems();
        return _relationshipService.RejectFriendshipRequestAsync(userId, requestedEmail, cancellationToken);
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