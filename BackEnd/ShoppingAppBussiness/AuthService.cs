using Microsoft.Extensions.Logging;
using ShoppingAppDB;
using ShoppingAppDB.Models;
using ShoppingAppDB.Services;
using static ShoppingAppDB.Enums.Enums;

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

        public async Task<TokenResponseDto?> LoginAsync(LoginRequestDto loginRequest)
        {
            _logger.LogInformation($"{_prefix}Login");
            return await _authService.LoginAsync(loginRequest);
        }

        public async Task<bool> LogoutAsync(int userId)
        {
            _logger.LogInformation($"{_prefix}Logout");
            return await _authService.LogoutAsync(userId);
        }

        public TokenResponseDto? RefreshToken(RefreshTokenRequestDto refreshToken)
        {
            _logger.LogInformation($"{_prefix}RefreshToken");
            return _authService.RefreshToken(refreshToken);
        }

        public async Task<int> GetLoginCountByDurationAsync(TimeDuration duration)
        {
            _logger.LogInformation($"{_prefix}GetLoginCountByDurationAsync called with duration: {duration}");
            return await _authService.GetLoginCountByDurationAsync(duration);
        }

        public async Task<int> GetRegisterationCountByDurationAsync(TimeDuration duration)
        {
            _logger.LogInformation($"{_prefix}GetRegisterationCountByDurationAsync called with duration: {duration}");
            return await _authService.GetRegisterationCountByDurationAsync(duration);
        }

    }
}