using Serilog;
using Shopping_App.User_Controls;
using Shopping_App.ViewData;
using ShoppingApp.Api;
using ShoppingApp.Api.Controllers;
using ShoppingApp.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shopping_App.Forms
{
    public partial class MainForm : Form
    {

        // Carts
        private List<ProductDto> CartProducts = new List<ProductDto>();

        public MainForm()
        {
            InitializeComponent();

            currentUserToolStripMenuItem.Text = ApiManger.CurrentLoggedInUser.Name;

            this.Text = "Shopping App";
            this.Size = new Size(715, 730);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.SizeGripStyle = SizeGripStyle.Hide;

            FetchAndUpdateCart();
            LoadProducts();
        }

        private async void FetchAndUpdateCart()
        {
            await Carts.FetchAndUpdateCart();
        }

        private async void LoadProducts()
        {
            await Products.LoadProducts(1, 12, this, null, null, null);

        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Carts.SaveCart(null, null);
            await LogOut();
        }

        private async void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Products.LoadProducts(1,12,this);
        }

        private void myCartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Carts.LoadCartItems(this);

        }

        private async void myOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Orders.LoadOrders(this);

        }

        private void updateProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateProfileForm updateProfileForm = new UpdateProfileForm();
            updateProfileForm.ShowDialog();
        }

        private async void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await LogOut();
            this.Close();
        }

        private async Task LogOut()
        {
            try
            {
                await ApiManger.Instance.AuthService.LogoutAsync();
            }
            catch (ApiException ex)
            {
                Log.Error($"Logout Faild {ex.Message}");
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

       


    }
}
