using Microsoft.Extensions.DependencyInjection;
using Sonar.UserProfile.Core.Domain.Users.Encoders;
using Sonar.UserProfile.Core.Domain.Users.Services;
using Sonar.UserProfile.Core.Domain.Users.Services.Interfaces;
using Sonar.UserProfile.Data.Users.Encoders;

namespace Sonar.UserProfile.Core;

public static class Bootstrapper
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRelationshipService, RelationshipService>();
        services.AddScoped<IPasswordEncoder, BCryptPasswordEncoder>();

        return services;
    }
}