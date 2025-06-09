using ShoppingAppDB.Models;
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
    public partial class StockProductForm : Form
    {
        private int prevQuentity;

        private int Id;
        public StockProductForm(int id, int quentity = 0)
        {
            InitializeComponent();

            lbID.Text = id.ToString();
            Id = id;
            Quentity.Value = quentity;
            prevQuentity = quentity;
        }

        private async void btnStock_Click(object sender, EventArgs e)
        {
            if (Quentity.Value < prevQuentity)
            {
                MessageBox.Show("You can't reduce the stock quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Correctly initialize the UpdateProductStockRequest object using property assignments  
                var stockRequest = new UpdateProductStockRequest
                {
                    ProductId = Id,
                    Quentity = (int)Quentity.Value
                };

                await ApiManger.Instance.ProductService.UpdateProductStock(stockRequest);

                MessageBox.Show("Stock updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
