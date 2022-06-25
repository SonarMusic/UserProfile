using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sonar.UserProfile.Core.Domain.Users.Repositories;
using Sonar.UserProfile.Data.Users.UserRepository;
using Microsoft.Extensions.Configuration;

namespace Sonar.UserProfile.Data;

public static class Bootstrapper
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRelationshipRepository, RelationshipRepository>();

        services.AddDbContext<SonarContext>(options => options
            .UseLazyLoadingProxies()
            .UseSqlite(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                configuration["SQLiteConnectionString"])));
        return services;
    }
}