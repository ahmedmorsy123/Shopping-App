using Serilog;
using Shopping_App.Hellpers;
using Shopping_App.ViewData;
using ShoppingApp.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ShoppingAppDB.Enums.Enums;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Shopping_App.Forms
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
            currentUserToolStripMenuItem.Text = Config.GetCurrentUserName();
            Users.SetForm(this);

            this.Text = "Admin DashBoard";
            this.Size = new Size(715, 730);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.SizeGripStyle = SizeGripStyle.Hide;

            Users.AddDGV();

            Users.AddTitle("");


            Console.WriteLine("constroctor");
        }


        private async void AdminForm_Load(object sender, EventArgs e)
        {
            Console.WriteLine("load");
            await Users.LoadAdmins();
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

        private async void lowStockProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Products.LoadLowStockProducts(this);
        }

        private async void outOfStockProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Products.GetOutOfStockProducts(this);
        }

        private async void getCartsCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int count = await ApiManger.Instance.CartService.GetCartsCountAsync();
            MessageBox.Show($"Total Carts Count: {count}", "Carts Count", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void listOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Orders.AddFilters(TimeDuration.AllTime, OrderStatus.All, this);
            await Orders.LoadAllOrders(TimeDuration.AllTime, OrderStatus.All, this);
        }

        private async void listUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Users.LoadUsers();
        }

        private async void listAdminsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Users.LoadAdmins();
        }

        private void registerationCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLoginRegisterCountForm form = new ShowLoginRegisterCountForm("Register Count", false);
            form.ShowDialog();
        }

        private void logInCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLoginRegisterCountForm form = new ShowLoginRegisterCountForm("LogIn Count", true);
            form.ShowDialog();
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

        private async void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Carts.SaveCart(null, null);
            if (string.IsNullOrEmpty(Config.GetRememberedRefreshToken()))
                await LogOut();

            Config.ClearCurrentUser();
        }
    }
}
