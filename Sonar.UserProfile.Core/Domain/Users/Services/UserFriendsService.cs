using Sonar.UserProfile.Core.Domain.Exceptions;
using Sonar.UserProfile.Core.Domain.Users.Repositories;

namespace Sonar.UserProfile.Core.Domain.Users.Services;

public class UserFriendsService : IUserFriendsService
{
    private readonly IUserRepository _userRepository;

    public UserFriendsService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task AddFriend(Guid userId, string friendEmail, CancellationToken cancellationToken)
    {
        var dataBaseUser = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (dataBaseUser.Email == friendEmail)
        {
            throw new InvalidRequestException("Users must be different.");
        }
        var dataBaseFriend = await _userRepository.GetByEmailAsync(friendEmail, cancellationToken);

        if (await _userRepository.IsFriendsAsync(userId, dataBaseFriend.Id, cancellationToken))
        {
            throw new DataOccupiedException("These users are already friends.");
        }

        await _userRepository.AddFriendAsync(userId, dataBaseFriend.Id, cancellationToken);
    }

    public Task<IReadOnlyList<User>> GetFriendsById(Guid userId, CancellationToken cancellationToken)
    {
        return _userRepository.GetFriendsByIdAsync(userId, cancellationToken);
    }
}