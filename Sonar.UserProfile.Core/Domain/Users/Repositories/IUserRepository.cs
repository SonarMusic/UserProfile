namespace Sonar.UserProfile.Core.Domain.Users.Repositories;

public interface IUserRepository
{
    Task<User> GetById(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetAll(CancellationToken cancellationToken);
    Task<Guid> Create(User user, CancellationToken cancellationToken);
    Task Update(User user, CancellationToken cancellationToken);
    Task Delete(Guid id, CancellationToken cancellationToken);
}