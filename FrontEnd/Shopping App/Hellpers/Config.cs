using Serilog;
using ShoppingApp.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shopping_App.Hellpers
{
    internal class Config
    {
        private static string path = Path.Combine(Environment.CurrentDirectory, "data.xml");
        private static XDocument doc;

        public static void Initialize()
        {
            if (!File.Exists(path))
            {
                try
                {
                    doc = new XDocument(
                        new XDeclaration("1.0", "utf-8", null),
                        new XElement("ShoppingData",
                            // Api section
                            new XElement("Api",
                                new XElement("BaseUri", "https://localhost:7093"),
                                new XElement("Auth",
                                    new XElement("LogIn", "/api/Auth/Login"),
                                    new XElement("RefreshToken", "/api/Auth/refresh-token"),
                                    new XElement("LogOut", "/api/Auth/logout")
                                ),
                                new XElement("Products",
                                    new XElement("GetProducts", "/api/Products/GetProducts")
                                ),
                                new XElement("Orders",
                                    new XElement("GetUserOrders", "/api/Orders/GetUserOrders"),
                                    new XElement("GetOrderById", "/api/Orders/GetOrderById"),
                                    new XElement("MakeOrder", "/api/Orders/MakeOrders"),
                                    new XElement("CancelOrder", "/api/Orders/CancelOrder")
                                ),
                                new XElement("Carts",
                                    new XElement("GetUserCart", "/api/Carts/GetUserCart"),
                                    new XElement("UpdateCart", "/api/Carts/UpdateCart"),
                                    new XElement("AddCart", "/api/Carts/AddCart"),
                                    new XElement("DeleteCart", "/api/Carts/DeleteCart")
                                ),
                                new XElement("Users",
                                    new XElement("GetUser", "/api/Users/getUser"),
                                    new XElement("UpdateUser", "/api/Users/UpdateUser"),
                                    new XElement("DeleteUser", "/api/Users/DeleteUser"),
                                    new XElement("AddUser", "/api/Users/AddUser")
                                )
                            ),
                            // CurrentUser section (empty values for tokens)
                            new XElement("CurrentUser",
                                new XElement("Id", ""),
                                new XElement("Name", ""),
                                new XElement("Email", ""),
                                new XElement("CartId", ""),
                                new XElement("RefreshToken", ""),
                                new XElement("AccessToken", "")
                            ),
                            // LoginInfo section
                            new XElement("LoginInfo",
                                new XElement("RefreshToken", ""),
                                new XElement("Id", "")
                            )
                        )
                    );

                    doc.Save(path);
                    Log.Information("XML file created successfully at: {FilePath}", path);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error creating XML file at: {FilePath}", path);
                    throw;
                }
            }
            else
            {
                doc = XDocument.Load(path);
            }
        }

        public static string GetApiBaseUri()
        {
            return doc.Root.Element("Api").Element("BaseUri").Value;
        }

        public static string GetApiEndpoint(string section, string action)
        {
            return doc.Root.Element("Api").Element(section).Element(action).Value;
        }

        public static int GetCurrentUserId()
        {
            return Convert.ToInt32(doc.Root.Element("CurrentUser").Element("Id").Value);
        }
        public static string GetCurrentUserName()
        {
            return doc.Root.Element("CurrentUser").Element("Name").Value;
        }

        public static string GetCurrentUserEmail()
        {
            return doc.Root.Element("CurrentUser").Element("Email").Value;
        }

        public static int GetCurrentUserCartId()
        {
            return Convert.ToInt32(doc.Root.Element("CurrentUser").Element("CartId").Value);
        }

        public static string GetCurrentUserRefreshToken()
        {
            return doc.Root.Element("CurrentUser").Element("RefreshToken").Value;
        }

        public static string GetCurrentUserAccessToken()
        {
            return doc.Root.Element("CurrentUser").Element("AccessToken").Value;
        }

        public static void SetCurrentUser(UserDto user)
        {
            Console.WriteLine("Current user is setted");
            doc.Root.Element("CurrentUser").Element("Id").Value = user.Id.ToString();
            doc.Root.Element("CurrentUser").Element("Name").Value = user.Name;
            doc.Root.Element("CurrentUser").Element("Email").Value = user.Email;
            doc.Save(path);
        }

        public static void SetCurrentUserCartId(int cartId)
        {
            doc.Root.Element("CurrentUser").Element("CartId").Value = cartId.ToString();
            doc.Save(path);
        }

        public static void SetCurrentUserRefreshToken(string refreshToken)
        {
            doc.Root.Element("CurrentUser").Element("RefreshToken").Value = refreshToken;
            doc.Save(path);
        }

        public static void SetCurrentUserAccessToken(string accessToken)
        {
            doc.Root.Element("CurrentUser").Element("AccessToken").Value = accessToken;
            doc.Save(path);
        }

        public static void ClearRefreshToken()
        {
            doc.Root.Element("CurrentUser").Element("RefreshToken").Value = "";
            doc.Save(path);
        }

        public static void ClearAccessToken()
        {
            doc.Root.Element("CurrentUser").Element("AccessToken").Value = "";
            doc.Save(path);
        }

        public static void ClearCurrentUser()
        {
            doc.Root.Element("CurrentUser").Element("Id").Value = "0";
            doc.Root.Element("CurrentUser").Element("Name").Value = "";
            doc.Root.Element("CurrentUser").Element("Email").Value = "";
            doc.Root.Element("CurrentUser").Element("CartId").Value = "0";
            doc.Root.Element("CurrentUser").Element("RefreshToken").Value = "";
            doc.Root.Element("CurrentUser").Element("AccessToken").Value = "";
            doc.Save(path);
        }

        public static void SetRememberMe(string refreshToken, int id)
        {
            doc.Root.Element("LoginInfo").Element("RefreshToken").Value = refreshToken;
            doc.Root.Element("LoginInfo").Element("Id").Value = id.ToString();
            doc.Save(path);
        }

        public static string GetRememberedRefreshToken()
        {
            return doc.Root.Element("LoginInfo").Element("RefreshToken").Value;
        }

        public static int GetRememberedUserId()
        {
            return Convert.ToInt32(doc.Root.Element("LoginInfo").Element("Id").Value);
        }

        public static void ClearRemeberedData()
        {
            doc.Root.Element("LoginInfo").Element("RefreshToken").Value = "";
            doc.Root.Element("LoginInfo").Element("Id").Value = "";
            doc.Save(path);
        }


    }
}
