﻿namespace Sonar.UserProfile.Core.Domain.Users.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken);
    Task<Guid> CreateAsync(User user, CancellationToken cancellationToken);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
    Task AddFriendAsync(Guid userId, Guid friendId, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetFriendsByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> IsFriendsAsync(Guid userId, Guid friendId, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}