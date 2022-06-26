using Sonar.UserProfile.Core.Domain.Users.ValueObjects;

namespace Sonar.UserProfile.Data.Users
{
    public class UserDbModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public AccountType AccountType { get; set; }
        public ConfirmStatus ConfirmStatus { get; set; }
    }
}