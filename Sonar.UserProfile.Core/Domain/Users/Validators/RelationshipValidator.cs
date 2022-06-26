using FluentValidation;
using Sonar.UserProfile.Core.Domain.Users.ValueObjects;

namespace Sonar.UserProfile.Core.Domain.Users.Validators;

public class RelationshipValidator : AbstractValidator<RelationshipStatus>
{
    public RelationshipValidator()
    {
    }
}