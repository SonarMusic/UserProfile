using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Sonar.UserProfile.Core.Domain.Users.Services.Interfaces;
using Sonar.UserProfile.Web.Controllers.Users.Dto;
using Sonar.UserProfile.Web.Filters;

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
    /// Send a friendship request if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="targetUserEmail">An email of user who you want to send request.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    [HttpPost("send-friendship-request")]
    [AuthorizationFilter]
    public Task SendFriendshipRequest(
        [FromHeader(Name = "Token")] string token,
        [Required] string targetUserEmail,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpExtensions.GetIdFromItems(HttpContext);
        return _relationshipService.SendFriendshipRequestAsync(userId, targetUserEmail, cancellationToken);
    }

    /// <summary>
    /// Return list of user's friends if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>List of users. Every user is UserDto which contains: Id, Email.</returns>
    [HttpGet("get-friends")]
    [AuthorizationFilter]
    public async Task<IReadOnlyList<UserDto>> GetFriends(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpExtensions.GetIdFromItems(HttpContext);
        var friends = await _relationshipService.GetUserFriendsAsync(userId, cancellationToken);

        return friends.Select(f => new UserDto
        {
            Id = f.Id,
            Email = f.Email
        }).ToList();
    }
    
    /// <summary>
    /// Return list of users who you've send request.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>List of users. Every user is UserDto which contains: Id, Email.</returns>
    [HttpGet("get-requests-from-me")]
    [AuthorizationFilter]
    public async Task<IReadOnlyList<UserDto>> GetRequestsFromMe(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpExtensions.GetIdFromItems(HttpContext);
        var requests = await _relationshipService.GetRequestsFromUserAsync(userId, cancellationToken);

        return requests.Select(f => new UserDto
        {
            Id = f.Id,
            Email = f.Email
        }).ToList();
    }
    
    /// <summary>
    /// Return list of users who have send request to you.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>List of users. Every user is UserDto which contains: Id, Email.</returns>
    [HttpGet("get-requests-to-me")]
    [AuthorizationFilter]
    public async Task<IReadOnlyList<UserDto>> GetRequestsToMe(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpExtensions.GetIdFromItems(HttpContext);
        var requests = await _relationshipService.GetRequestsToUserAsync(userId, cancellationToken);

        return requests.Select(f => new UserDto
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
    [HttpPatch("accept-friendship-request")]
    [AuthorizationFilter]
    public Task AcceptFriendshipRequest(
        [FromHeader(Name = "Token")] string token,
        [Required] string requestedEmail,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpExtensions.GetIdFromItems(HttpContext);
        return _relationshipService.AcceptFriendshipRequestAsync(userId, requestedEmail, cancellationToken);
    }

    /// <summary>
    /// Reject friendship request if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="requestedEmail">An email of user who you want to reject.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    [HttpPatch("reject-friendship-request")]
    [AuthorizationFilter]
    public Task RejectFriendshipRequest(
        [FromHeader(Name = "Token")] string token,
        [Required] string requestedEmail,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpExtensions.GetIdFromItems(HttpContext);
        return _relationshipService.RejectFriendshipRequestAsync(userId, requestedEmail, cancellationToken);
    }
}