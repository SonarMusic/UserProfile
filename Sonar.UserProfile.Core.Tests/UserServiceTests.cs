using System;
using Microsoft.Extensions.Configuration;
using Moq;
using Sonar.UserProfile.Core.Domain.Exceptions;
using Sonar.UserProfile.Core.Domain.SmtpClients.Services;
using Sonar.UserProfile.Core.Domain.Users;
using Sonar.UserProfile.Core.Domain.Users.Encoders;
using Sonar.UserProfile.Core.Domain.Users.Repositories;
using Sonar.UserProfile.Core.Domain.Users.Services;
using Sonar.UserProfile.Core.Domain.Users.Services.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace Sonar.UserProfile.Core.Tests;
public class UserServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IUserService _userService;
    private readonly Mock<IUserRepository> _fakeUserRepository;
    private readonly Mock<IConfigurationSection> _fakeConfiguration;
    private readonly Mock<IPasswordEncoder> _fakePasswordEncoder;
    private readonly Mock<ISmtpClientService> _fakeSmtpClientService;
    public UserServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
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
    }
    
    [Fact]
    public void GetUserById_WithNotExistingUser_ShouldThrowException()
    {
        Assert.ThrowsAsync<TokenNotFoundException>(() => _userService.GetByIdAsync(Guid.Empty, default));
    }
}