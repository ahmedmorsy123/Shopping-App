using Shopping_App.Forms;
using Shopping_App.Hellpers;
using Shopping_App.User_Controls;
using ShoppingApp.Api;
using ShoppingApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Shopping_App.ViewData
{
    internal class Orders
    {
        public static async Task LoadOrders(Form form)
        {
            // Clear existing controls
            HellpersMethodes.ClearForm(form);
            form.AutoScroll = true;
            // Fetch orders from API
            int id = Config.GetCurrentUserId();
            List<OrderDto> orders = await ApiManger.Instance.OrderService.GetUserOrdersAsync(id);
            int i = 0;
            foreach (var order in orders)
            {
                OrderControl orderControl = new OrderControl(order);
                orderControl.Location = new Point(10, 30 + 110 * i);
                form.Controls.Add(orderControl);
                orderControl.BringToFront();
                i++;
            }
        }
    }
}
