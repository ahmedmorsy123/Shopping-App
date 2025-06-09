using Serilog;
using Shopping_App.Hellpers;
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
    public partial class LoginRegisterForm : Form
    {
        public bool IsAdmin;
        public LoginRegisterForm()
        {
            InitializeComponent();
            

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.SizeGripStyle = SizeGripStyle.Hide;

            tabControl.SelectedTab = tabLogin;
            this.Text = "Login";
        }

        private async void Login_Click(object sender, EventArgs e)
        {
            string username = txtUserNameLogin.Text;
            string password = txtPasswordLogin.Text;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            TokenResponseDto tokenResponse;

            try
            {
                tokenResponse = await ApiManger.Instance.AuthService.LoginAsync(username, password);
                IsAdmin = HellpersMethodes.IsUserAdmin(tokenResponse.AccessToken);
            }
            catch (ApiException ex)
            {
                Log.Error($"Login failed: {ex.Message}");
                MessageBox.Show($"Login failed: {ex.Message}");
                return;
            }

            if (cbRememberMe.Checked)
            {
                Config.SetRememberMe(tokenResponse.RefreshToken, Config.GetCurrentUserId());
            }


            Log.Information($"User {username} Loged in Successfully");
            MessageBox.Show("Login successful!");

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void btnClearLogin_Click(object sender, EventArgs e)
        {
            txtUserNameLogin.Text = string.Empty;
            txtPasswordLogin.Text = string.Empty;
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUserNameRegister.Text;
            string password = txtPasswordRegister.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string email = txtEmail.Text;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            UserDto user = new UserDto
            {
                Name = username,
                Password = password,
                Email = email
            };
            try
            {
                await ApiManger.Instance.UserService.AddUserAsync(user);
            }
            catch (ApiException ex)
            {
                Log.Error($"Registration failed: {ex.Message} status code {ex.StatusCode}");
                MessageBox.Show($"Registration failed: {ex.Message} status code {ex.StatusCode}");
                return;
            }

            Log.Information($"User {user.Name} Registared successfully");

            MessageBox.Show("Registration successful!");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnClearRegister_Click(object sender, EventArgs e)
        {
            txtUserNameRegister.Text = string.Empty;
            txtPasswordRegister.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            txtEmail.Text = string.Empty;
        }

        private void cbShowPasswordLogin_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowPasswordLogin.Checked)
            {
                txtPasswordLogin.UseSystemPasswordChar = false;
            }
            else
            {
                txtPasswordLogin.UseSystemPasswordChar = true;
            }
        }

        private void cbShowPasswordRegister_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowPasswordRegister.Checked)
            {
                txtPasswordRegister.UseSystemPasswordChar = false;
                txtConfirmPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPasswordRegister.UseSystemPasswordChar = true;
                txtConfirmPassword.UseSystemPasswordChar = true;
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabLogin)
            {
                this.Text = "Login";
            }
            else if (tabControl.SelectedTab == tabRegister)
            {
                this.Text = "Register";
            }
        }
    }
}
