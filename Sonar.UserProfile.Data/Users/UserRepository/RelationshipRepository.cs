using Microsoft.EntityFrameworkCore;
using Sonar.UserProfile.Core.Domain.Exceptions;
using Sonar.UserProfile.Core.Domain.Users;
using Sonar.UserProfile.Core.Domain.Users.Repositories;
using Sonar.UserProfile.Core.Domain.Users.ValueObjects;

namespace Sonar.UserProfile.Data.Users.UserRepository;

public class RelationshipRepository : IRelationshipRepository
{
    private readonly SonarContext _context;

    public RelationshipRepository(SonarContext context)
    {
        _context = context;
    }

    public async Task SendFriendshipRequestAsync(Guid userId, Guid targetUserId, CancellationToken cancellationToken)
    {
        var entityUser =
            await _context.Users.FirstOrDefaultAsync(it => it.Id == userId, cancellationToken);
        var entityFriend =
            await _context.Users.FirstOrDefaultAsync(it => it.Id == targetUserId, cancellationToken);

        if (entityUser is null)
        {
            throw new NotFoundException($"User with id = {userId} does not exists");
        }

        if (entityFriend is null)
        {
            throw new NotFoundException($"User with id = {targetUserId} does not exists");
        }

        _context.Relationships.Add(new RelationshipDbModel
        {
            UserId = userId,
            FriendId = targetUserId,
            RelationshipStatus = RelationshipStatus.Request
        });

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetRelationshipsAsync(
        Guid id,
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken)
    {
        var relationshipList = await _context.Relationships
            .Where(r => r.UserId == id && r.RelationshipStatus == relationshipStatus)
            .Select(r => new User
            {
                Id = r.FriendId,
                Email = _context.Users.FirstOrDefault(f => f.Id == r.FriendId).Email
            }).ToListAsync(cancellationToken);

        relationshipList.AddRange(await _context.Relationships
            .Where(r => r.FriendId == id && r.RelationshipStatus == relationshipStatus)
            .Select(r => new User
            {
                Id = r.UserId,
                Email = _context.Users.FirstOrDefault(f => f.Id == r.UserId).Email
            }).ToListAsync(cancellationToken));

        return relationshipList;
    }

    public Task<bool> IsRelationshipAsync(
        Guid userId,
        Guid friendId,
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken)
    {
        return _context.Relationships.AnyAsync(r =>
            (r.UserId == userId && r.FriendId == friendId || r.UserId == friendId && r.FriendId == userId) &&
            r.RelationshipStatus == relationshipStatus, cancellationToken);
    }
    
    public async Task ChangeRelationshipStatusAsync(
        Guid userId, 
        Guid requestedId, 
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken)
    {
        var relationship = await _context.Relationships
            .FirstOrDefaultAsync(r =>
                r.UserId == requestedId && r.FriendId == userId &&
                r.RelationshipStatus == RelationshipStatus.Request, cancellationToken);

        if (relationship is null)
        {
            throw new NotFoundException("There is no request from this user");
        }

        relationship.RelationshipStatus = relationshipStatus;
        _context.Relationships.Update(relationship);
        await _context.SaveChangesAsync(cancellationToken);
    }
}