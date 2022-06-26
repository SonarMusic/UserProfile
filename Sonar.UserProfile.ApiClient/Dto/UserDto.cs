using Sonar.UserProfile.ApiClient.Tools;

namespace Sonar.UserProfile.ApiClient.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public AccountType AccountType { get; set; }
}