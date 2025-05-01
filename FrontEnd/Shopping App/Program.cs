using Shopping_App.APIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Shopping_App.APIs.Carts;
using static Shopping_App.APIs.Orders;
using static Shopping_App.APIs.Products;
using static Shopping_App.APIs.Users;

namespace Shopping_App
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            await Users.Login("Ahmed", "3420");

            Cart newCart = new Cart();
            newCart.CartId = 4050;


            var products = (await Products.GetProductsAsPaginated(55)).Take(2).ToList();
            newCart.Products = products;

            var cart = await Carts.UpdateCart(newCart);

            Console.WriteLine($"cart is null: {cart == null}");
            Console.WriteLine($"cartId: {cart.CartId}, cartCreatedAt: {cart.CreatedAt}, cartUpdatedAt: {cart.UpdatedAt}, cartProducts: {cart.Products.Count}");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
