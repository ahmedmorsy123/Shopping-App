using Serilog;
using Shopping_App.Hellpers;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Shopping_App.Forms
{
    public partial class MainForm : Form
    {

        private List<ProductDto> CartProducts = new List<ProductDto>();

        public MainForm()
        {
            InitializeComponent();
            currentUserToolStripMenuItem.Text = Config.GetCurrentUserName();

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
            if(string.IsNullOrEmpty(Config.GetRememberedRefreshToken()))
                await LogOut();

            Config.ClearCurrentUser();
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
            Config.ClearRemeberedData();
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

        private async void deleteAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete your account? This action cannot be undone.", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    await ApiManger.Instance.UserService.DeleteUserAsync(Config.GetCurrentUserId());
                    Config.ClearRemeberedData();
                    Config.ClearCurrentUser();
                    MessageBox.Show("Account deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (ApiException ex)
                {
                    Log.Error($"Delete Account Faild {ex.Message}");
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
