using Microsoft.EntityFrameworkCore;
using Sonar.UserProfile.Core.Domain.Tokens;
using Sonar.UserProfile.Core.Domain.Tokens.Repositories;

namespace Sonar.UserProfile.Data.Tokens.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly SonarContext _context;

    public TokenRepository(SonarContext context)
    {
        _context = context;
    }

    public async Task<Token> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Tokens
            .FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

        //TODO: добавить класс эксешенов и мидлварки
        if (entity is null)
        {
            throw new Exception($"Token with id = {id} does not exists");
        }

        return new Token
        {
            Id = entity.Id,
            ExpirationDate = entity.ExpirationDate,
            UserId = entity.UserId
        };
    }

    public async Task<IReadOnlyList<Token>> GetAllAsync(CancellationToken cancellationToken)
    {
        var tokens = await _context.Tokens.ToListAsync(cancellationToken);

        return (IReadOnlyList<Token>)tokens.Select(entity => new Token()
        {
            Id = entity.Id,
            ExpirationDate = entity.ExpirationDate,
            UserId = entity.UserId
        });
    }

    public async Task<Guid> CreateAsync(Token token, CancellationToken cancellationToken)
    {
        var entity = new TokenDbModel
        {
            Id = token.Id,
            ExpirationDate = token.ExpirationDate,
            UserId = token.UserId
        };

        await _context.Tokens.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(Token token, CancellationToken cancellationToken)
    {
        var entity =
            await _context.Tokens.FirstOrDefaultAsync(it => it.Id == token.Id, cancellationToken);

        if (entity is null)
        {
            throw new Exception($"Token with id = {token.Id} does not exists");
        }

        entity.ExpirationDate = token.ExpirationDate;
        entity.UserId = token.UserId;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Tokens.FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

        if (entity is null)
        {
            throw new Exception($"Token with id = {id} does not exists");
        }

        _context.Tokens.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}