using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sonar.UserProfile.Core.Domain.Users.Repositories;
using Sonar.UserProfile.Data.Users.UserRepository;
using Microsoft.Extensions.Configuration;
using Sonar.UserProfile.Core.Domain.Tokens.Repositories;
using Sonar.UserProfile.Data.Tokens.Repositories;

namespace Sonar.UserProfile.Data;

public static class Bootstrapper
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();

        services.AddDbContext<SonarContext>(options => options
            .UseLazyLoadingProxies()
            .UseSqlite(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                configuration["SQLiteConnectionString"])));
        return services;
    }
}