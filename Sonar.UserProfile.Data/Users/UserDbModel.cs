namespace Sonar.UserProfile.Data.Users
{
    public class UserDbModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual IEnumerable<UserFriendDbModel> Friends { get; set; }
    }
}