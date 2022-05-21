using Sonar.UserProfile.Core.Domain.Users.Repositories;
using Sonar.UserProfile.Core.Domain.ValueObjects;

namespace Sonar.UserProfile.Core.Domain.Users.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetById(id, cancellationToken);
        
        // TODO: Добавить валидатор.
        if (user.Token is null)
        {
            throw new Exception("This user is logged out.");
        }
        
        if (user.Token.ExpirationDate < DateTime.UtcNow)
        {
            throw new Exception("Token has expired.");
        }

        return user;
    }

    public async Task Login(User user, CancellationToken cancellationToken = default)
    {
        var savedUser = await _userRepository.GetById(user.Id, cancellationToken);

        if (savedUser.Password != user.Password)
        {
            throw new Exception("Incorrect password.");
        }

        // TODO: Пуcть какой-то провайдер поставляет дни для жизни токена, а не магическое число.
        const int tokenLifeDays = 7;
        savedUser.Token = new Token { ExpirationDate = DateTime.UtcNow.AddDays(tokenLifeDays) };

        await _userRepository.Update(savedUser, cancellationToken);
    }

    public async Task Logout(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetById(id, cancellationToken);
        
        if (user.Token is null)
        {
            throw new Exception("This user is already logged out.");
        }
        
        if (user.Token.ExpirationDate < DateTime.UtcNow)
        {
            throw new Exception($"Token has expired {user.Token.ExpirationDate}.");
        }

        user.Token.ExpirationDate = DateTime.UtcNow;
        await _userRepository.Update(user, cancellationToken);
    }
}