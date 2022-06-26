using System;
using System.Net.Http;
using System.Threading;
using Moq;
using Sonar.UserProfile.ApiClient;
using Sonar.UserProfile.ApiClient.Dto;
using Xunit;


namespace Sonar.UserProfile.Core.Tests;

public class ApiClientTests
{
    
    public ApiClientTests()
    {
        
    }
    
    [Fact]
    public void GetUser_Success_ShouldReturnUser()
    {
        
    }
    
    [Fact]
    public void GetUser_UserWithThisTokenDoesNotExists_ShouldThrowException()
    {
        
    }
}