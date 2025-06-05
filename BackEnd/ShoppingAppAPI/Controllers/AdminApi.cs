using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminApi : ControllerBase
    {
        private const string _prefix = "AuthAPI ";

        private readonly AdminService _authService;
        private readonly ILogger<AdminApi> _logger;

        public AdminApi(ILogger<AdminApi> logger, AdminService adminService)
        {
            _logger = logger;
            _authService = adminService;
        }
    }
}