using Shopping_App.Api.Controllers;
using ShoppingApp.Api.Controllers;
using ShoppingApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_App
{
    internal class ApiManger
    {
        private static readonly Lazy<ApiManger> _instance = new Lazy<ApiManger>(() => new ApiManger());
        private HttpClient _httpClient = new HttpClient();

        public AuthService AuthService { get; }
        public CartsService CartService { get; }
        public OrdersService OrderService { get; }
        public ProductsService ProductService { get; }
        public UsersService UserService { get; }
        public AdminService AdminService { get; }


        private ApiManger()
        {
            AuthService = new AuthService(_httpClient);
            CartService = new CartsService(_httpClient);
            OrderService = new OrdersService(_httpClient);
            ProductService = new ProductsService(_httpClient);
            UserService = new UsersService(_httpClient);
            AdminService = new AdminService(_httpClient);
        }

        public static ApiManger Instance => _instance.Value;
    }

}

