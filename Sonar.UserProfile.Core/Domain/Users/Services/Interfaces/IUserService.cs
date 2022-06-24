﻿namespace Sonar.UserProfile.Core.Domain.Users.Services.Interfaces;

public interface IUserService
{
    Task<User> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task UpdateUserAsync(User userId, CancellationToken cancellationToken);
    Task<string> RegisterAsync(User user, CancellationToken cancellationToken);
    Task<string> LoginAsync(User user, CancellationToken cancellationToken);
}