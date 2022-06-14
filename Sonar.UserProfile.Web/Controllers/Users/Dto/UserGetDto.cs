using System.Text.Json.Serialization;

namespace Sonar.UserProfile.Web.Controllers.Users.Dto;

public class UserGetDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
}