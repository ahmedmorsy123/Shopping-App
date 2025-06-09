using Serilog;
using Shopping_App.Forms;
using Shopping_App.User_Controls;
using ShoppingApp.Api;
using ShoppingApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ShoppingAppDB.Enums.Enums;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Shopping_App.ViewData
{
    internal class Products
    {
        private static PictureBox RightArrow;
        private static Label PageNumber;
        private static PictureBox LeftArrow;
        private static int _page = 1;
        private static Form _form;

        public static void SetForm(Form form)
        {
            _form = form;
        }
        public static async Task LoadProducts(int Page, int PageSize, string SearchTerm = null, string SortColumn = null, string SortOrder = null)
        {
            _page = Page;
            // Get products from API
            PagedList<ProductDto> ProductsPage;
            try
            {
                ProductsPage = await ApiManger.Instance.ProductService.GetProductsAsync(SearchTerm, SortColumn, SortOrder, Page, PageSize);

            }
            catch (ApiException ex)
            {
                Log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AddProductsToForm(ProductsPage.Items);
            AddArrowsAndPageNumber();

            RightArrow.Enabled = ProductsPage.HasNextPage;
            LeftArrow.Enabled = ProductsPage.HasPreviousPage;
            PageNumber.Text = ProductsPage.Page.ToString();
        }

        public static async Task LoadLowStockProducts(Form form)
        {
            List<ProductDto> products;
            try
            {
                products = await ApiManger.Instance.ProductService.GetLowStockProducts();
            }
            catch (ApiException ex)
            {
                Log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var title = form.Controls.Find("_title", true).FirstOrDefault() as Label;
            title.Text = "Low Stock Products";

            var dgv = form.Controls.Find("_dgv", true).FirstOrDefault() as DataGridView;
            if (dgv == null)
            {
                MessageBox.Show("DataGridView '_dgv' not found on the form.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgv.DataSource = products;
            dgv.Columns["Quantity"].Visible = false; // Hide max quantity column
            dgv.Columns["maxQuantity"].HeaderText = "Current Stock"; // Rename Quantity column

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripItem stockProduct = new ToolStripMenuItem("Stock Product", null, async (s, e) => { 
                StockProduct(form);
                await LoadLowStockProducts(form);
            });
            contextMenu.Items.Add(stockProduct);
            dgv.ContextMenuStrip = contextMenu;
        }

        private static void StockProduct(Form form)
        {
            var dgv = form.Controls.Find("_dgv", true).FirstOrDefault() as DataGridView;
            if (dgv.CurrentRow != null && dgv.CurrentRow.DataBoundItem is ProductDto product)
            {
                try
                {
                    StockProductForm stockProductForm = new StockProductForm(product.Id, product.maxQuantity);
                    stockProductForm.ShowDialog();
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

        public static async Task GetOutOfStockProducts(Form form)
        {
            List<ProductDto> products;
            try
            {
                products = await ApiManger.Instance.ProductService.GetOutOfStockProducts();
            }
            catch (ApiException ex)
            {
                Log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var title = form.Controls.Find("_title", true).FirstOrDefault() as Label;
            title.Text = "Out of Stock Products";

            var dgv = form.Controls.Find("_dgv", true).FirstOrDefault() as DataGridView;
            if (dgv == null)
            {
                MessageBox.Show("DataGridView '_dgv' not found on the form.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgv.DataSource = products;

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripItem stockProduct = new ToolStripMenuItem("Stock Product", null, async (s, e) => {
                StockProduct(form);
                await GetOutOfStockProducts(form);
            });
            contextMenu.Items.Add(stockProduct);
            dgv.ContextMenuStrip = contextMenu;

        }

        private static void AddProductsToForm(List<ProductDto> products)
        {
            // Clear existing controls
            HellpersMethodes.ClearForm(_form);

            //Create new controls for each product

           int i = 0;
            foreach (ProductDto product in products)
            {
                int QuentityInCart = Carts.GetProductQuentityInCart(product.Id);
                product.Quantity = QuentityInCart;

                ProductControl productControl = new ProductControl(product);
                productControl.Location = new Point(230 * (i % 3) + 5, 160 * (i / 3) + 30);
                productControl.ProductStatusChanged += ProductControl_ProductStatusChanged;
                _form.Controls.Add(productControl);
                i++;
            }

        }

        private static void ProductControl_ProductStatusChanged(bool IsInCart, ProductDto product)
        {
            if (IsInCart)
            {
                Carts.AddUpdateCart(product);
            }
            else
            {
                Carts.RemoveFromCart(product);
            }
        }

        private static void AddArrowsAndPageNumber()
        {
            LeftArrow = new PictureBox();
            LeftArrow.Image = Image.FromFile("Images/arrow_left.png");
            LeftArrow.SizeMode = PictureBoxSizeMode.StretchImage;
            LeftArrow.Size = new Size(24, 24);
            LeftArrow.Location = new Point(300, 660);
            LeftArrow.Click += (s, e) => { LeftArrowClicked(); };
            _form.Controls.Add(LeftArrow);
            LeftArrow.BringToFront();

            PageNumber = new Label();
            PageNumber.Text = "1";
            PageNumber.Font = new Font("Arial", 12, FontStyle.Bold);
            Size size = TextRenderer.MeasureText(PageNumber.Text, PageNumber.Font);
            int offsite = (52 - size.Width) / 2;
            PageNumber.Location = new Point(324 + offsite, 660);
            PageNumber.AutoSize = true;
            _form.Controls.Add(PageNumber);

            RightArrow = new PictureBox();
            RightArrow.Image = Image.FromFile("Images/arrow_right.png");
            RightArrow.SizeMode = PictureBoxSizeMode.StretchImage;
            RightArrow.Size = new Size(24, 24);
            RightArrow.Location = new Point(400 - 24, 660);
            RightArrow.Click += (s, e) => { RightArrowClicked(); };
            _form.Controls.Add(RightArrow);
            RightArrow.BringToFront();
        }

        private static async void LeftArrowClicked()
        {
            await LoadProducts(_page - 1, 12);
        }

        private static async void RightArrowClicked()
        {
            await LoadProducts( _page + 1, 12);
        }


    }
}
