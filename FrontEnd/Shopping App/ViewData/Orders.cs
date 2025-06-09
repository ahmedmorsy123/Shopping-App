using Serilog;
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
using static ShoppingAppDB.Enums.Enums;

namespace Shopping_App.ViewData
{
    internal class Orders
    {
        public static async Task LoadUserOrders(Form form)
        {
            // Clear existing controls
            HellpersMethodes.ClearForm(form);
            form.AutoScroll = true;
            // Fetch orders from API
            int id = Config.GetCurrentUserId();
            List<OrderDto> orders;
            try
            {
                orders = await ApiManger.Instance.OrderService.GetUserOrdersAsync(id);
            }
            catch (ApiException ex)
            {
                Log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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

        public static async Task LoadAllOrders(TimeDuration duration, OrderStatus status, Form form)
        {
            List<OrderDto> orders;
            try
            {
                orders = await ApiManger.Instance.OrderService.GetOrdersByDurationAndStatusAsync(duration, status);
            }
            catch (ApiException ex)
            {
                Log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var title = form.Controls.Find("_title", true).FirstOrDefault() as Label;
            title.Text = $"Orders List - Duration: {duration}, Status: {status}";

            var dgv = form.Controls.Find("_dgv", true).FirstOrDefault() as DataGridView;
            if (dgv == null)
            {
                MessageBox.Show("DataGridView '_dgv' not found on the form.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgv.DataSource = orders;

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripItem processOrder = new ToolStripMenuItem("Process Order", null, (s, e) => ProcessOrder(form));
            ToolStripItem shipOrder = new ToolStripMenuItem("Ship Order", null, (s,e) =>  ShipOrder(form));
            ToolStripItem deliverOrder = new ToolStripMenuItem("Deliver Order", null, (s, e) => DeliverOrder(form));
            contextMenu.Items.Add(processOrder);
            contextMenu.Items.Add(shipOrder);
            contextMenu.Items.Add(deliverOrder);
            dgv.ContextMenuStrip = contextMenu;
        }

        private static async void DeliverOrder(Form form)
        {
            var dgv = form.Controls.Find("_dgv", true).FirstOrDefault() as DataGridView;

            if (dgv.CurrentRow != null && dgv.CurrentRow.DataBoundItem is OrderDto order)
            {
                try
                {
                        Console.WriteLine($"dddd {OrderStatus.Shipped.ToString()}");
                    if (order.Status != OrderStatus.Shipped.ToString() || order.Status == OrderStatus.Cancelled.ToString() || order.Status == OrderStatus.Delivered.ToString())
                    {
                        MessageBox.Show("Order must be shipped before it can be delivered.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    await ApiManger.Instance.OrderService.DeliverOrderAsync(order.Id);
                    MessageBox.Show("Order delivered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await LoadAllOrders(TimeDuration.AllTime, OrderStatus.All, form);
                }
                catch (ApiException ex)
                {
                    Log.Error(ex.Message);
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No user selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static async void ShipOrder(Form form)
        {
            var dgv = form.Controls.Find("_dgv", true).FirstOrDefault() as DataGridView;
            if (dgv.CurrentRow != null && dgv.CurrentRow.DataBoundItem is OrderDto order)
            {
                try
                {
                    if (order.Status != OrderStatus.Processing.ToString() || order.Status == OrderStatus.Cancelled.ToString() || order.Status == OrderStatus.Delivered.ToString())
                    {
                        MessageBox.Show("Order must be processed before it can be shipped.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    await ApiManger.Instance.OrderService.ShipOrderAsync(order.Id);
                    MessageBox.Show("Order shipped successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadAllOrders(TimeDuration.AllTime, OrderStatus.All, form);
                }
                catch (ApiException ex)
                {
                    Log.Error(ex.Message);
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No user selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static async void ProcessOrder(Form form)
        {
            var dgv = form.Controls.Find("_dgv", true).FirstOrDefault() as DataGridView;
            if (dgv.CurrentRow != null && dgv.CurrentRow.DataBoundItem is OrderDto order)
            {
                try
                {
                    if (order.Status != OrderStatus.Pending.ToString() || order.Status == OrderStatus.Cancelled.ToString() || order.Status == OrderStatus.Delivered.ToString())
                    {
                        MessageBox.Show("Order must be pending before it can be processed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    await ApiManger.Instance.OrderService.ProcessOrderAsync(order.Id);
                    MessageBox.Show("Order processed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadAllOrders(TimeDuration.AllTime, OrderStatus.All, form);
                }
                catch (ApiException ex)
                {
                    Log.Error(ex.Message);
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No user selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static void AddFilters(TimeDuration duration, OrderStatus status, Form form)
        {
            ComboBox durationComboBox = new ComboBox
            {
                Name = "_durationComboBox",
                Location = new Point(300, 80),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            durationComboBox.SelectedIndexChanged += FilterChanged;
            durationComboBox.DataSource = Enum.GetValues(typeof(TimeDuration));
            form.Controls.Add(durationComboBox);
            durationComboBox.SelectedItem = duration;

            ComboBox statusComboBox = new ComboBox
            {
                Name = "_statusComboBox",
                Location = new Point(500, 80),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            statusComboBox.SelectedIndexChanged += FilterChanged;
            statusComboBox.DataSource = Enum.GetValues(typeof(OrderStatus));
            form.Controls.Add(statusComboBox);
            statusComboBox.SelectedItem = status;
        }

        private static async void FilterChanged(object sender, EventArgs e)
        {
            // Find the parent form
            Control control = sender as Control;
            Form form = control?.FindForm();
            if (form == null)
                return;

            // Get selected values from both ComboBoxes
            var durationComboBox = form.Controls.Find("_durationComboBox", true).FirstOrDefault() as ComboBox;
            var statusComboBox = form.Controls.Find("_statusComboBox", true).FirstOrDefault() as ComboBox;

            if (durationComboBox == null || statusComboBox == null)
                return;

            if (!(durationComboBox.SelectedItem is TimeDuration selectedDuration) ||
                !(statusComboBox.SelectedItem is OrderStatus selectedStatus))
                return;

            // Reload orders with new filters
            await LoadAllOrders(selectedDuration, selectedStatus, form);
        }


    }
}
