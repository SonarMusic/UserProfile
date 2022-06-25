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
    private readonly IUserService _userService;
    private readonly ILogger<RelationshipController> _logger;

    public RelationshipController(
        IRelationshipService relationshipService,
        IUserService userService,
        ILogger<RelationshipController> logger)
    {
        _relationshipService = relationshipService;
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Send a friendship request if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="targetUserEmail">An email of user who you want to send request.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    [HttpPost("send-friendship-request")]
    [AuthorizationFilter]
    public async Task SendFriendshipRequest(
        [FromHeader(Name = "Token")] string token,
        [Required] string targetUserEmail,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to send friendship request");

        var userId = HttpExtensions.GetIdFromItems(HttpContext);
        await _relationshipService.SendFriendshipRequestAsync(userId, targetUserEmail, cancellationToken);
        _logger.LogInformation("Friendship request successfully sent");
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
        _logger.LogInformation("Trying to get user's outgoing requests");

        var userId = HttpExtensions.GetIdFromItems(HttpContext);
        var requests = await _relationshipService.GetRequestsFromUserAsync(userId, cancellationToken);

        var userDto = requests.Select(f => new UserDto
        {
            Id = f.Id,
            Email = f.Email
        }).ToList();

        _logger.LogInformation("Outgoing user's requests successfully retrieved");

        return userDto;
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
        _logger.LogInformation("Trying to get user's incoming requests");

        var userId = HttpExtensions.GetIdFromItems(HttpContext);
        var requests = await _relationshipService.GetRequestsToUserAsync(userId, cancellationToken);

        var userDto = requests.Select(f => new UserDto
        {
            Id = f.Id,
            Email = f.Email
        }).ToList();

        _logger.LogInformation("Incoming user's requests successfully retrieved");

        return userDto;
    }

    /// <summary>
    /// Accept friendship request if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="requestedEmail">An email of user who you want to accept.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    [HttpPatch("accept-friendship-request")]
    [AuthorizationFilter]
    public async Task AcceptFriendshipRequest(
        [FromHeader(Name = "Token")] string token,
        [Required] string requestedEmail,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to get accept friendship request");

        var userId = HttpExtensions.GetIdFromItems(HttpContext);
        await _relationshipService.AcceptFriendshipRequestAsync(userId, requestedEmail, cancellationToken);

        _logger.LogInformation("Friendship request successfully accepted");
    }

    /// <summary>
    /// Reject friendship request if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="requestedEmail">An email of user who you want to reject.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    [HttpPatch("reject-friendship-request")]
    [AuthorizationFilter]
    public async Task RejectFriendshipRequest(
        [FromHeader(Name = "Token")] string token,
        [Required] string requestedEmail,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to get reject friendship request");

        var userId = HttpExtensions.GetIdFromItems(HttpContext);
        await _relationshipService.RejectFriendshipRequestAsync(userId, requestedEmail, cancellationToken);

        _logger.LogInformation("Friendship request successfully rejected");
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
        _logger.LogInformation("Trying to get friends list");

        var userId = HttpExtensions.GetIdFromItems(HttpContext);
        var friends = await _relationshipService.GetUserFriendsAsync(userId, cancellationToken);

        var friendsDto = friends.Select(f => new UserDto
        {
            Id = f.Id,
            Email = f.Email
        }).ToList();
        _logger.LogInformation("Friends list successfully retrieved");
        return friendsDto;
    }

    /// <summary>
    /// Get bool statement if users are friends.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="friendId">Id of user, friendship with who you want to check.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>True if users are friends and false if opposite.</returns>
    [HttpGet("is-friends")]
    [AuthorizationFilter]
    public async Task<bool> IsFriends(
        [FromHeader(Name = "Token")] string token,
        Guid friendId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to get if users are friends");

        var userId = HttpExtensions.GetIdFromItems(HttpContext);
        var isFriends = await _relationshipService.IsFriends(userId, friendId, cancellationToken);

        _logger.LogInformation("Bool statement successfully retrieved");
        return isFriends;
    }
}