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

namespace Shopping_App.ViewData
{
    internal class Products
    {
        private static PictureBox RightArrow;
        private static Label PageNumber;
        private static PictureBox LeftArrow;
        private static int _page = 1;

        public static async Task LoadProducts(int Page, int PageSize, Form form, string SearchTerm = null, string SortColumn = null, string SortOrder = null)
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

            AddProductsToForm(ProductsPage.Items, form);
            AddArrowsAndPageNumber(form);

            RightArrow.Enabled = ProductsPage.HasNextPage;
            LeftArrow.Enabled = ProductsPage.HasPreviousPage;
            PageNumber.Text = ProductsPage.Page.ToString();
        }
        private static void AddProductsToForm(List<ProductDto> products, Form form)
        {
            // Clear existing controls
            Hellpers.ClearForm(form);

            //Create new controls for each product

           int i = 0;
            foreach (ProductDto product in products)
            {
                int QuentityInCart = Carts.GetProductQuentityInCart(product.Id);
                product.Quantity = QuentityInCart;

                ProductControl productControl = new ProductControl(product);
                productControl.Location = new Point(230 * (i % 3) + 5, 160 * (i / 3) + 30);
                productControl.ProductStatusChanged += ProductControl_ProductStatusChanged;
                form.Controls.Add(productControl);
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

        private static void AddArrowsAndPageNumber(Form form)
        {
            LeftArrow = new PictureBox();
            LeftArrow.Image = Image.FromFile("Images/arrow_left.png");
            LeftArrow.SizeMode = PictureBoxSizeMode.StretchImage;
            LeftArrow.Size = new Size(24, 24);
            LeftArrow.Location = new Point(300, 660);
            LeftArrow.Click += (s, e) => { LeftArrowClicked(form); };
            form.Controls.Add(LeftArrow);
            LeftArrow.BringToFront();

            PageNumber = new Label();
            PageNumber.Text = "1";
            PageNumber.Font = new Font("Arial", 12, FontStyle.Bold);
            Size size = TextRenderer.MeasureText(PageNumber.Text, PageNumber.Font);
            int offsite = (52 - size.Width) / 2;
            PageNumber.Location = new Point(324 + offsite, 660);
            PageNumber.AutoSize = true;
            form.Controls.Add(PageNumber);

            RightArrow = new PictureBox();
            RightArrow.Image = Image.FromFile("Images/arrow_right.png");
            RightArrow.SizeMode = PictureBoxSizeMode.StretchImage;
            RightArrow.Size = new Size(24, 24);
            RightArrow.Location = new Point(400 - 24, 660);
            RightArrow.Click += (s, e) => { RightArrowClicked(form); };
            form.Controls.Add(RightArrow);
            RightArrow.BringToFront();
        }

        private static async void LeftArrowClicked(Form form)
        {
            await LoadProducts(_page - 1, 12, form);
        }

        private static async void RightArrowClicked(Form form)
        {
            await LoadProducts( _page + 1, 12,  form);
        }


    }
}
