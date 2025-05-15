using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ShoppingAppDB.Data;
using ShoppingAppDB.Entities;
using ShoppingAppDB.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShoppingAppDB.Services
{
    public class Auth
    {
        private ILogger<Auth> _logger;
        private IConfiguration _configuration;
        private Password _passwordService;
        private const string _prefix = "ServAuth ";

        public Auth(ILogger<Auth> logger, IConfiguration configuration, Password passwordService)
        {
            _logger = logger;
            _configuration = configuration;
            _passwordService = passwordService;
        }

        private TokenResponseDto CreateTokenResponse(User user)
        {
            _logger.LogInformation($"{_prefix}Creating token response");
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = GenerateAndSaveRefreshToken(user)
            };
        }

        private string GenerateAndSaveRefreshToken(User user)
        {
            _logger.LogInformation($"{_prefix}Generating and saving refresh token");
            using (var context = new AppDbContext())
            {
                var refreshToken = GenerateRefreshToken();

                var trackedUser = context.Users.Attach(user);
                trackedUser.Entity.RefreshToken = refreshToken;
                trackedUser.Entity.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                _logger.LogInformation($"{_prefix}Refresh token saved successfully");
                context.SaveChanges();
                return refreshToken;
            }
        }

        private string CreateToken(User user)
        {
            _logger.LogInformation($"{_prefix}Creating token");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Auth:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Auth:Issuer"),
                audience: _configuration.GetValue<string>("Auth:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            _logger.LogInformation($"{_prefix}Token created successfully");
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public TokenResponseDto? RefreshToken(RefreshTokenRequestDto request)
        {
            _logger.LogInformation($"{_prefix}Refreshing tokens");
            var user = ValidateRefreshToken(request.UserId, request.RefreshToken);
            if (user is null)
            {
                _logger.LogWarning($"{_prefix}Refresh token validation failed");
                return null;
            }

            _logger.LogInformation($"{_prefix}Refresh token validation successful");
            return CreateTokenResponse(user);
        }

        private User? ValidateRefreshToken(int userId, string refreshToken)
        {
            _logger.LogInformation($"{_prefix}Validating refresh token");
            using (var context = new AppDbContext())
            {
                var user = context.Users.Find(userId);
                if (user is null || user.RefreshToken != refreshToken
                    || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    _logger.LogWarning($"{_prefix}Refresh token validation failed");
                    return null;
                }

                _logger.LogInformation($"{_prefix}Refresh token validation successful");
                return user;
            }
        }

        private string GenerateRefreshToken()
        {
            _logger.LogInformation($"{_prefix}Generating refresh token");
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<TokenResponseDto?> Login(string username, string password)
        {
            _logger.LogInformation($"{_prefix}Login");
            using (var context = new AppDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Name == username);

                if (user != null && _passwordService.VerifyPassword(password, user.PasswordHash))
                {
                    user.LastLogin = DateTime.Now;
                    await context.SaveChangesAsync();

                    _logger.LogInformation($"{_prefix}User logged in successfully with id {user.Id}");

                    return CreateTokenResponse(user);
                }
                _logger.LogWarning($"{_prefix}Login failed");
                return null;
            }
        }

        public async Task<bool> Logout(int userId)
        {
            _logger.LogInformation($"{_prefix}Logout");
            using (var context = new AppDbContext())
            {
                var user = await context.Users.FindAsync(userId);
                if (user is null || user.RefreshToken is null || user.RefreshTokenExpiryTime is null)
                {
                    _logger.LogWarning($"{_prefix}Logout failed");
                    return false;
                }

                // Invalidate refresh token
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;

                _logger.LogInformation($"{_prefix}User logged out successfully with id {user.Id}");

                await context.SaveChangesAsync();
                return true;
            }
        }
    }
}