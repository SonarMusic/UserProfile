using Microsoft.EntityFrameworkCore;
using Sonar.UserProfile.Core.Domain.Users;
using Sonar.UserProfile.Core.Domain.Users.Repositories;

namespace Sonar.UserProfile.Data.Users.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly SonarContext _context;

    public UserRepository(SonarContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

        //TODO: добавить класс эксешенов и мидлварки
        if (entity is null)
        {
            throw new Exception($"User with id = {id} does not exists");
        }

        return new User
        {
            Id = entity.Id,
            Password = entity.Password,
            Token = entity.Token
        };
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await _context.Users.ToListAsync(cancellationToken);

        return (IReadOnlyList<User>)users.Select(entity => new User()
        {
            Id = entity.Id,
            Password = entity.Password,
            Token = entity.Token
        });
    }

    public async Task<Guid> CreateAsync(User user, CancellationToken cancellationToken)
    {
        var entity = new UserDbModel
        {
            Id = Guid.NewGuid(),
            Password = user.Password,
            Token = user.Token
        };

        await _context.Users.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        var entity =
            await _context.Users.FirstOrDefaultAsync(it => it.Id == user.Id, cancellationToken);

        if (entity is null)
        {
            throw new Exception($"User with id = {user.Id} does not exists");
        }

        entity.Password = user.Password;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

        if (entity is null)
        {
            throw new Exception($"User with id = {id} does not exists");
        }

        _context.Users.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}