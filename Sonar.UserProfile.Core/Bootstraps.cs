using Microsoft.Extensions.DependencyInjection;
using Sonar.UserProfile.Core.Domain.Users.Services;

namespace Sonar.UserProfile.Core;

public static class Bootstraps
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}