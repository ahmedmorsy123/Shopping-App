using Newtonsoft.Json.Linq;
using Serilog;
using Shopping_App.Forms;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var loginForm = new LoginRegisterForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm());
                }
            }

            //Application.Run(new MainForm());

        }
    }
}
