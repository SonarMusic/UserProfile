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

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        
        // TODO: Добавить валидатор.
        if (user.Token.ExpirationDate < DateTime.UtcNow)
        {
            throw new Exception("Token has expired.");
        }

        return user;
    }

    public async Task<Guid> RegisterAsync(User user, CancellationToken cancellationToken)
    {
        user.Id = Guid.NewGuid();
        
        const int tokenLifeDays = 7;
        user.Token = new Token
        {
            Id = Guid.NewGuid(),
            ExpirationDate = DateTime.UtcNow.AddDays(tokenLifeDays)
        };

        await _userRepository.CreateAsync(user, cancellationToken);

        return user.Id;
    }

    public async Task LoginAsync(User user, CancellationToken cancellationToken = default)
    {
        var savedUser = await _userRepository.GetByIdAsync(user.Id, cancellationToken);

        if (savedUser.Password != user.Password)
        {
            throw new Exception("Incorrect password.");
        }

        // TODO: Пуcть какой-то провайдер поставляет дни для жизни токена, а не магическое число.
        const int tokenLifeDays = 7;
        savedUser.Token = new Token
        {
            Id = Guid.NewGuid(),
            ExpirationDate = DateTime.UtcNow.AddDays(tokenLifeDays)
        };

        await _userRepository.UpdateAsync(savedUser, cancellationToken);
    }

    public async Task LogoutAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (user.Token.ExpirationDate < DateTime.UtcNow)
        {
            throw new Exception($"Token has expired {user.Token.ExpirationDate}.");
        }

        user.Token = new Token
        {
            Id = Guid.NewGuid(),
            ExpirationDate = DateTime.UtcNow
        };
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}