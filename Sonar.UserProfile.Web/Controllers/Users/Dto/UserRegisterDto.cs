using System.ComponentModel.DataAnnotations;

namespace Sonar.UserProfile.Web.Controllers.Users.Dto;

public class UserRegisterDto
{
    [Required]
    [RegularExpression(".+@.+\\..+")]
    public string Email { get; set; }
    [Required]
    // Считаю, что пороль меньше 6 символов не может считаться паролем априори
    [StringLength(6)]
    public string Password { get; set; }
}