using FluentValidation;

namespace Sonar.UserProfile.Core.Domain.Users.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be empty");
    }
}