using Serilog;
using Shopping_App.User_Controls;
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
        public MainForm()
        {
            InitializeComponent();


            this.Text = "Shopping App";
            this.Size = new Size(715, 730);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.SizeGripStyle = SizeGripStyle.Hide;

            AuthService authService = new AuthService();
            Console.WriteLine(AppState.CurrentLoggedInUser.Id);
            UsersService usersService = new UsersService();
            Console.WriteLine(AppState.CurrentLoggedInUser.Id);


        }



        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            await LogOut();
        }

        private async void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductsService productsService = new ProductsService();
            PagedList<ProductDto> products;
            try
            {
                products = await productsService.GetProductsAsync("","","",1,12);
            }
            catch(ApiException ex) 
            {
                Log.Error(ex.Message);
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            int i = 0;
            foreach (ProductDto product in products.Items)
            {
                ProductControl productControl = new ProductControl(product);
                productControl.Location = new Point(230 * (i % 3) + 5, 160 * (i / 3) + 30);
                this.Controls.Add(productControl);
                i++;
            }

        }

        private void myCartToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void myOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private async void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await LogOut();

        }

        private async Task LogOut()
        {
            AuthService authService = new AuthService();
            try
            {
                await authService.LogoutAsync();
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
