using Sonar.UserProfile.Core.Domain.ValueObjects;

namespace Sonar.UserProfile.Data.Users
{
    public class UserDbModel
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public virtual Token Token { get; set; }
    }
}