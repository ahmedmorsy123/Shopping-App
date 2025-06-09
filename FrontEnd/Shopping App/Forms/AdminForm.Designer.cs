namespace Shopping_App.Forms
{
    partial class AdminForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStripCurrentUser = new System.Windows.Forms.MenuStrip();
            this.currentUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.productsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowStockProductsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outOfStockProductsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myCartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getCartsCountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listOrdersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listUsersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listAdminsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registerationCountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logInCountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripCurrentUser.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripCurrentUser
            // 
            this.menuStripCurrentUser.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStripCurrentUser.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripCurrentUser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.currentUserToolStripMenuItem});
            this.menuStripCurrentUser.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.menuStripCurrentUser.Location = new System.Drawing.Point(0, 0);
            this.menuStripCurrentUser.Name = "menuStripCurrentUser";
            this.menuStripCurrentUser.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStripCurrentUser.Size = new System.Drawing.Size(110, 28);
            this.menuStripCurrentUser.TabIndex = 3;
            this.menuStripCurrentUser.Text = "menuStrip1";
            // 
            // currentUserToolStripMenuItem
            // 
            this.currentUserToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateProfileToolStripMenuItem,
            this.logOutToolStripMenuItem});
            this.currentUserToolStripMenuItem.Name = "currentUserToolStripMenuItem";
            this.currentUserToolStripMenuItem.Size = new System.Drawing.Size(104, 24);
            this.currentUserToolStripMenuItem.Text = "Current User";
            this.currentUserToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // updateProfileToolStripMenuItem
            // 
            this.updateProfileToolStripMenuItem.Name = "updateProfileToolStripMenuItem";
            this.updateProfileToolStripMenuItem.Size = new System.Drawing.Size(188, 26);
            this.updateProfileToolStripMenuItem.Text = "Update Profile";
            this.updateProfileToolStripMenuItem.Click += new System.EventHandler(this.updateProfileToolStripMenuItem_Click);
            // 
            // logOutToolStripMenuItem
            // 
            this.logOutToolStripMenuItem.Name = "logOutToolStripMenuItem";
            this.logOutToolStripMenuItem.Size = new System.Drawing.Size(188, 26);
            this.logOutToolStripMenuItem.Text = "Log Out";
            this.logOutToolStripMenuItem.Click += new System.EventHandler(this.logOutToolStripMenuItem_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.productsToolStripMenuItem,
            this.myCartToolStripMenuItem,
            this.myOrderToolStripMenuItem,
            this.usersToolStripMenuItem});
            this.menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStrip.Size = new System.Drawing.Size(800, 28);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // productsToolStripMenuItem
            // 
            this.productsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lowStockProductsToolStripMenuItem,
            this.outOfStockProductsToolStripMenuItem});
            this.productsToolStripMenuItem.Name = "productsToolStripMenuItem";
            this.productsToolStripMenuItem.Size = new System.Drawing.Size(80, 24);
            this.productsToolStripMenuItem.Text = "Products";
            // 
            // lowStockProductsToolStripMenuItem
            // 
            this.lowStockProductsToolStripMenuItem.Name = "lowStockProductsToolStripMenuItem";
            this.lowStockProductsToolStripMenuItem.Size = new System.Drawing.Size(235, 26);
            this.lowStockProductsToolStripMenuItem.Text = "Low Stock Products";
            this.lowStockProductsToolStripMenuItem.Click += new System.EventHandler(this.lowStockProductsToolStripMenuItem_Click);
            // 
            // outOfStockProductsToolStripMenuItem
            // 
            this.outOfStockProductsToolStripMenuItem.Name = "outOfStockProductsToolStripMenuItem";
            this.outOfStockProductsToolStripMenuItem.Size = new System.Drawing.Size(235, 26);
            this.outOfStockProductsToolStripMenuItem.Text = "Out of Stock Products";
            this.outOfStockProductsToolStripMenuItem.Click += new System.EventHandler(this.outOfStockProductsToolStripMenuItem_Click);
            // 
            // myCartToolStripMenuItem
            // 
            this.myCartToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getCartsCountToolStripMenuItem});
            this.myCartToolStripMenuItem.Name = "myCartToolStripMenuItem";
            this.myCartToolStripMenuItem.Size = new System.Drawing.Size(56, 24);
            this.myCartToolStripMenuItem.Text = "Carts";
            // 
            // getCartsCountToolStripMenuItem
            // 
            this.getCartsCountToolStripMenuItem.Name = "getCartsCountToolStripMenuItem";
            this.getCartsCountToolStripMenuItem.Size = new System.Drawing.Size(195, 26);
            this.getCartsCountToolStripMenuItem.Text = "Get Carts Count";
            this.getCartsCountToolStripMenuItem.Click += new System.EventHandler(this.getCartsCountToolStripMenuItem_Click);
            // 
            // myOrderToolStripMenuItem
            // 
            this.myOrderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.listOrdersToolStripMenuItem});
            this.myOrderToolStripMenuItem.Name = "myOrderToolStripMenuItem";
            this.myOrderToolStripMenuItem.Size = new System.Drawing.Size(67, 24);
            this.myOrderToolStripMenuItem.Text = "Orders";
            // 
            // listOrdersToolStripMenuItem
            // 
            this.listOrdersToolStripMenuItem.Name = "listOrdersToolStripMenuItem";
            this.listOrdersToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.listOrdersToolStripMenuItem.Text = "List Orders";
            this.listOrdersToolStripMenuItem.Click += new System.EventHandler(this.listOrdersToolStripMenuItem_Click);
            // 
            // usersToolStripMenuItem
            // 
            this.usersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.listUsersToolStripMenuItem,
            this.listAdminsToolStripMenuItem,
            this.registerationCountToolStripMenuItem,
            this.logInCountToolStripMenuItem});
            this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(58, 24);
            this.usersToolStripMenuItem.Text = "Users";
            // 
            // listUsersToolStripMenuItem
            // 
            this.listUsersToolStripMenuItem.Name = "listUsersToolStripMenuItem";
            this.listUsersToolStripMenuItem.Size = new System.Drawing.Size(223, 26);
            this.listUsersToolStripMenuItem.Text = "List Users";
            this.listUsersToolStripMenuItem.Click += new System.EventHandler(this.listUsersToolStripMenuItem_Click);
            // 
            // listAdminsToolStripMenuItem
            // 
            this.listAdminsToolStripMenuItem.Name = "listAdminsToolStripMenuItem";
            this.listAdminsToolStripMenuItem.Size = new System.Drawing.Size(223, 26);
            this.listAdminsToolStripMenuItem.Text = "List Admins";
            this.listAdminsToolStripMenuItem.Click += new System.EventHandler(this.listAdminsToolStripMenuItem_Click);
            // 
            // registerationCountToolStripMenuItem
            // 
            this.registerationCountToolStripMenuItem.Name = "registerationCountToolStripMenuItem";
            this.registerationCountToolStripMenuItem.Size = new System.Drawing.Size(223, 26);
            this.registerationCountToolStripMenuItem.Text = "Registeration Count";
            this.registerationCountToolStripMenuItem.Click += new System.EventHandler(this.registerationCountToolStripMenuItem_Click);
            // 
            // logInCountToolStripMenuItem
            // 
            this.logInCountToolStripMenuItem.Name = "logInCountToolStripMenuItem";
            this.logInCountToolStripMenuItem.Size = new System.Drawing.Size(223, 26);
            this.logInCountToolStripMenuItem.Text = "LogIn Count";
            this.logInCountToolStripMenuItem.Click += new System.EventHandler(this.logInCountToolStripMenuItem_Click);
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 685);
            this.Controls.Add(this.menuStripCurrentUser);
            this.Controls.Add(this.menuStrip);
            this.KeyPreview = true;
            this.Name = "AdminForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AdminForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AdminForm_FormClosing);
            this.Load += new System.EventHandler(this.AdminForm_Load);
            this.menuStripCurrentUser.ResumeLayout(false);
            this.menuStripCurrentUser.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripCurrentUser;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem productsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem myCartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem myOrderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem currentUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lowStockProductsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outOfStockProductsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listUsersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listAdminsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getCartsCountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listOrdersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registerationCountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logInCountToolStripMenuItem;
    }
}