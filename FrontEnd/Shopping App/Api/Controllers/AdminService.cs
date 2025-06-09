using Serilog;
using Shopping_App.Hellpers;
using ShoppingApp.Api;
using ShoppingApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shopping_App.Api.Controllers
{
    internal class AdminService : ApiClient
    {
        public AdminService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<UserDto> AddAdminAsync(UserDto admin)
        {
            try
            {
                var endpoint = Config.GetApiEndpoint("Admin", "AddAdmin");
                var result = await PostAsync<UserDto>(endpoint, admin);
                return result;
            }
            catch (ApiException ex) when (ex.StatusCode == 400)
            {
                return null;
            }
            catch (ApiException ex) when (ex.StatusCode == 401 || ex.StatusCode == 403)
            {
                throw;
            }
        }

        public async Task<bool> RemoveAdminAsync(int adminId)
        {
            try
            {
                var endpoint = Config.GetApiEndpoint("Admin", "RemoveAdmin");
                await DeleteAsync(endpoint, $"adminId={adminId}");
                return true;
            }
            catch (ApiException ex) when (ex.StatusCode == 404)
            {
                return false;
            }
            catch (ApiException ex) when (ex.StatusCode == 400)
            {
                Log.Error("Failed to remove admin with ID: {AdminId}", adminId);
                throw;
            }
            catch (ApiException ex) when (ex.StatusCode == 401 || ex.StatusCode == 403)
            {
                throw;
            }
        }

        public async Task<bool> MakeAdminAsync(int userId)
        {
            try
            {
                var endpoint = Config.GetApiEndpoint("Admin", "MakeAdmin");
                await PutAsync<object>(endpoint, userId);
                return true;
            }
            catch (ApiException ex) when (ex.StatusCode == 404)
            {
                return false;
            }
            catch (ApiException ex) when (ex.StatusCode == 400)
            {
                return false;
            }
            catch (ApiException ex) when (ex.StatusCode == 401 || ex.StatusCode == 403)
            {
                throw;
            }
        }

        public async Task<List<UserDto>> ListAdminsAsync()
        {
            try
            {
                var endpoint = Config.GetApiEndpoint("Admin", "ListAdmins");
                var admins = await GetAsync<List<UserDto>>(endpoint);
                return admins;
            }
            catch (ApiException ex) when (ex.StatusCode == 404)
            {
                return (List<UserDto>)Enumerable.Empty<UserDto>();
            }
            catch (ApiException ex) when (ex.StatusCode == 401 || ex.StatusCode == 403)
            {
                throw;
            }
        }
    }
}
