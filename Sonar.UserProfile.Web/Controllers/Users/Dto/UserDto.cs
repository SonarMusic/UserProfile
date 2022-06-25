using System.ComponentModel.DataAnnotations;

namespace Sonar.UserProfile.Web.Controllers.Users.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is incorrect")]
    public string Email { get; set; }
}