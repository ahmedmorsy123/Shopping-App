using ShoppingApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_App
{
    internal static class AppState
    {
        public static UserDto CurrentLoggedInUser { get; set; } = new UserDto();
        public static string RefreshToken { get; set; }
        public static string AccessToken { get; set; }
    }
}
