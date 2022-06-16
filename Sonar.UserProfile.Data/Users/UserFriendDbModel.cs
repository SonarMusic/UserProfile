namespace Sonar.UserProfile.Data.Users;

public class UserFriendDbModel
{
    public Guid UserId { get; set; }
    public virtual UserDbModel User { get; set; }
    public Guid FriendId { get; set; }
    public virtual UserDbModel Friend { get; set; }
}