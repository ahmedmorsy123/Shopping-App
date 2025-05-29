using Shopping_App.User_Controls;
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
    public partial class OrderItemsListForm : Form
    {

        public OrderItemsListForm(List<ProductDto> products)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.Text = "Order Items List";
            this.Size = new Size(540, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.AutoScroll = true;

            foreach (var product in products)
            {
                OrderItemControl orderItemControl = new OrderItemControl(product);
                orderItemControl.Location = new Point(10, 10 + 60 * products.IndexOf(product));
                this.Controls.Add(orderItemControl);
            }

        }
    }
}
