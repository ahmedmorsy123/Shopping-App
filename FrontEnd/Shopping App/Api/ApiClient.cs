using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Serilog;
using Shopping_App;
using Shopping_App.Hellpers;
using ShoppingApp.Api.Models;
using static System.Net.WebRequestMethods;

namespace ShoppingApp.Api
{
    public class ApiClient
    {
        public readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            Log.Information("Initializing API client Http");


            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(Config.GetApiBaseUri());
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void SetAuthorizationToken(string token)
        {
            Log.Information("Setting authorization token");
            Config.SetCurrentUserAccessToken(token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        public void SetTokens(string accessToken, string refreshToken)
        {
            Log.Information("Setting access and refresh tokens");
            SetAuthorizationToken(accessToken);
            SetRefreshToken(refreshToken);
        }

        private void SetRefreshToken(string token)
        {
            Log.Information("Setting refresh token");
            Config.SetCurrentUserRefreshToken(token);
        }

        protected async Task<T> GetAsync<T>(string endpoint, string queryString = "")
        {
            Log.Information("Sending GET request to {Endpoint} with query string: {QueryString}", endpoint, queryString);
            string url = string.IsNullOrEmpty(queryString) ? endpoint : $"{endpoint}?{queryString}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            await HandleResponseErrors(response);
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        protected async Task<T> PostAsync<T>(string endpoint, object data = null)
        {
            Log.Information("Sending POST request to {Endpoint} with data: {Data}", endpoint, data);
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
            Log.Information("Sending POST request to {Endpoint} with data: {Data}", endpoint, data);
            HttpContent content = data == null
            ? null
                : new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);
            await HandleResponseErrors(response);
        }

        protected async Task PutAsync(string endpoint, string queryString)
        {
            Log.Information("Sending PUT request to {Endpoint} with query string: {QueryString}", endpoint, queryString);
            string url = string.IsNullOrEmpty(queryString) ? endpoint : $"{endpoint}?{queryString}";
            HttpResponseMessage response = await _httpClient.PutAsync(url, null);
            await HandleResponseErrors(response);
        }

        protected async Task<T> PutAsync<T>(string endpoint, object data, string queryString = "")
        {
            Log.Information("Sending PUT request to {Endpoint} with data: {Data} and query string: {QueryString}", endpoint, data, queryString);
            string url = string.IsNullOrEmpty(queryString) ? endpoint : $"{endpoint}?{queryString}";
            HttpContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(url, content);
            await HandleResponseErrors(response);
            string responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        protected async Task DeleteAsync(string endpoint, string queryString = "")
        {
            
            Log.Information("Sending DELETE request to {Endpoint} with query string: {QueryString}", endpoint, queryString);
            string url = string.IsNullOrEmpty(queryString) ? endpoint : $"{endpoint}?{queryString}";
            Console.WriteLine($"query is {queryString}");
            Console.WriteLine($"url is {url}");
            HttpResponseMessage response = await _httpClient.DeleteAsync(url);
            await HandleResponseErrors(response);
        }

        private async Task HandleResponseErrors(HttpResponseMessage response)
        {
            Log.Information("Handling response errors for status code: {StatusCode}", response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                ProblemDetails problemDetails = null;

                try
                {
                    problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(errorContent);
                    Log.Error($"Error response: [Title: {problemDetails?.Title}] [Details: {problemDetails?.Detail}] [Path: {problemDetails?.Instance}]");
                }
                catch
                {
                    // If parsing fails, problemDetails remains null
                }

                string errorMessage = problemDetails != null
                    ? $"{problemDetails.Title}: {problemDetails.Detail} {problemDetails.Instance}"
                    : $"Error: {response.StatusCode} - {errorContent}";

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.BadRequest:
                        throw new ApiException(400, errorMessage);
                    case System.Net.HttpStatusCode.Unauthorized:
                        throw new ApiException(401, "Authentication required. Please login.");
                    case System.Net.HttpStatusCode.Forbidden:
                        throw new ApiException(403, "You don't have permission to access this resource.");
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
        public int Status { get; set; }
        public string TraceId { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }
    }
}