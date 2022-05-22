using System.Text.Json;
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

    public async Task<User> GetByIdAsync(string stringToken, CancellationToken cancellationToken = default)
    {
        var token = JsonSerializer.Deserialize<Token>(stringToken);

        if (token is null)
        {
            throw new Exception("Incorrect token");
        }
        
        if (token.ExpirationDate < DateTime.UtcNow)
        {
            throw new Exception($"Token has expired {token.ExpirationDate}");
        }
        
        var user = await _userRepository.GetByIdAsync(token.User.Id, cancellationToken);

        return user;
    }

    public async Task<string> RegisterAsync(User user, CancellationToken cancellationToken)
    {
        user.Id = Guid.NewGuid();
        
        const int tokenLifeDays = 7;
        var token = new Token
        {
            User = user,
            ExpirationDate = DateTime.UtcNow.AddDays(tokenLifeDays)
        };

        await _userRepository.CreateAsync(user, cancellationToken);

        return JsonSerializer.Serialize(token);
    }

    public async Task<string> LoginAsync(User user, CancellationToken cancellationToken = default)
    {
        var savedUser = await _userRepository.GetByIdAsync(user.Id, cancellationToken);

        if (savedUser.Password != user.Password)
        {
            throw new Exception("Incorrect password.");
        }

        // TODO: Пуcть какой-то провайдер поставляет дни для жизни токена, а не магическое число.
        const int tokenLifeDays = 7;
        var token = new Token
        {
            User = user,
            ExpirationDate = DateTime.UtcNow.AddDays(tokenLifeDays)
        };

        return JsonSerializer.Serialize(token);
    }

    public string Logout(User user, CancellationToken cancellationToken = default)
    {
        var token = new Token
        {
            User = user,
            ExpirationDate = DateTime.UtcNow
        };

        return JsonSerializer.Serialize(token);
    }
}