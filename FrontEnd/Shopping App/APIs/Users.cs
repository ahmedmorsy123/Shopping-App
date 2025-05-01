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
            User user = null;
            try
            {

                var response = await httpClient.GetAsync($"http://localhost:5002/api/Users/getUser?id={id}");
                var responseBody = await response.Content.ReadAsStringAsync();

                if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
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
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error deserializing JSON: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return user;
        }
        public static async Task<User> UpdateUser(User user, string oldPassword)
        {
            // i need to login first
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

                string jsonPayload = JsonSerializer.Serialize(updateData);

                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync($"http://localhost:5002/api/Users/UpdateUser?oldPassword={oldPassword}", content);

                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    updatedUser = JsonSerializer.Deserialize<User>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Console.WriteLine(responseBody);
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error deserializing JSON: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return updatedUser;
        }

        public static async Task DeleteUser(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"http://localhost:5002/api/Users/DeleteUser?id={id}");
                var responseBody = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(responseBody);
                    return;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static async Task<User> AddUser(User user)
        {
            User addedUser = null;
            try
            {
                string jsonPayload = JsonSerializer.Serialize(user);

                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("http://localhost:5002/api/Users/AddUser", content);

                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    addedUser = JsonSerializer.Deserialize<User>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Console.WriteLine(responseBody);
                }
            }
            catch (HttpRequestException ex) {
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error deserializing JSON: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return addedUser;

        }

        public static async Task<User> CurrentUser()
        {
            User CurrentUser = null;
            try
            {
                var response = await httpClient.GetAsync("http://localhost:5002/api/Users/CurrentUser\r\n");
                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    CurrentUser = JsonSerializer.Deserialize<User>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Console.WriteLine(responseBody);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return CurrentUser;
        }

        public static async Task<User> Login(string username, string password)
        {
            User User = null;
            try
            {
                string JsonPayload = JsonSerializer.Serialize(new { userName = username, password = password });
                var content = new StringContent(JsonPayload, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"http://localhost:5002/api/Users/Login?userName={username}&password={password}", null);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    User = JsonSerializer.Deserialize<User>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Console.WriteLine(responseBody);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error deserializing JSON in Login: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return User;
        }

        public static async Task Logout()
        {
            try
            {
                var response = await httpClient.PostAsync("http://localhost:5002/api/Users/Logout", null);
                var responseBody = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(responseBody);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
