using Shopping_App.Forms;
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

namespace Shopping_App.ViewData
{
    internal class Users
    {
        private static Form _form;
        private static Label _title;
        private static DataGridView _dgv;

        public static void SetForm(Form form)
        {
            _form = form;
        }

        public static void AddTitle(string titleText)
        {
            _title = new Label();
            _title.Text = titleText;
            _title.Font = new Font("Arial", 16, FontStyle.Bold);
            _title.Location = new Point(5, 50);
            _title.AutoSize = true;
            _title.Name = "_title";
            _form.Controls.Add(_title);
        }
        public static void AddDGV()
        {
            _dgv = new DataGridView();
            _dgv.Location = new Point(5, 120);
            _dgv.Size = new Size(690, 550);
            _dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Ensure columns fill the control
            _dgv.Name = "_dgv";

            // Ensure right-click selects the row under the mouse
            _dgv.MouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    var hit = _dgv.HitTest(e.X, e.Y);
                    if (hit.Type == DataGridViewHitTestType.Cell && hit.RowIndex >= 0)
                    {
                        _dgv.ClearSelection();
                        _dgv.Rows[hit.RowIndex].Selected = true;
                        _dgv.CurrentCell = _dgv.Rows[hit.RowIndex].Cells[hit.ColumnIndex];
                    }
                }
            };

            _form.Controls.Add(_dgv);
        }

        public static async Task LoadUsers()
        {
            RemoveFilters();
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem makeAdminItem = new ToolStripMenuItem("Make Admin", null, makeAdmin);
            contextMenu.Items.Add(makeAdminItem);
            _dgv.ContextMenuStrip = contextMenu;

            _title.Text = "Users List";

            try
            {
                // Fetch users from API
                var users = await ApiManger.Instance.UserService.GetAllUsers();
                // Set DataGridView data source
                _dgv.DataSource = users;
                _dgv.Columns["Password"].Visible = false; // Hide password column
                // Update total count label
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static async void makeAdmin(object sender, EventArgs e)
        {
            if (_dgv.CurrentRow != null && _dgv.CurrentRow.DataBoundItem is UserDto user)
            {
                try
                {
                    await ApiManger.Instance.AdminService.MakeAdminAsync(user.Id);
                    MessageBox.Show("User made admin successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (ApiException ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No user selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            await LoadUsers();
        }

        public static async Task LoadAdmins()
        {
            RemoveFilters();
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem addAdminItem = new ToolStripMenuItem("Add Admin", null, addAdmin);
            ToolStripMenuItem removeAdminItem = new ToolStripMenuItem("Remove Admin", null, removeAdmin);
            contextMenu.Items.AddRange(new ToolStripItem[] { addAdminItem, removeAdminItem });
            _dgv.ContextMenuStrip = contextMenu;

            _title.Text = "Admins List";

            try
            {
                // Fetch admins from API
                var admins = await ApiManger.Instance.AdminService.ListAdminsAsync();
                // Set DataGridView data source
                _dgv.DataSource = admins;
                _dgv.Columns["Password"].Visible = false; // Hide password column

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading admins: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static async void removeAdmin(object sender, EventArgs e)
        {

            if (_dgv.CurrentRow != null && _dgv.CurrentRow.DataBoundItem is UserDto user)
            {
                try
                {
                    bool result = await ApiManger.Instance.AdminService.RemoveAdminAsync(user.Id);
                    if (!result)
                    {
                        MessageBox.Show("Failed to remove admin. admin Id not found or it's not an admin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show("User removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (ApiException ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No user selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            await LoadAdmins();
        }

        private static async void addAdmin(object sender, EventArgs e)
        {
            AddUserForm addUserForm = new AddUserForm();
            addUserForm.ShowDialog();
            await LoadAdmins();
        }

        public static async Task<int> GetRegisterationCount(TimeDuration duratoin)
        {
            try
            {
                // Fetch registration count from API
                var count = await ApiManger.Instance.AuthService.GetRegisterationCountByDurationAsync(duratoin);
                return count;
            }
            catch (ApiException ex)
            {
                MessageBox.Show($"Error fetching registration count: {ex.Message} \n status code: {ex.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        public static async Task<int> GetLogInCount(TimeDuration duration)
        {
            try
            {
                // Fetch login count from API
                var count = await ApiManger.Instance.AuthService.GetLoginCountByDurationAsync(duration);
                return count;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching login count: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        private static void RemoveFilters()
        {
            var controls = _form.Controls.OfType<ComboBox>();
            foreach(var control in controls)
            {
                _form.Controls.Remove(control);
            }
        }
    }
}
