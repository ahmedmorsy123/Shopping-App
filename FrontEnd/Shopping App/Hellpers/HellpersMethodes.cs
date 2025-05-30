using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shopping_App.ViewData
{
    internal class HellpersMethodes
    {
        public static void ClearForm(Form form)
        {
            form.Controls.OfType<Control>().ToList().ForEach(c =>
            {
                if (c.GetType() != typeof(MenuStrip))
                {
                    form.Controls.Remove(c);
                }
            });
        }

        // ckech if the role of the user is admin in the access token
        public static bool IsUserAdmin(string jwt)
        {
            Log.Information("Checking if user is admin");
            try
            {
                string[] parts = jwt.Split('.');
                if (parts.Length != 3)
                {
                    return false;
                }
                string payloadBase64Url = parts[1];
                string payloadJson = Base64UrlDecode(payloadBase64Url);
                JObject payload = JObject.Parse(payloadJson);
                JArray roles = (JArray)payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
                return roles != null && roles.Any(role => role.ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error checking if user is admin");
                return false;
            }
        }

        public static int? GetUserIdFromJwt(string jwt)
        {
            Log.Information("Extracting user ID from JWT");
            try
            {
                string[] parts = jwt.Split('.');
                if (parts.Length != 3)
                {
                    return null;
                }

                string payloadBase64Url = parts[1];
                string payloadJson = Base64UrlDecode(payloadBase64Url);

                JObject payload = JObject.Parse(payloadJson);

                string userId = (string)payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];

                return int.Parse(userId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private static string Base64UrlDecode(string base64Url)
        {
            string base64 = base64Url.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }
            byte[] bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
