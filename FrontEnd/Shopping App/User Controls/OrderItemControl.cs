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
    public partial class OrderItemControl : UserControl
    {
        public OrderItemControl(ProductDto product)
        {
            InitializeComponent();
            this.Size = new Size(520, 50);

            lbProductName.Text = product.ProductName;
            lbQuentity.Text = product.Quantity.ToString();
            lbTotalPrice.Text = (product.Price * product.Quantity).ToString("C") + "$";
        }
    }
}
