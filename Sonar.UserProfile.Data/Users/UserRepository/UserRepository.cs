using Microsoft.EntityFrameworkCore;
using Sonar.UserProfile.Core.Domain.Exceptions;
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

        if (entity is null)
        {
            throw new NotFoundException($"User with id = {id} does not exists");
        }

        return new User
        {
            Id = entity.Id,
            Email = entity.Email,
            Password = entity.Password
        };
    }

    public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        email = email.ToLower();
        var entity = await _context.Users
            .FirstOrDefaultAsync(it => string.Equals(it.Email, email), cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException($"User with email = {email} does not exists");
        }

        return new User
        {
            Id = entity.Id,
            Email = entity.Email,
            Password = entity.Password
        };
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await _context.Users.ToListAsync(cancellationToken);

        return (IReadOnlyList<User>)users.Select(entity => new User
        {
            Id = entity.Id,
            Email = entity.Email,
            Password = entity.Password
        });
    }

    public async Task<Guid> CreateAsync(User user, CancellationToken cancellationToken)
    {
        user.Email = user.Email.ToLower();
        var sameEmailUser = _context.Users.FirstOrDefault(u => string.Equals(u.Email, user.Email));

        if (sameEmailUser != null)
        {
            throw new DataOccupiedException($"Email {user.Email} is already occupied");
        }

        var entity = new UserDbModel
        {
            Id = user.Id,
            Email = user.Email,
            Password = user.Password
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
            throw new NotFoundException($"User with id = {user.Id} does not exists");
        }

        var sameEmailEntity = 
            await _context.Users.FirstOrDefaultAsync(it => it.Email == user.Email, cancellationToken);

        if (sameEmailEntity != null)
        {
            throw new DataOccupiedException($"User with mail = {user.Email} already exists");
        }

        entity.Email = user.Email;
        entity.Password = user.Password;

        _context.Users.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException($"User with id = {id} does not exists");
        }

        _context.Users.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}