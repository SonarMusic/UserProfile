namespace Sonar.UserProfile.Core.Domain.Users.Services;

public interface IUserService
{
    Task<User> GetById(Guid id, CancellationToken cancellationToken);
    Task Login(User user, CancellationToken cancellationToken);
    Task Logout(Guid id, CancellationToken cancellationToken);
}