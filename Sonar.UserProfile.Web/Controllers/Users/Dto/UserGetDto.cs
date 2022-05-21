using Sonar.UserProfile.Core.Domain.ValueObjects;

namespace Sonar.UserProfile.Web.Controllers.Users.Dto;

public class UserGetDto
{
    public Guid Id { get; set; }
    public string Password { get; set; }
    public Token Token { get; set; }
}