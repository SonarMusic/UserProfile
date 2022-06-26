using System.ComponentModel.DataAnnotations;
using Sonar.UserProfile.Core.Domain.Users.ValueObjects;

namespace Sonar.UserProfile.Web.Controllers.Users.Dto;

public class UserUpdateDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is incorrect")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }

    public AccountType AccountType { get; set; }
}