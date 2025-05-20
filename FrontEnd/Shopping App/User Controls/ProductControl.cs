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
    public partial class ProductControl : UserControl
    {
        private ProductDto _product;
        private bool _isInCart = false;

        public delegate void OnProductStatusChanged(bool IsInCart);
        public event OnProductStatusChanged ProductStatusChanged;


        public ProductControl()
        {
            InitializeComponent();
            this.Size = new Size(225, 150);
        }
        public ProductControl(ProductDto product)
        {
            InitializeComponent();
            _product = product;

            lbProductName.Text = _product.ProductName;
            lbCategory.Text = _product.ProductCategory;
            lbPrice.Text = _product.Price.ToString("C") + "$";
            lbDescription.Text = "Description: " + _product.ProductDescription.Substring(0, 60) + "...";
        }

        private void btnAddRemove_Click(object sender, EventArgs e)
        {
            if (_isInCart)
            {
                _isInCart = false;
                btnAddRemove.Text = "Add to Cart";
                ProductStatusChanged?.Invoke(_isInCart);
            }
            else
            {
                _isInCart = true;
                btnAddRemove.Text = "Remove from Cart";
                ProductStatusChanged?.Invoke(_isInCart);
            }
        }
    }
}
