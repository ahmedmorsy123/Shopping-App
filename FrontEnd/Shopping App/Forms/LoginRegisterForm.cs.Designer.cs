namespace Shopping_App.Forms
{
    partial class LoginRegisterForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabLogin = new System.Windows.Forms.TabPage();
            this.cbShowPasswordLogin = new System.Windows.Forms.CheckBox();
            this.Login = new System.Windows.Forms.Button();
            this.txtPasswordLogin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClearLogin = new System.Windows.Forms.Button();
            this.txtUserNameLogin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabRegister = new System.Windows.Forms.TabPage();
            this.cbShowPasswordRegister = new System.Windows.Forms.CheckBox();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnRegister = new System.Windows.Forms.Button();
            this.txtPasswordRegister = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClearRegister = new System.Windows.Forms.Button();
            this.txtUserNameRegister = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabLogin.SuspendLayout();
            this.tabRegister.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabLogin);
            this.tabControl.Controls.Add(this.tabRegister);
            this.tabControl.ItemSize = new System.Drawing.Size(48, 21);
            this.tabControl.Location = new System.Drawing.Point(0, 2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(380, 383);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabLogin
            // 
            this.tabLogin.Controls.Add(this.cbShowPasswordLogin);
            this.tabLogin.Controls.Add(this.Login);
            this.tabLogin.Controls.Add(this.txtPasswordLogin);
            this.tabLogin.Controls.Add(this.label2);
            this.tabLogin.Controls.Add(this.btnClearLogin);
            this.tabLogin.Controls.Add(this.txtUserNameLogin);
            this.tabLogin.Controls.Add(this.label1);
            this.tabLogin.Location = new System.Drawing.Point(4, 25);
            this.tabLogin.Name = "tabLogin";
            this.tabLogin.Padding = new System.Windows.Forms.Padding(3);
            this.tabLogin.Size = new System.Drawing.Size(372, 354);
            this.tabLogin.TabIndex = 0;
            this.tabLogin.Text = "Login";
            this.tabLogin.UseVisualStyleBackColor = true;
            // 
            // cbShowPasswordLogin
            // 
            this.cbShowPasswordLogin.AutoSize = true;
            this.cbShowPasswordLogin.Location = new System.Drawing.Point(181, 127);
            this.cbShowPasswordLogin.Name = "cbShowPasswordLogin";
            this.cbShowPasswordLogin.Size = new System.Drawing.Size(126, 21);
            this.cbShowPasswordLogin.TabIndex = 15;
            this.cbShowPasswordLogin.Text = "Show Password";
            this.cbShowPasswordLogin.UseVisualStyleBackColor = true;
            this.cbShowPasswordLogin.CheckedChanged += new System.EventHandler(this.cbShowPasswordLogin_CheckedChanged);
            // 
            // Login
            // 
            this.Login.Location = new System.Drawing.Point(151, 154);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(75, 23);
            this.Login.TabIndex = 3;
            this.Login.Text = "Login";
            this.Login.UseVisualStyleBackColor = true;
            this.Login.Click += new System.EventHandler(this.Login_Click);
            // 
            // txtPasswordLogin
            // 
            this.txtPasswordLogin.Location = new System.Drawing.Point(152, 86);
            this.txtPasswordLogin.Name = "txtPasswordLogin";
            this.txtPasswordLogin.Size = new System.Drawing.Size(155, 24);
            this.txtPasswordLogin.TabIndex = 2;
            this.txtPasswordLogin.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label2.Location = new System.Drawing.Point(25, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password: ";
            // 
            // btnClearLogin
            // 
            this.btnClearLogin.Location = new System.Drawing.Point(232, 154);
            this.btnClearLogin.Name = "btnClearLogin";
            this.btnClearLogin.Size = new System.Drawing.Size(75, 23);
            this.btnClearLogin.TabIndex = 5;
            this.btnClearLogin.Text = "Clear";
            this.btnClearLogin.UseVisualStyleBackColor = true;
            this.btnClearLogin.Click += new System.EventHandler(this.btnClearLogin_Click);
            // 
            // txtUserNameLogin
            // 
            this.txtUserNameLogin.Location = new System.Drawing.Point(152, 48);
            this.txtUserNameLogin.Name = "txtUserNameLogin";
            this.txtUserNameLogin.Size = new System.Drawing.Size(155, 24);
            this.txtUserNameLogin.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label1.Location = new System.Drawing.Point(25, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "User Name: ";
            // 
            // tabRegister
            // 
            this.tabRegister.Controls.Add(this.cbShowPasswordRegister);
            this.tabRegister.Controls.Add(this.txtConfirmPassword);
            this.tabRegister.Controls.Add(this.label6);
            this.tabRegister.Controls.Add(this.txtEmail);
            this.tabRegister.Controls.Add(this.label5);
            this.tabRegister.Controls.Add(this.btnRegister);
            this.tabRegister.Controls.Add(this.txtPasswordRegister);
            this.tabRegister.Controls.Add(this.label3);
            this.tabRegister.Controls.Add(this.btnClearRegister);
            this.tabRegister.Controls.Add(this.txtUserNameRegister);
            this.tabRegister.Controls.Add(this.label4);
            this.tabRegister.Location = new System.Drawing.Point(4, 25);
            this.tabRegister.Name = "tabRegister";
            this.tabRegister.Size = new System.Drawing.Size(372, 354);
            this.tabRegister.TabIndex = 1;
            this.tabRegister.Text = "Register";
            this.tabRegister.UseVisualStyleBackColor = true;
            // 
            // cbShowPasswordRegister
            // 
            this.cbShowPasswordRegister.AutoSize = true;
            this.cbShowPasswordRegister.Location = new System.Drawing.Point(182, 212);
            this.cbShowPasswordRegister.Name = "cbShowPasswordRegister";
            this.cbShowPasswordRegister.Size = new System.Drawing.Size(126, 21);
            this.cbShowPasswordRegister.TabIndex = 14;
            this.cbShowPasswordRegister.Text = "Show Password";
            this.cbShowPasswordRegister.UseVisualStyleBackColor = true;
            this.cbShowPasswordRegister.CheckedChanged += new System.EventHandler(this.cbShowPasswordRegister_CheckedChanged);
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.Location = new System.Drawing.Point(153, 174);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.Size = new System.Drawing.Size(155, 24);
            this.txtConfirmPassword.TabIndex = 4;
            this.txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label6.Location = new System.Drawing.Point(26, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 21);
            this.label6.TabIndex = 13;
            this.label6.Text = "Conf Password: ";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(152, 89);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(155, 24);
            this.txtEmail.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label5.Location = new System.Drawing.Point(25, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 21);
            this.label5.TabIndex = 11;
            this.label5.Text = "Email: ";
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(152, 239);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(75, 23);
            this.btnRegister.TabIndex = 5;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // txtPasswordRegister
            // 
            this.txtPasswordRegister.Location = new System.Drawing.Point(152, 128);
            this.txtPasswordRegister.Name = "txtPasswordRegister";
            this.txtPasswordRegister.Size = new System.Drawing.Size(155, 24);
            this.txtPasswordRegister.TabIndex = 3;
            this.txtPasswordRegister.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label3.Location = new System.Drawing.Point(25, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 21);
            this.label3.TabIndex = 9;
            this.label3.Text = "Password: ";
            // 
            // btnClearRegister
            // 
            this.btnClearRegister.Location = new System.Drawing.Point(233, 239);
            this.btnClearRegister.Name = "btnClearRegister";
            this.btnClearRegister.Size = new System.Drawing.Size(75, 23);
            this.btnClearRegister.TabIndex = 7;
            this.btnClearRegister.Text = "Clear";
            this.btnClearRegister.UseVisualStyleBackColor = true;
            this.btnClearRegister.Click += new System.EventHandler(this.btnClearRegister_Click);
            // 
            // txtUserNameRegister
            // 
            this.txtUserNameRegister.Location = new System.Drawing.Point(153, 47);
            this.txtUserNameRegister.Name = "txtUserNameRegister";
            this.txtUserNameRegister.Size = new System.Drawing.Size(155, 24);
            this.txtUserNameRegister.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label4.Location = new System.Drawing.Point(26, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 21);
            this.label4.TabIndex = 4;
            this.label4.Text = "User Name: ";
            // 
            // LoginRegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 385);
            this.Controls.Add(this.tabControl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginRegisterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginRegisterForm";
            this.tabControl.ResumeLayout(false);
            this.tabLogin.ResumeLayout(false);
            this.tabLogin.PerformLayout();
            this.tabRegister.ResumeLayout(false);
            this.tabRegister.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabRegister;
        private System.Windows.Forms.TabPage tabLogin;
        private System.Windows.Forms.Button Login;
        private System.Windows.Forms.TextBox txtPasswordLogin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClearLogin;
        private System.Windows.Forms.TextBox txtUserNameLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.TextBox txtPasswordRegister;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnClearRegister;
        private System.Windows.Forms.TextBox txtUserNameRegister;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbShowPasswordLogin;
        private System.Windows.Forms.CheckBox cbShowPasswordRegister;
    }
}