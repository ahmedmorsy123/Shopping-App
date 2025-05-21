using Serilog;
using Shopping_App.User_Controls;
using ShoppingApp.Api;
using ShoppingApp.Api.Controllers;
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
    public partial class MainForm : Form
    {
        // Products
        private int _page = 1;
        private bool _hasNextPage = true;
        private bool _hasPreviousPage = false;
        PictureBox RightArrow;
        Label PageNumber;
        PictureBox LeftArrow;

        // Carts
        private List<(ProductDto Product, int MaxQuentity)> CartProducts = new List<(ProductDto, int)>();

        public MainForm()
        {
            InitializeComponent();

            currentUserToolStripMenuItem.Text = ApiManger.CurrentLoggedInUser.Name;

            this.Text = "Shopping App";
            this.Size = new Size(715, 730);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.SizeGripStyle = SizeGripStyle.Hide;

            productsToolStripMenuItem_Click(null, null);

        }



        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            await LogOut();
        }

        private async void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await LoadProducts(PageSize: 12, Page: 1);
        }

        private async Task LoadProducts(int Page, int PageSize, string SearchTerm = null, string SortColumn = null, string SortOrder = null)
        {
            PagedList<ProductDto> products;
            try
            {
                products = await ApiManger.Instance.ProductService.GetProductsAsync(SearchTerm, SortColumn, SortOrder, Page, PageSize);
            }
            catch (ApiException ex)
            {
                Log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Controls.OfType<Control>().ToList().ForEach(c =>
            {
                if (c.GetType() != typeof(MenuStrip))
                {
                    this.Controls.Remove(c);
                }
            });

            int i = 0;
            foreach (ProductDto product in products.Items)
            {
                var ProductTupleInCart = CartProducts.FirstOrDefault(p => p.Product.Id == product.Id);

                ProductControl productControl = new ProductControl(product, ProductTupleInCart.Product?.Quantity);
                productControl.Location = new Point(230 * (i % 3) + 5, 160 * (i / 3) + 30);
                productControl.ProductStatusChanged += ProductControl_ProductStatusChanged;
                this.Controls.Add(productControl);
                i++;
            }

            AddArrowsAndPageNumber();

            _hasNextPage = products.HasNextPage;
            _hasPreviousPage = products.HasPreviousPage;
            _page = products.Page;

            RightArrow.Enabled = _hasNextPage;
            LeftArrow.Enabled = _hasPreviousPage;
            PageNumber.Text = _page.ToString();
        }

        private void ProductControl_ProductStatusChanged(bool IsInCart, ProductDto product, int MaxQuentity)
        {
            if (IsInCart)
            {
                var existingProductTuple = CartProducts.FirstOrDefault(p => p.Product.Id == product.Id);
                if(existingProductTuple.Product != null)
                {
                    existingProductTuple.Product.Quantity = product.Quantity;
                }
                else
                {
                    CartProducts.Add((product, MaxQuentity));
                }
            }
            else
            {
                CartProducts.Remove(CartProducts.First(p => p.Product.Id == product.Id));
            }

        }

        private void myCartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Controls.OfType<Control>().ToList().ForEach(c =>
            {
                if(c.GetType() != typeof(MenuStrip))
                {
                    this.Controls.Remove(c);
                }
            });

            CartItem cartItem = new CartItem();
            cartItem.Location = new Point(10, 40 + 60 * 0);
            this.Controls.Add(cartItem);
            cartItem.BringToFront();

            CartItem cart = new CartItem();
            cart.Location = new Point(10, 40 + 60 * 1);
            this.Controls.Add(cart);
            cart.BringToFront();

            CartItem cartItem1 = new CartItem();
            cartItem1.Location = new Point(10, 40 + 60 * 2);
            this.Controls.Add(cartItem1);
            cartItem1.BringToFront();

        }

        private void myOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void updateProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateProfileForm updateProfileForm = new UpdateProfileForm();
            updateProfileForm.ShowDialog();
        }

        private async void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await LogOut();
            this.Close();
        }

        private async Task LogOut()
        {
            try
            {
                await ApiManger.Instance.AuthService.LogoutAsync();
            }
            catch (ApiException ex)
            {
                Log.Error($"Logout Faild {ex.Message}");
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void AddArrowsAndPageNumber()
        {
            LeftArrow = new PictureBox();
            LeftArrow.Image = Image.FromFile("Images/arrow_left.png");
            LeftArrow.SizeMode = PictureBoxSizeMode.StretchImage;
            LeftArrow.Size = new Size(24, 24);
            LeftArrow.Location = new Point(300, 660);
            LeftArrow.Click += (s, e) => { LeftArrowClicked(); };
            this.Controls.Add(LeftArrow);
            LeftArrow.BringToFront();

            PageNumber = new Label();
            PageNumber.Text = "1";
            PageNumber.Font = new Font("Arial", 12, FontStyle.Bold);
            Size size = TextRenderer.MeasureText(PageNumber.Text, PageNumber.Font);
            int offsite = (52 - size.Width) / 2;
            PageNumber.Location = new Point(324 + offsite, 660);
            PageNumber.AutoSize = true;
            this.Controls.Add(PageNumber);

            RightArrow = new PictureBox();
            RightArrow.Image = Image.FromFile("Images/arrow_right.png");
            RightArrow.SizeMode = PictureBoxSizeMode.StretchImage;
            RightArrow.Size = new Size(24, 24);
            RightArrow.Location = new Point(400 - 24, 660);
            RightArrow.Click += (s, e) => { RightArrowClicked(); };
            this.Controls.Add(RightArrow);
            RightArrow.BringToFront();
            this.Refresh();
        }

        private async void LeftArrowClicked()
        {
            await LoadProducts(PageSize: 12, Page: _page - 1);
        }

        private async void RightArrowClicked()
        {
            await LoadProducts(PageSize: 12, Page: _page + 1);
        }


    }
}
