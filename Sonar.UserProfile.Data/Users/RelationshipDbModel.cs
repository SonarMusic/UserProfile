using Sonar.UserProfile.Core.Domain.Users.ValueObjects;

namespace Sonar.UserProfile.Data.Users;

public class RelationshipDbModel
{
    public Guid SenderUserId { get; set; }
    public Guid TargetUserId { get; set; }
    public RelationshipStatus RelationshipStatus { get; set; }
}