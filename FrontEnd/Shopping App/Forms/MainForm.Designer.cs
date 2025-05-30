namespace Shopping_App.Forms
{
    partial class MainForm
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.productsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myCartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripCurrentUser = new System.Windows.Forms.MenuStrip();
            this.currentUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.menuStripCurrentUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.productsToolStripMenuItem,
            this.myCartToolStripMenuItem,
            this.myOrderToolStripMenuItem});
            this.menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStrip.Size = new System.Drawing.Size(697, 28);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // productsToolStripMenuItem
            // 
            this.productsToolStripMenuItem.Name = "productsToolStripMenuItem";
            this.productsToolStripMenuItem.Size = new System.Drawing.Size(80, 24);
            this.productsToolStripMenuItem.Text = "Products";
            this.productsToolStripMenuItem.Click += new System.EventHandler(this.productsToolStripMenuItem_Click);
            // 
            // myCartToolStripMenuItem
            // 
            this.myCartToolStripMenuItem.Name = "myCartToolStripMenuItem";
            this.myCartToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.myCartToolStripMenuItem.Text = "My Cart";
            this.myCartToolStripMenuItem.Click += new System.EventHandler(this.myCartToolStripMenuItem_Click);
            // 
            // myOrderToolStripMenuItem
            // 
            this.myOrderToolStripMenuItem.Name = "myOrderToolStripMenuItem";
            this.myOrderToolStripMenuItem.Size = new System.Drawing.Size(91, 24);
            this.myOrderToolStripMenuItem.Text = "My Orders";
            this.myOrderToolStripMenuItem.Click += new System.EventHandler(this.myOrderToolStripMenuItem_Click);
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
            this.menuStripCurrentUser.Size = new System.Drawing.Size(260, 28);
            this.menuStripCurrentUser.TabIndex = 1;
            this.menuStripCurrentUser.Text = "menuStrip1";
            // 
            // currentUserToolStripMenuItem
            // 
            this.currentUserToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateProfileToolStripMenuItem,
            this.logOutToolStripMenuItem,
            this.deleteAccountToolStripMenuItem});
            this.currentUserToolStripMenuItem.Name = "currentUserToolStripMenuItem";
            this.currentUserToolStripMenuItem.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.currentUserToolStripMenuItem.Size = new System.Drawing.Size(104, 24);
            this.currentUserToolStripMenuItem.Text = "Current User";
            // 
            // updateProfileToolStripMenuItem
            // 
            this.updateProfileToolStripMenuItem.Name = "updateProfileToolStripMenuItem";
            this.updateProfileToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.updateProfileToolStripMenuItem.Text = "Update Profile";
            this.updateProfileToolStripMenuItem.Click += new System.EventHandler(this.updateProfileToolStripMenuItem_Click);
            // 
            // logOutToolStripMenuItem
            // 
            this.logOutToolStripMenuItem.Name = "logOutToolStripMenuItem";
            this.logOutToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.logOutToolStripMenuItem.Text = "Log Out";
            this.logOutToolStripMenuItem.Click += new System.EventHandler(this.logOutToolStripMenuItem_Click);
            // 
            // deleteAccountToolStripMenuItem
            // 
            this.deleteAccountToolStripMenuItem.Name = "deleteAccountToolStripMenuItem";
            this.deleteAccountToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.deleteAccountToolStripMenuItem.Text = "Delete Account";
            this.deleteAccountToolStripMenuItem.Click += new System.EventHandler(this.deleteAccountToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 683);
            this.Controls.Add(this.menuStripCurrentUser);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.menuStripCurrentUser.ResumeLayout(false);
            this.menuStripCurrentUser.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem productsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem myCartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem myOrderToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStripCurrentUser;
        private System.Windows.Forms.ToolStripMenuItem currentUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAccountToolStripMenuItem;
    }
}