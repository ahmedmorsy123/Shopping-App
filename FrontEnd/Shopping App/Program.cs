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
            using (var loginForm = new LoginRegisterForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm());
                }
            }

            //Application.Run(new OrderItemsListForm());





            //// read data from XML file
            //StringBuilder sb = new StringBuilder();
            //foreach ( var item in doc.Root.Elements("Item"))
            //{
            //    sb.AppendLine($"Id: {item.Element("Id").Value}, Name: {item.Element("Name").Value}, Quantity: {item.Element("Quantity").Value}, Price: {item.Element("Price").Value}");
            //}
            //MessageBox.Show(sb.ToString(), "Items in XML", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
