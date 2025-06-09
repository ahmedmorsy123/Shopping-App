using Shopping_App.ViewData;
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

namespace Shopping_App.Forms
{
    public partial class ShowLoginRegisterCountForm : Form
    {
        private bool _isLogin;
        public ShowLoginRegisterCountForm(string title, bool Login)
        {
            InitializeComponent();

            foreach (var duration in Enum.GetValues(typeof(TimeDuration)))
            {
                comboBox.Items.Add(duration);
            }

            this.Text = title;
            lbCountTitle.Text = title + ":";
            _isLogin = Login;
        }

        private async void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = 0;
            if (_isLogin)
            {
                count = await Users.GetLogInCount((TimeDuration)comboBox.SelectedItem);
            }
            else
            {
                count = await Users.GetRegisterationCount((TimeDuration)comboBox.SelectedItem);
            }

            lbCount.Text = count.ToString();
        }
    }
}
