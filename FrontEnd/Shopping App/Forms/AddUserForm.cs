using Shopping_App.ViewData;
using ShoppingApp.Api;
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
    public partial class AddUserForm : Form
    {
        public AddUserForm()
        {
            InitializeComponent();
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowPassword.Checked)
            {
                tbNewPassword.UseSystemPasswordChar = false;
                tbConfPassword.UseSystemPasswordChar = false;
            }
            else
            {
                tbNewPassword.UseSystemPasswordChar = true;
                tbConfPassword.UseSystemPasswordChar = true;
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbNewPassword.Text != tbConfPassword.Text)
            {
                MessageBox.Show("New Password doesn't match the Confirmation Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(tbUserName.Text) || string.IsNullOrEmpty(tbEmail.Text) || string.IsNullOrEmpty(tbNewPassword.Text))
            {
                MessageBox.Show("Please fill all the fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UserDto userDto = new UserDto
            {
                Name = tbUserName.Text,
                Email = tbEmail.Text,
                Password = tbNewPassword.Text,
            };

            try
            {
                await ApiManger.Instance.AdminService.AddAdminAsync(userDto);
                tbUserName.Text = tbNewPassword.Text = tbConfPassword.Text = tbEmail.Text = string.Empty; // Clear fields after adding
                MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(ApiException ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message} \n status code: {ex.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
