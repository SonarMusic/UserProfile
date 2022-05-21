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
        var entity = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(it => it.Id == id, cancellationToken: cancellationToken);

        if (entity is null)
        {
            return null;
        }

        return new User
        {
            Id = entity.Id,
            Password = entity.Password
        };
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await _context.Users.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);

        return (IReadOnlyList<User>)users.Select(entity => new User()
        {
            Id = entity.Id,
            Password = entity.Password
        });
    }

    public async Task<Guid> CreateAsync(User user, CancellationToken cancellationToken)
    {
        var entity = new UserDbModel
        {
            Id = Guid.NewGuid(),
            Password = user.Password,
        };

        await _context.Users.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(it => it.Id == user.Id, cancellationToken: cancellationToken);

        if (entity != null) 
            entity.Password = user.Password;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(it => it.Id == id);

        if (entity is not null)
        {
            _context.Users.Remove(entity);
        }
    }
}