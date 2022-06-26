using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Moq;
using Sonar.UserProfile.Core.Domain.Exceptions;
using Sonar.UserProfile.Core.Domain.SmtpClients.Services;
using Sonar.UserProfile.Core.Domain.Users;
using Sonar.UserProfile.Core.Domain.Users.Encoders;
using Sonar.UserProfile.Core.Domain.Users.Repositories;
using Sonar.UserProfile.Core.Domain.Users.Services;
using Sonar.UserProfile.Core.Domain.Users.Services.Interfaces;
using Sonar.UserProfile.Data.Users.Encoders;
using Xunit;
using Xunit.Abstractions;
using IConfiguration = Castle.Core.Configuration.IConfiguration;

namespace Sonar.UserProfile.Core.Tests;
public class UserServiceTests
{
    private readonly IUserService _userService;
    private readonly Mock<IUserRepository> _fakeUserRepository;
    private readonly Mock<IConfigurationSection> _fakeConfiguration;
    private readonly Mock<IPasswordEncoder> _fakePasswordEncoder;
    private readonly Mock<ISmtpClientService> _fakeSmtpClientService;
    public UserServiceTests()
    {
        _fakeUserRepository = new Mock<IUserRepository>();
        _fakeConfiguration = new Mock<IConfigurationSection>();
        _fakePasswordEncoder = new Mock<IPasswordEncoder>();
        _fakeSmtpClientService = new Mock<ISmtpClientService>();
 
        _userService = new UserService(_fakeUserRepository.Object, _fakeConfiguration.Object, _fakePasswordEncoder.Object, _fakeSmtpClientService.Object);
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
        
        _fakeUserRepository.Verify(work => work.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void GetUserById_Success_ShouldReturnUser()
    {
        var user = new User { Id = Guid.Empty, Email = "email@example.ru", Password = "123456" };

        _fakeUserRepository.Setup(repo => repo.GetByIdAsync(user.Id, default).Result).Returns(user);
        var returnedUser = _userService.GetByIdAsync(user.Id, default).Result;
        
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

        _fakeUserRepository.Setup(repo => repo.GetByIdAsync(user.Id, default).Result).Returns(user);

        _fakeUserRepository.Setup(repo => repo.GetByEmailAsync(user.Email, default).Result).Returns(user);
        var result = _userService.RegisterAsync(user, default).Result;
        // TODO: вынести IPasswordEncoder в bootstrapper и протестировать 
        // var guid = _userService.LoginAsync(user, default).Result;

        // _fakeUserRepository.Verify(work => work.GetByEmailAsync(user.Email, default), Times.Once);
        // _fakeTokenRepository.Verify(work => work.CreateAsync(token, default), Times.Once);
    }
}