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

    public async Task AddRelationshipAsync(
        Guid userId, 
        Guid targetUserId, 
        RelationshipStatus relationshipStatus, 
        CancellationToken cancellationToken)
    {
        var userEntity =
            await _context.Users.FirstOrDefaultAsync(it => it.Id == userId, cancellationToken);
        var targetUserEntity =
            await _context.Users.FirstOrDefaultAsync(it => it.Id == targetUserId, cancellationToken);

        if (userEntity is null)
        {
            throw new NotFoundException($"User with id = {userId} does not exists");
        }

        if (targetUserEntity is null)
        {
            throw new NotFoundException($"User with id = {targetUserId} does not exists");
        }

        _context.Relationships.Add(new RelationshipDbModel
        {
            SenderUserId = userId,
            TargetUserId = targetUserId,
            RelationshipStatus = RelationshipStatus.Request
        });

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetUsersInRelationshipFromUserAsync(
        Guid id,
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken)
    {
        var relationshipList = await _context.Relationships
            .Where(r => r.SenderUserId == id && r.RelationshipStatus == relationshipStatus)
            .Select(r => new User
            {
                Id = r.TargetUserId,
                Email = _context.Users.FirstOrDefault(f => f.Id == r.TargetUserId).Email
            }).ToListAsync(cancellationToken);

        return relationshipList;
    }

    public async Task<IReadOnlyList<User>> GetUsersInRelationshipToUserAsync(
        Guid id,
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken)
    {
        var relationshipList = await _context.Relationships
            .Where(r => r.TargetUserId == id && r.RelationshipStatus == relationshipStatus)
            .Select(r => new User
            {
                Id = r.SenderUserId,
                Email = _context.Users.FirstOrDefault(f => f.Id == r.SenderUserId).Email
            }).ToListAsync(cancellationToken);

        return relationshipList;
    }

    public async Task<RelationshipStatus> GetStatusAsync(
        Guid senderUserId,
        Guid targetUserId,
        CancellationToken cancellationToken)
    {
        var relationship = await _context.Relationships.FirstOrDefaultAsync(r =>
            r.SenderUserId == senderUserId && r.TargetUserId == targetUserId, cancellationToken);

        return relationship?.RelationshipStatus ?? RelationshipStatus.Absence;
    }

    public async Task UpdateStatusAsync(
        Guid senderUserId,
        Guid targetUserId,
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken)
    {
        var relationship = await _context.Relationships
            .FirstOrDefaultAsync(r =>
                r.SenderUserId == senderUserId && r.TargetUserId == targetUserId, cancellationToken);

        if (relationship is null)
        {
            throw new NotFoundException("There is no request from this user");
        }

        relationship.RelationshipStatus = relationshipStatus;
        _context.Relationships.Update(relationship);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid senderUserId, Guid targetUserId, CancellationToken cancellationToken)
    {
        var relationship = await _context.Relationships.FirstOrDefaultAsync(
            r => r.SenderUserId == senderUserId && r.TargetUserId == targetUserId, cancellationToken);

        if (relationship is null)
        {
            throw new NotFoundException("There is no such relationship.");
        }

        _context.Relationships.Remove(relationship);

        await _context.SaveChangesAsync(cancellationToken);
    }
}