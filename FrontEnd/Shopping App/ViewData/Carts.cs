using Shopping_App.Forms;
using Shopping_App.Hellpers;
using Shopping_App.User_Controls;
using ShoppingApp.Api;
using ShoppingApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Shopping_App.ViewData
{
    internal class Carts
    {
        public static List<ProductDto> CartProducts = new List<ProductDto>();
        private static Label TotalPriceLabel;
        private static Button SaveBtn;
        private static Button ClearCartBtn;
        private static Button CheckoutBtn;

        public static void LoadCartItems(Form form)
        {
            // Clear existing controls
            HellpersMethodes.ClearForm(form);

            form.AutoScroll = true;
            AddTotalPriceLabelAndButtons(form);

            // Create new controls for each product in the cart
            int i = 0;
            foreach (var p in CartProducts)
            {
                CartItemControl cartItem = new CartItemControl(p);
                cartItem.Location = new Point(10, 40 + 30 + 60 * i);
                cartItem.CartItemStatusChanged += (isInCart, product) =>
                {
                    if (isInCart)
                    {
                        AddUpdateCart(product);
                    }
                    else
                    {
                        RemoveFromCart(product);
                        LoadCartItems(form);
                    }
                };
                form.Controls.Add(cartItem);
                cartItem.BringToFront();
                i++;
            }

        }
        public static void AddUpdateCart(ProductDto product)
        {
            var existingProduct = CartProducts.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Quantity = product.Quantity;
            }
            else
            {
                CartProducts.Add(product);
            }
            if (TotalPriceLabel != null) TotalPriceLabel.Text = "Total Price: " + CartProducts.Sum(p => p.Price * p.Quantity).ToString("C") + "$";
        }
        public static void RemoveFromCart(ProductDto product)
        {
            CartProducts.Remove(CartProducts.First(p => p.Id == product.Id));
        }

        public static int GetProductQuentityInCart(int id)
        {
            return CartProducts.Where(p => p.Id == id).FirstOrDefault()?.Quantity ?? 0;
        }
        public static async Task FetchAndUpdateCart()
        {
            CartDto UserCart;
            try
            {
                UserCart = await ApiManger.Instance.CartService.GetUserCartAsync(Config.GetCurrentUserId());
                if (UserCart != null)
                {
                    CartProducts = UserCart.Products;
                }
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == (int)System.Net.HttpStatusCode.NotFound)
                {
                    CartProducts = new List<ProductDto>();
                    Config.SetCurrentUserCartId(0);
                    return;
                }
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UserCart.Products.ForEach(p => AddUpdateCart(p));
            Config.SetCurrentUserCartId(UserCart.CartId);
        }
        public static async void SaveCart(object sender, EventArgs e)
        {
            CartDto cart = new CartDto
            {
                UserId = Config.GetCurrentUserId(),
                CartId = Config.GetCurrentUserCartId(),
                Products = CartProducts
            };
            try
            {
                if(cart.CartId == 0)
                {
                    cart = await ApiManger.Instance.CartService.AddCartAsync(cart);
                    Config.SetCurrentUserCartId(cart.CartId);
                }
                else
                {
                    cart = await ApiManger.Instance.CartService.UpdateCartAsync(cart);
                    Config.SetCurrentUserCartId(cart.CartId);
                }
            }
            catch (ApiException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (sender != null ) MessageBox.Show("Cart saved successfully!");
            
        }
        private static void AddTotalPriceLabelAndButtons(Form form)
        {
            SaveBtn = new Button();
            SaveBtn.Text = "Save Cart";
            SaveBtn.Size = new Size(100, 30);
            SaveBtn.Location = new Point(10, 30);
            SaveBtn.Click += SaveCart;
            form.Controls.Add(SaveBtn);
            SaveBtn.BringToFront();

            ClearCartBtn = new Button();
            ClearCartBtn.Text = "Clear Cart";
            ClearCartBtn.Size = new Size(100, 30);
            ClearCartBtn.Location = new Point(120, 30);
            ClearCartBtn.Click += ClearCart;
            form.Controls.Add(ClearCartBtn);
            ClearCartBtn.BringToFront();


            CheckoutBtn = new Button();
            CheckoutBtn.Text = "Checkout";
            CheckoutBtn.Size = new Size(100, 30);
            CheckoutBtn.Location = new Point(230, 30);
            CheckoutBtn.Click += Checkout;
            form.Controls.Add(CheckoutBtn);
            CheckoutBtn.BringToFront();


            TotalPriceLabel = new Label();
            TotalPriceLabel.Text = "Total Price: " + CartProducts.Sum(p => p.Price * p.Quantity).ToString("C") + "$";
            TotalPriceLabel.Size = new Size(200, 30);
            TotalPriceLabel.Font = new Font("Arial", 12, FontStyle.Bold);
            TotalPriceLabel.Location = new Point(450, 30);
            form.Controls.Add(TotalPriceLabel);
            TotalPriceLabel.BringToFront();
        }



        private static async void ClearCart(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to clear the cart?", "Confirm", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                CartProducts.Clear();
                TotalPriceLabel.Text = "Total Price: 0$";
                await ApiManger.Instance.CartService.DeleteCartAsync(Config.GetCurrentUserCartId());
                HellpersMethodes.ClearForm((sender as Button).Parent as Form);
                MessageBox.Show("Cart cleared successfully!");
            }
        }

        private static void Checkout(object sender, EventArgs e)
        {
            SaveCart(null, null);
            ShippingInfoForm shippingInfoForm = new ShippingInfoForm();
            shippingInfoForm.CheckOutConfirmed += async (string shippingAddress, string paymentMethod) =>
            {
                try
                {
                    await ApiManger.Instance.OrderService.MakeOrderAsync(Config.GetCurrentUserId(), shippingAddress, paymentMethod);
                    CartProducts.Clear();
                    Form form = (sender as Button).Parent as Form;
                    HellpersMethodes.ClearForm(form);
                    AddTotalPriceLabelAndButtons(form);
                    TotalPriceLabel.Text = "Total Price: 0$";
                    MessageBox.Show("Order placed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    shippingInfoForm.Close();
                }
                catch (ApiException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            };
            shippingInfoForm.ShowDialog();
        }

    }
}
