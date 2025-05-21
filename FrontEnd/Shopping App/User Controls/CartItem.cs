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

namespace Shopping_App.User_Controls
{
    public partial class CartItem : UserControl
    {
        private ProductDto _product;
        public CartItem(ProductDto product, int MaxQuentity)
        {
            InitializeComponent();
            this.Size = new Size(680, 50);


            _product = product;

            Quantity.Maximum = MaxQuentity;

            lbProductName.Text = _product.ProductName;
            Quantity.Value = _product.Quantity;
            lbTotalPrice.Text = (_product.Price * _product.Quantity).ToString("C") + "$";
        }

        public CartItem()
        {
            InitializeComponent();
            this.Size = new Size(680, 50);
        }

        private void Quantity_ValueChanged(object sender, EventArgs e)
        {

        }

        private void lbRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
