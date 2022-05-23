using Sonar.UserProfile.Core.Domain.Tokens;
using Sonar.UserProfile.Core.Domain.Tokens.Repositories;
using Sonar.UserProfile.Core.Domain.Users.Repositories;

namespace Sonar.UserProfile.Core.Domain.Users.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;

    public UserService(IUserRepository userRepository, ITokenRepository tokenRepository)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
    }

    public async Task<User> GetByIdAsync(Guid tokenId, CancellationToken cancellationToken = default)
    {
        var token = await _tokenRepository.GetByIdAsync(tokenId, cancellationToken);

        if (token.ExpirationDate < DateTime.UtcNow)
        {
            throw new Exception($"Token has expired {token.ExpirationDate}");
        }
        
        var user = await _userRepository.GetByIdAsync(token.UserId, cancellationToken);

        return user;
    }

    public async Task<Guid> RegisterAsync(User user, CancellationToken cancellationToken)
    {
        user.Id = Guid.NewGuid();
        
        const int tokenLifeDays = 7;
        var token = new Token
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            ExpirationDate = DateTime.UtcNow.AddDays(tokenLifeDays)
        };

        await _userRepository.CreateAsync(user, cancellationToken);
        await _tokenRepository.CreateAsync(token, cancellationToken);

        return token.Id;
    }

    public async Task<Guid> LoginAsync(User user, CancellationToken cancellationToken = default)
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
            Id = Guid.NewGuid(),
            UserId = user.Id,
            ExpirationDate = DateTime.UtcNow.AddDays(tokenLifeDays)
        };
        // TODO: Каждый раз новый токен создаётся, мб стоит старый рефрешить.

        await _tokenRepository.CreateAsync(token, cancellationToken);

        return token.Id;
    }

    public async Task Logout(Guid tokenId, CancellationToken cancellationToken)
    {
        var token = await _tokenRepository.GetByIdAsync(tokenId, cancellationToken);

        if (token.ExpirationDate < DateTime.UtcNow)
        {
            throw new Exception($"Your token has expired {token.ExpirationDate}");
        }
        
        await _tokenRepository.DeleteAsync(tokenId, cancellationToken);
    }
}