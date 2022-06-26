using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sonar.UserProfile.Core.Domain.Exceptions;
using Sonar.UserProfile.Core.Domain.SmtpClients.Services;
using Sonar.UserProfile.Core.Domain.Users.Encoders;
using Sonar.UserProfile.Core.Domain.Users.Repositories;
using Sonar.UserProfile.Core.Domain.Users.Services.Interfaces;

namespace Sonar.UserProfile.Core.Domain.Users.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IPasswordEncoder _passwordEncoder;
    private readonly ISmtpClientService _smtpClientService;
    public UserService(IUserRepository userRepository, IConfiguration configuration, IPasswordEncoder passwordEncoder, ISmtpClientService smtpClientService)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _passwordEncoder = passwordEncoder;
        _smtpClientService = smtpClientService;
    }

    public Task<User> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _userRepository.GetByIdAsync(userId, cancellationToken);
    }

    public Task UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        user.Password = _passwordEncoder.Encode(user.Password);
        return _userRepository.UpdateAsync(user, cancellationToken);
    }

    public async Task<string> RegisterAsync(User user, CancellationToken cancellationToken)
    {
        user.Id = Guid.NewGuid();
        user.Password = _passwordEncoder.Encode(user.Password);

        const int tokenLifeDays = 7;
        var secret = _configuration["Secret"];
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        var issuer = _configuration["Issuer"];
        var audience = _configuration["Audience"];
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
            Expires = DateTime.UtcNow.AddDays(tokenLifeDays)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        await _userRepository.CreateAsync(user, cancellationToken);
        var mailMessage = await _smtpClientService.CreateMailMessageAsync(user.Email, "Registration", $"Hello {user.Email}, you have successfully registered in Sonar User Profile.", cancellationToken);
        _smtpClientService.SendMailMessageAsync(mailMessage, user.Email, cancellationToken);
        
        return tokenHandler.WriteToken(token);
    }

    public async Task<string> LoginAsync(User user, CancellationToken cancellationToken = default)
    {
        var dataBaseUser = await _userRepository.GetByEmailAsync(user.Email, cancellationToken);

        if (!_passwordEncoder.Matches(user.Password, dataBaseUser.Password))
        {
            throw new InvalidPasswordException("Incorrect password.");
        }

        const int tokenLifeDays = 7;
        var secret = _configuration["Secret"];
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        var issuer = _configuration["Issuer"];
        var audience = _configuration["Audience"];
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, dataBaseUser.Id.ToString())
            }),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
            Expires = DateTime.UtcNow.AddDays(tokenLifeDays)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
    public async Task RecoverPassword(string email, CancellationToken cancellationToken = default)
    {
        // return await _userRepository.GetByEmailAsync(email, cancellationToken);
    }
}