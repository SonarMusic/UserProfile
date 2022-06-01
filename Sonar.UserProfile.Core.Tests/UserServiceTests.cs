using System;
using System.ComponentModel.DataAnnotations;
using Moq;
using Sonar.UserProfile.Core.Domain.Tokens.Repositories;
using Sonar.UserProfile.Core.Domain.Users.Repositories;
using Sonar.UserProfile.Core.Domain.Users.Services;
using Xunit;

namespace Sonar.UserProfile.Core.Tests;

public class UserServiceTests
{
    private readonly IUserService _userService;
    private readonly Mock<IUserRepository> _fakeUserRepository;
    private readonly Mock<ITokenRepository> _tokenRepository;

    public UserServiceTests()
    {
        _fakeUserRepository = new Mock<IUserRepository>();
        _tokenRepository = new Mock<ITokenRepository>();
        _userService = new UserService(_fakeUserRepository.Object, _tokenRepository.Object);
    }
}
