using Shopping_App.Forms;
using ShoppingApp.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shopping_App.ViewData
{
    internal class Orders
    {
        public static void Checkout(object sender, EventArgs e)
        {
            ShippingInfoForm shippingInfoForm = new ShippingInfoForm();
            shippingInfoForm.CheckOutConfirmed += async (string shippingAddress, string paymentMethod) =>
            {
                try
                {
                    await ApiManger.Instance.OrderService.MakeOrderAsync(ApiManger.CurrentLoggedInUser.Id, shippingAddress, paymentMethod);
                }
                catch (ApiException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            };
            shippingInfoForm.ShowDialog();
        }
    }
}
