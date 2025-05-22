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
    public partial class ShippingInfoForm : Form
    {
        public delegate void OnCheckOutConfirmed(string ShippingAddress, string PaymentMethod);
        public event OnCheckOutConfirmed CheckOutConfirmed;
        public ShippingInfoForm()
        {
            InitializeComponent();
        }

        private void btnConfirme_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(tbShippingAddress.Text) || cbPaymentMethod.SelectedIndex == -1)
            {
                MessageBox.Show("Shipping Address and/or PaymentMethod Can't by empty.","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CheckOutConfirmed?.Invoke(tbShippingAddress.Text, (string)cbPaymentMethod.SelectedValue);
        }
    }
}
