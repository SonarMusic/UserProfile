using Sonar.UserProfile.Core.Domain.Users.Repositories;

namespace Sonar.UserProfile.Core.Domain.Users.Services;

public class UserService : IUserService
{
    private IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return _userRepository.GetById(id, cancellationToken);
    }

    public Task<IReadOnlyList<User>> GetAll(CancellationToken cancellationToken = default)
    {
        return _userRepository.GetAll(cancellationToken);
    }

    public Task Login(User user, CancellationToken cancellationToken = default)
    {
        // TODO: Проверяем, совпал ли пароль и обновляем токен.
        throw new NotImplementedException();
    }

    public Task Logout(Guid id, CancellationToken cancellationToken = default)
    {
        // TODO: Анулируем токен.
        throw new NotImplementedException();
    }
}