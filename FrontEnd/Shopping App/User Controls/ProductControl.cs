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
        private bool _suppressQuantityEvent;

        public delegate void OnProductStatusChanged(bool IsInCart, ProductDto product);
        public event OnProductStatusChanged ProductStatusChanged;

        public ProductControl(ProductDto product)
        {
            InitializeComponent();
            _product = product;

            Quentity.Maximum = _product.maxQuantity;

            if (_product.Quantity != 0)
            {
                _isInCart = true;
                btnAddRemove.Text = "Remove from Cart";
                Quentity.Value = _product.Quantity;
            }

            lbProductName.Text = _product.ProductName;
            lbCategory.Text = _product.ProductCategory;
            lbPrice.Text = _product.Price.ToString("C") + "$";
            lbDescription.Text = string.IsNullOrEmpty(_product.ProductDescription) ? "" : "Description: " + _product.ProductDescription.Substring(0, Math.Min(60, _product.ProductDescription.Length)) + "...";
        }

        private void btnAddRemove_Click(object sender, EventArgs e)
        {
            if (_isInCart)
            {
                _isInCart = false;
                btnAddRemove.Text = "Add to Cart";
                _suppressQuantityEvent = true;
                Quentity.Value = 0;
                _product.Quantity = 0;
                _suppressQuantityEvent = false;
                ProductStatusChanged?.Invoke(_isInCart, _product);
            }
            else
            {
                _isInCart = true;
                btnAddRemove.Text = "Remove from Cart";
                _product.Quantity = 1;
                _suppressQuantityEvent = true;
                Quentity.Value = 1;
                _suppressQuantityEvent = false;
                ProductStatusChanged?.Invoke(_isInCart, _product);
            }
        }

        private void Quentity_ValueChanged(object sender, EventArgs e)
        {
            if (_suppressQuantityEvent)
                return;
            
            if (Quentity.Value == 0)
            {
                _isInCart = false;
                btnAddRemove.Text = "Add to Cart";
                _product.Quantity = 0;
                ProductStatusChanged?.Invoke(_isInCart, _product);
            }
            else
            {
                _isInCart = true;
                btnAddRemove.Text = "Remove from Cart";
                _product.Quantity = (int)Quentity.Value;
                ProductStatusChanged?.Invoke(_isInCart, _product);
            }
        }
    }
}
