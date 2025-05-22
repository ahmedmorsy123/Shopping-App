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
    public partial class CartItemControl : UserControl
    {
        private ProductDto _product;
        public delegate void OnCartItemStatusChanged(bool IsInCart, ProductDto product);
        public event OnCartItemStatusChanged CartItemStatusChanged;
        public CartItemControl(ProductDto product)
        {
            InitializeComponent();
            this.Size = new Size(680, 50);


            _product = product;

            Quentity.Maximum = product.maxQuantity;

            lbProductName.Text = _product.ProductName;
            Quentity.Value = _product.Quantity;
            lbTotalPrice.Text = (_product.Price * _product.Quantity).ToString("C") + "$";
        }


        private void Quantity_ValueChanged(object sender, EventArgs e)
        {
            if(Quentity.Value == 0)
            {
                CartItemStatusChanged?.Invoke(false, _product);
                return;
            }
            _product.Quantity = (int)Quentity.Value;
            lbTotalPrice.Text = (_product.Price * _product.Quantity).ToString("C") + "$";
            CartItemStatusChanged?.Invoke(true, _product);
        }

        private void lbRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CartItemStatusChanged?.Invoke(false, _product);
        }
    }
}
