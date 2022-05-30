namespace Sonar.UserProfile.Web.Controllers.Users.Dto;

public class UserGetDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}