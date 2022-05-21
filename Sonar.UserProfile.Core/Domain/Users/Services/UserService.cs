namespace Sonar.UserProfile.Core.Domain.Users.Services;

public class UserService : IUserService
{
    public Task<User> GetById(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<User>> GetAll(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Login(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Logout(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}