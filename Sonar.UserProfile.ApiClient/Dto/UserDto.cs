using Sonar.UserProfile.ApiClient.ValueObjects;

namespace Sonar.UserProfile.ApiClient.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public AccountType AccountType { get; set; }
    public ConfirmStatus ConfirmStatus { get; set; } = ConfirmStatus.Unconfirmed;
}