using Microsoft.Extensions.Logging;
using ShoppingAppDB.Models;
using ShoppingAppDB.Services;

namespace ShoppingAppBussiness
{
    public class AuthService
    {
        private ILogger<AuthService> _logger;
        private Auth _authService;
        private const string _prefix = "AuthBL ";

        public AuthService(ILogger<AuthService> logger, Auth authService)
        {
            _logger = logger;
            _authService = authService;
        }

        public async Task<TokenResponseDto?> Login(string username, string password)
        {
            _logger.LogInformation($"{_prefix}Login");
            return await _authService.Login(username, password);
        }

        public async Task<bool> Logout(int userId)
        {
            _logger.LogInformation($"{_prefix}Logout");
            return await _authService.Logout(userId);
        }

        public TokenResponseDto? RefreshToken(RefreshTokenRequestDto refreshToken)
        {
            _logger.LogInformation($"{_prefix}RefreshToken");
            return _authService.RefreshToken(refreshToken);
        }
    }
}