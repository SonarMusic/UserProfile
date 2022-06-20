using Sonar.UserProfile.Data.Users.ValueObjects;

namespace Sonar.UserProfile.Data.Users;

public class RelationshipDbModel
{
    public Guid UserId { get; set; }
    public Guid FriendId { get; set; }
    public RelationshipStatus RelationshipStatus { get; set; }
}