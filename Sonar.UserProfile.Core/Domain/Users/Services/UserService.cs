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
using Sonar.UserProfile.Core.Domain.Users.ValueObjects;

namespace Sonar.UserProfile.Core.Domain.Users.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IPasswordEncoder _passwordEncoder;
    private readonly ISmtpClientService _smtpClientService;

    public UserService(IUserRepository userRepository, IConfiguration configuration, IPasswordEncoder passwordEncoder,
        ISmtpClientService smtpClientService)
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
        user.AccountType = AccountType.Open;

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
        var mailMessage = await _smtpClientService.CreateMailMessageAsync(user.Email,
            "Registration on Sonar Music Streaming",
            $"Hello {user.Email}, you have successfully registered in Sonar Music Streaming. To confirm your account follow this link {_configuration["uri"] + "/user/confirm-mail/" + user.Id.ToString()}",
            cancellationToken);
        await _smtpClientService.SendMailMessageAsync(mailMessage, cancellationToken);

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

    public async Task RecoverPasswordAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        var newPassword = Guid.NewGuid().ToString()[..8];
        user.Password = _passwordEncoder.Encode(newPassword);
        var mailMessage = await _smtpClientService.CreateMailMessageAsync(user.Email, "Recover Password",
            $"Hello {user.Email}, your new password is {newPassword}.", cancellationToken);
        await _smtpClientService.SendMailMessageAsync(mailMessage, cancellationToken);
        await _userRepository.UpdateAsync(user, cancellationToken);
    }

    public async Task ConfirmMailAsync(string confirmToken, CancellationToken cancellationToken = default)
    {
        var userId = Guid.Parse(confirmToken);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException($"User with id {userId} not found.");
        }

        user.ConfirmStatus = ConfirmStatus.Confirmed;
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}