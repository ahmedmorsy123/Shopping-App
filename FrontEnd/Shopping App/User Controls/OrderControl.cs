using Shopping_App.Forms;
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
    public partial class OrderControl : UserControl
    {
        private OrderDto _order;
        public OrderControl(OrderDto order = null)
        {
            InitializeComponent();
            this.Size = new Size(680, 100);
            _order = order;

            if (_order != null)
            {
                lbProductsCount.Text = _order.OrderItems.Count.ToString();
                lbTotalPrice.Text = _order.TotalPrice.ToString("C") + "$";
                lbShippingAddress.Text = _order.ShippingAddress;
                lbPaymentMethod.Text = _order.PaymentMethod;
                lbStatus.Text = _order.Status.ToString();
                lbCreatedAt.Text = _order.CreatedAt.ToString("g"); // General date/time pattern (short time)
            }
            else
            {
                lbProductsCount.Text = "0";
                lbTotalPrice.Text = "0.00$";
                lbShippingAddress.Text = "N/A";
                lbPaymentMethod.Text = "N/A";
                lbStatus.Text = "N/A";
                lbCreatedAt.Text = "N/A";
            }
        }

        private async void btnCancel_Click(object sender, EventArgs e)
        {
            await ApiManger.Instance.OrderService.CancelOrderAsync(_order.Id);
            lbStatus.Text = "Cancelled";
            _order.Status = "Cancelled";
            MessageBox.Show("Order cancelled successfully!");

        }

        private void btnViewItems_Click(object sender, EventArgs e)
        {

            OrderItemsListForm orderItemsForm = new OrderItemsListForm(_order.OrderItems);
            orderItemsForm.ShowDialog();
        }
    }
}
