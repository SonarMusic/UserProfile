namespace Sonar.UserProfile.Core.Domain.Tokens.Repositories;

public interface ITokenRepository
{
    Task<Token> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Token>> GetAllAsync(CancellationToken cancellationToken);
    Task<Guid> CreateAsync(Token token, CancellationToken cancellationToken);
    Task UpdateAsync(Token token, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}