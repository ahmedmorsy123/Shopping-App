using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShoppingApp.Api.Models;
using static System.Net.WebRequestMethods;

namespace ShoppingApp.Api
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private string _accessToken;

        public string refereshToken;
        public UserDto currentUser;

        public ApiClient(string baseUrl = "https://localhost:7093")
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void SetAuthorizationToken(string token)
        {
            _accessToken = token;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void SetRefereshToken(string token)
        {
            refereshToken = token;
        }
        public void ClearAuthorizationToken()
        {
            _accessToken = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public void ClearRefereshToken()
        {
            refereshToken = null;
        }

        protected async Task<T> GetAsync<T>(string endpoint, string queryString = "")
        {
            string url = string.IsNullOrEmpty(queryString) ? endpoint : $"{endpoint}?{queryString}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            await HandleResponseErrors(response);
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        protected async Task<T> PostAsync<T>(string endpoint, object data = null)
        {
            HttpContent content = data == null
            ? null
                : new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);
            await HandleResponseErrors(response);
            string responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        protected async Task PostAsync(string endpoint, object data = null)
        {
            HttpContent content = data == null
            ? null
                : new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);
            await HandleResponseErrors(response);
        }

        protected async Task<T> PutAsync<T>(string endpoint, object data, string queryString = "")
        {
            string url = string.IsNullOrEmpty(queryString) ? endpoint : $"{endpoint}?{queryString}";
            HttpContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(url, content);
            await HandleResponseErrors(response);
            string responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        protected async Task DeleteAsync(string endpoint, string queryString = "")
        {
            string url = string.IsNullOrEmpty(queryString) ? endpoint : $"{endpoint}?{queryString}";
            HttpResponseMessage response = await _httpClient.DeleteAsync(url);
            await HandleResponseErrors(response);
        }

        private async Task HandleResponseErrors(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                ProblemDetails problemDetails = null;

                try
                {
                    problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(errorContent);
                }
                catch
                {
                    // If parsing fails, problemDetails remains null
                }

                string errorMessage = problemDetails != null
                    ? $"{problemDetails.Title}: {problemDetails.Detail}"
                    : $"Error: {response.StatusCode} - {errorContent}";

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.BadRequest:
                        throw new ApiException(400, errorMessage);
                    case System.Net.HttpStatusCode.Unauthorized:
                        throw new ApiException(401, "Authentication required. Please login.");
                    case System.Net.HttpStatusCode.Forbidden:
                        throw new ApiException(403, "You don't have permission to access this resource.");
                    case System.Net.HttpStatusCode.NotFound:
                        throw new ApiException(404, "The requested resource was not found.");
                    default:
                        throw new ApiException((int)response.StatusCode, $"Server error occurred: {errorMessage}");
                }
            }
        }
    }

    public class ApiException : Exception
    {
        public int StatusCode { get; }

        public ApiException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }

    public class ProblemDetails
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int? Status { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }
    }
}