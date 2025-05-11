using Microsoft.Extensions.Logging;

namespace ShoppingAppDB.Services
{
    public class Password
    {
        private const int WorkFactor = 10;
        private const string _prefix = "ServPass ";
        private ILogger<Password> _logger;

        public Password(ILogger<Password> logger)
        {
            _logger = logger;
        }

        public string HashPassword(string plainTextPassword)
        {
            _logger.LogInformation($"{_prefix}Hashing password");
            return BCrypt.Net.BCrypt.HashPassword(plainTextPassword, WorkFactor);
        }

        public bool VerifyPassword(string plainTextPassword, string hashedPassword)
        {
            _logger.LogInformation($"{_prefix}Verifying password");
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
        }
    }
}