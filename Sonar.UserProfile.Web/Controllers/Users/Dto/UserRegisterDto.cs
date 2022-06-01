using System.ComponentModel.DataAnnotations;

namespace Sonar.UserProfile.Web.Controllers.Users.Dto;

public class UserRegisterDto
{
    [Required(ErrorMessage = "Email is required")]
    [RegularExpression(".+@.+\\..+", ErrorMessage = "Email is incorrect")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    // Считаю, что пороль меньше 6 символов не может считаться паролем априори
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }
}