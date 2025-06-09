using Newtonsoft.Json.Linq;
using Serilog;
using Shopping_App.Forms;
using Shopping_App.Hellpers;
using ShoppingApp.Api;
using ShoppingApp.Api.Controllers;
using ShoppingApp.Api.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;



namespace Shopping_App
{
    internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
                .WriteTo.File("C:/Users/ENG Ahmed/source/repos/ShoppingApp/Logs/Client Logs/Client.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Information("Application starting.");


            Config.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new AdminForm());


            //if (!string.IsNullOrEmpty(Config.GetRememberedRefreshToken()))
            //{
            //    bool result = UserIsRemembered().GetAwaiter().GetResult();
            //    bool isAdmin = Config.IsUserAdmin();

            //    if (result)
            //    {
            //        if (isAdmin)
            //        {
            //            Log.Information("User is remembered as admin, proceeding to admin form.");
            //            Application.Run(new AdminForm());
            //        }
            //        else
            //        {
            //            Log.Information("User is remembered, proceeding to main form.");
            //            Application.Run(new MainForm());
            //        }
            //    }
            //    else
            //    {
            //        Log.Error("Token refresh failed, prompting user to login.");
            //        LogIn();
            //    }
            //}
            //else
            //{
            //    LogIn();
            //}
        }

        private static void LogIn()
        {
            using (var loginForm = new LoginRegisterForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    if (loginForm.IsAdmin)
                    {
                        Log.Information("Admin logged in, proceeding to admin form.");
                        Application.Run(new AdminForm());
                    }
                    else
                    {
                        Log.Information("User logged in, proceeding to main form.");
                        Application.Run(new MainForm());
                    }
                }
            }
        }
        private static async Task<bool> UserIsRemembered()
        {
            TokenResponseDto tokenResponse;
            try
            {
                tokenResponse = await ApiManger.Instance.AuthService.RefreshTokenAsync();
                return true;
            }
            catch (ApiException ex)
            {
                Log.Error($"Token refresh failed: {ex.Message}, Forward user to login");
                return false;
            }
        }
    }
}
