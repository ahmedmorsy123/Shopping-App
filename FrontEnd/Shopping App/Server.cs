using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingApp.Api.Controllers;

namespace Shopping_App.Api
{
    public class Server
    {
        private AuthService _authService;
        private CartsService _cartsService;  
        private OrdersService _ordersService;
        private ProductsService _productsService;
        private UsersService _usersService;

        public Server()
        {
            _authService = new AuthService();
            _cartsService = new CartsService(); 
            _ordersService = new OrdersService();
            _productsService = new ProductsService();
            _usersService = new UsersService();
        }

    }
}
