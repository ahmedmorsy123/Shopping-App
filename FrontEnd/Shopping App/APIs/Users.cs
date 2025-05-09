using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Shopping_App.APIs.Products;

namespace Shopping_App.APIs
{
    public class Users
    {
        public class User 
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; } = null;
        }

        static readonly HttpClient httpClient = new HttpClient();


        public static async Task<User> GetUserById(int id)
        {
            Log.Information("GetUserById");
            User user = null;
            try
            {
                Log.Information("Making Get Request");
                var response = await httpClient.GetAsync($"http://localhost:5002/api/Users/getUser?id={id}");
                var responseBody = await response.Content.ReadAsStringAsync();

                if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Log.Information("Success status code");
                    Console.WriteLine(responseBody);
                    return null;
                }

                user = JsonSerializer.Deserialize<User>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex, "HTTP Request Error");
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (JsonException ex)
            {
                Log.Error(ex, "Error deserializing JSON");
                MessageBox.Show($"Error deserializing JSON: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return user;
        }
        public static async Task<User> UpdateUser(User user, string oldPassword)
        {
            Log.Information("UpdateUser");
            User updatedUser = null;
            try
            {
                var updateData = new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Password,
                };

                Log.Information("Serializing data");
                string jsonPayload = JsonSerializer.Serialize(updateData);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                Log.Information("Making Put Request");
                var response = await httpClient.PutAsync($"http://localhost:5002/api/Users/UpdateUser?oldPassword={oldPassword}", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Log.Information("Success status code");
                    updatedUser = JsonSerializer.Deserialize<User>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Log.Information("Failed status code");
                    Console.WriteLine(responseBody);
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex, "HTTP Request Error");
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (JsonException ex)
            {
                Log.Error(ex, "Error deserializing JSON");
                MessageBox.Show($"Error deserializing JSON: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return updatedUser;
        }

        public static async Task DeleteUser(int id)
        {
            Log.Information("Delete User");
            try
            {
                Log.Information("Making Delete Request");
                var response = await httpClient.DeleteAsync($"http://localhost:5002/api/Users/DeleteUser?id={id}");
                var responseBody = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    Log.Information("Failed status code");
                    Console.WriteLine(responseBody);
                    return;
                }
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex, "Http request exception");
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static async Task<User> AddUser(User user)
        {
            Log.Information("Add User");
            User addedUser = null;
            try
            {
                Log.Information("Serializing the data");
                string jsonPayload = JsonSerializer.Serialize(user);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                Log.Information("Making post request");
                var response = await httpClient.PostAsync("http://localhost:5002/api/Users/AddUser", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Log.Information("Success status code");
                    addedUser = JsonSerializer.Deserialize<User>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Log.Information("Failed status code");
                    Console.WriteLine(responseBody);
                }
            }
            catch (HttpRequestException ex) 
            {
                Log.Error(ex, "Http request exception");
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException ex)
            {
                Log.Error(ex, "Error deserializing JSON");
                MessageBox.Show($"Error deserializing JSON: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return addedUser;

        }

        public static async Task<User> CurrentUser()
        {
            Log.Information("Current User");
            User CurrentUser = null;
            try
            {
                Log.Information("Making Get Request");
                var response = await httpClient.GetAsync("http://localhost:5002/api/Users/CurrentUser");
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Log.Information("Success status code");
                    CurrentUser = JsonSerializer.Deserialize<User>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Log.Information("Failed status code");
                    Console.WriteLine(responseBody);
                }
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex, "Http request exception");
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return CurrentUser;
        }

        public static async Task<User> Login(string username, string password)
        {
            Log.Information("Login");
            User User = null;
            try
            {
                Log.Information("Making post request");
                var response = await httpClient.PostAsync($"http://localhost:5002/api/Users/Login?userName={username}&password={password}", null);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Log.Information("Success status code");
                    User = JsonSerializer.Deserialize<User>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Log.Information("Failed status code");
                    Console.WriteLine(responseBody);
                }
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex, "Http request exception");
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException ex)
            {
                Log.Error(ex, "Error deserializing JSON");
                MessageBox.Show($"Error deserializing JSON in Login: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return User;
        }

        public static async Task Logout()
        {
            Log.Information("Logout");
            try
            {
                Log.Information("Making post request");
                var response = await httpClient.PostAsync("http://localhost:5002/api/Users/Logout", null);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Log.Information("Failed status code");
                    Console.WriteLine(responseBody);
                }
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex, "Http request exception");
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
