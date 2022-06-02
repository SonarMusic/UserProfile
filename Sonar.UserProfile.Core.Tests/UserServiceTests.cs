using System;
using System.ComponentModel.DataAnnotations;
using Moq;
using Sonar.UserProfile.Core.Domain.Exceptions;
using Sonar.UserProfile.Core.Domain.Tokens;
using Sonar.UserProfile.Core.Domain.Tokens.Repositories;
using Sonar.UserProfile.Core.Domain.Users;
using Sonar.UserProfile.Core.Domain.Users.Repositories;
using Sonar.UserProfile.Core.Domain.Users.Services;
using Xunit;
using Xunit.Abstractions;

namespace Sonar.UserProfile.Core.Tests;
// TODO: нужно перенести создание токена на уровень Data в репозиторий, мне кажется это не обязанность Core. 
public class UserServiceTests
{
    private readonly IUserService _userService;
    private readonly Mock<IUserRepository> _fakeUserRepository;
    private readonly Mock<ITokenRepository> _fakeTokenRepository;

    public UserServiceTests()
    {
        _fakeUserRepository = new Mock<IUserRepository>();
        _fakeTokenRepository = new Mock<ITokenRepository>();
        _userService = new UserService(_fakeUserRepository.Object, _fakeTokenRepository.Object);
    }

    [Fact]
    public void RegisterUser_WithNullValue_ShouldThrowException()
    {
        User? user = null;
        
        var exception =
            Assert.ThrowsAsync<NullReferenceException>(() => _userService.RegisterAsync(user, default));
        
        Assert.Contains(exception.Result.Message, "Object reference not set to an instance of an object.");
    }

    [Fact]
    public void RegisterUser_Success_ShouldRegisterUser()
    {
        var user = new User { Email = "email@example.ru", Password = "123456" };
        
        _userService.RegisterAsync(user, default);
        
        _fakeUserRepository.Verify(work => work.CreateAsync(user, default), Times.Once);
    }

    [Fact]
    public void GetUserById_Success_ShouldReturnUser()
    {
        var user = new User { Id = Guid.NewGuid(), Email = "email@example.ru", Password = "123456" };
        const int tokenLifeDays = 7;
        var token = new Token
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            ExpirationDate = DateTime.UtcNow.AddDays(tokenLifeDays)
        };
        
        _fakeTokenRepository.Setup(repo => repo.GetByIdAsync(token.Id, default).Result).Returns(token);
        _fakeUserRepository.Setup(repo => repo.GetByIdAsync(user.Id, default).Result).Returns(user);
        var returnedUser = _userService.GetByIdAsync(token.Id, default).Result;
        
        Assert.Equal(returnedUser.Password, user.Password);
        Assert.Equal(returnedUser.Email, user.Email);
        Assert.NotEqual(Guid.Empty, user.Id);
    }
    
    [Fact]
    public void GetUserById_WithNotExistingUser_ShouldThrowException()
    {
        Assert.ThrowsAsync<TokenNotFoundException>(() => _userService.GetByIdAsync(Guid.Empty, default));
    }

    [Fact]
    public void UserLogin_Success_ShouldLogin()
    {
        var user = new User { Id = Guid.NewGuid(), Email = "email@example.ru", Password = "123456" };
        const int tokenLifeDays = 7;
        var token = new Token
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            ExpirationDate = DateTime.UtcNow.AddDays(tokenLifeDays)
        };
        
        _fakeUserRepository.Setup(repo => repo.GetByIdAsync(user.Id, default).Result).Returns(user);
        _fakeTokenRepository.Setup(repo => repo.GetByIdAsync(token.Id, default).Result).Returns(token);
        _fakeUserRepository.Setup(repo => repo.GetByEmailAsync(user.Email, default).Result).Returns(user);
        var result = _userService.RegisterAsync(user, default).Result;
        // TODO: вынести IPasswordEncoder в bootstrapper и протестировать 
        // var guid = _userService.LoginAsync(user, default).Result;

        // _fakeUserRepository.Verify(work => work.GetByEmailAsync(user.Email, default), Times.Once);
        // _fakeTokenRepository.Verify(work => work.CreateAsync(token, default), Times.Once);
    }
    
    [Fact]
    public void UserLogout_Success_ShouldLogout()
    {
        //TODO: нужно реализовать 
    }
}