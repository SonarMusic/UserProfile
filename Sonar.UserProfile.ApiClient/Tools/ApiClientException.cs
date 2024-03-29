﻿namespace Sonar.UserProfile.ApiClient.Tools;

public class ApiClientException : Exception
{
    public ApiClientException()
    {
    }
    
    public ApiClientException(string message) 
        : base(message)
    {
    }

    public ApiClientException(string message, Exception e)
        : base(message, e)
    {
    }
}