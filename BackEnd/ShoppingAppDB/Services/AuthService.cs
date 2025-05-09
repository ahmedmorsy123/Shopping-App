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
    public class AuthService
    {
        private ILogger<AuthService> _logger;
        private IConfiguration _configuration;
        private const string _prefix = "ServAuth ";

        public AuthService(ILogger<AuthService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public TokenResponseDto CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = GenerateAndSaveRefreshToken(user)
            };
        }

        private string GenerateAndSaveRefreshToken(User user)
        {
            using (var context = new AppDbContext())
            {
                var refreshToken = GenerateRefreshToken();

                // Attach the user to the context to ensure it is tracked
                var trackedUser = context.Users.Attach(user);
                trackedUser.Entity.RefreshToken = refreshToken;
                trackedUser.Entity.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                context.SaveChanges();
                return refreshToken;
            }
        }

        private string CreateToken(User user)
        {
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

            Console.WriteLine($"Current UTC time: {DateTime.UtcNow}");
            var expiryTime = DateTime.UtcNow.AddDays(1);
            Console.WriteLine($"Token created with expiry: {expiryTime} (Unix timestamp: {new DateTimeOffset(expiryTime).ToUnixTimeSeconds()})");

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public TokenResponseDto? RefreshTokens(RefreshTokenRequestDto request)
        {
            var user = ValidateRefreshToken(request.UserId, request.RefreshToken);
            if (user is null)
                return null;

            return CreateTokenResponse(user);
        }

        private User? ValidateRefreshToken(Guid userId, string refreshToken)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.Find(userId);
                if (user is null || user.RefreshToken != refreshToken
                    || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    return null;
                }

                return user;
            }
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public bool Logout(int userId)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.Find(userId);
                if (user is null)
                    return false;

                // Invalidate refresh token
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;

                context.SaveChanges();
                return true;
            }
        }
    }
}