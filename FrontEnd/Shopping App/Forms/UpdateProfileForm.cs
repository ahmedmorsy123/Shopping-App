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
    public partial class UpdateProfileForm : Form
    {
        public UpdateProfileForm()
        {
            InitializeComponent();

            tbUserName.Text = ApiManger.CurrentLoggedInUser.Name;
            tbEmail.Text = ApiManger.CurrentLoggedInUser.Email;
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
                Id = ApiManger.CurrentLoggedInUser.Id,
                Name = tbUserName.Text == ApiManger.CurrentLoggedInUser.Name ? "" : tbUserName.Text,
                Email = tbEmail.Text == ApiManger.CurrentLoggedInUser.Email ? "" : tbEmail.Text,
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
