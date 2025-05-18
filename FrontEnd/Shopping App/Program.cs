using Newtonsoft.Json.Linq;
using Serilog;
using ShoppingApp.Api;
using ShoppingApp.Api.Controllers;
using ShoppingApp.Api.Models;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Shopping_App
{
    internal  class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug() 
                .WriteTo.File("C:/Users/ENG Ahmed/source/repos/ShoppingApp/Logs/Client Logs/Client.txt", 
                rollingInterval: RollingInterval.Day, 
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();


            AuthService authService = new AuthService();

            TokenResponseDto tokenResponseDto = await authService.LoginAsync("Ahmed", "3420");

            if (tokenResponseDto != null)
            {
                Console.WriteLine(tokenResponseDto.RefreshToken);
                Console.WriteLine(tokenResponseDto.AccessToken);
            }

            Console.WriteLine(extractUserIdButton_Click());

            Log.Information("Application starting.");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm()); 
            


        }


        
    }
}
