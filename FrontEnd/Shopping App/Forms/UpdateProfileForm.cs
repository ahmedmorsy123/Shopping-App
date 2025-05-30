using Shopping_App.Hellpers;
using ShoppingApp.Api;
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
    public partial class UpdateProfileForm : Form
    {
        public UpdateProfileForm()
        {
            InitializeComponent();

            tbUserName.Text = Config.GetCurrentUserName();
            tbEmail.Text = Config.GetCurrentUserEmail();
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if(tbNewPassword.Text != tbConfPassword.Text)
            {
                MessageBox.Show("New Password doesn't match the Confirmation Password", "Error" ,MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if(string.IsNullOrEmpty(tbOldPassword.Text))
            {
                MessageBox.Show("Please fill the old Password Section", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            UpdateUserDto user = new UpdateUserDto
            {
                Id = Config.GetCurrentUserId(),
                Name = tbUserName.Text == Config.GetCurrentUserName() ? "" : tbUserName.Text,
                Email = tbEmail.Text == Config.GetCurrentUserEmail() ? "" : tbEmail.Text,
                OldPassword = tbOldPassword.Text,
                NewPassword = tbNewPassword.Text
            };

            try
            {
                UserDto result = await ApiManger.Instance.UserService.UpdateUserAsync(user);
            }
            catch (ApiException ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowPassword.Checked)
            {
                tbOldPassword.UseSystemPasswordChar = false;
                tbNewPassword.UseSystemPasswordChar = false;
                tbConfPassword.UseSystemPasswordChar = false;
            }
            else
            {
                tbOldPassword.UseSystemPasswordChar = true;
                tbNewPassword.UseSystemPasswordChar = true;
                tbConfPassword.UseSystemPasswordChar = true;
            }
        }
    }
}
