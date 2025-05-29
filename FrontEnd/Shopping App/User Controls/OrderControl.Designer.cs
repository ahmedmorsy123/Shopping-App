namespace Shopping_App.User_Controls
{
    partial class OrderControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lbProductsCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbTotalPrice = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbShippingAddress = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbPaymentMethod = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.btnViewItems = new System.Windows.Forms.Button();
            this.lbCreatedAt = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Products Count: ";
            // 
            // lbProductsCount
            // 
            this.lbProductsCount.AutoSize = true;
            this.lbProductsCount.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbProductsCount.Location = new System.Drawing.Point(131, 12);
            this.lbProductsCount.Name = "lbProductsCount";
            this.lbProductsCount.Size = new System.Drawing.Size(28, 21);
            this.lbProductsCount.TabIndex = 1;
            this.lbProductsCount.Text = "00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label2.Location = new System.Drawing.Point(200, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "total Price: ";
            // 
            // lbTotalPrice
            // 
            this.lbTotalPrice.AutoSize = true;
            this.lbTotalPrice.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbTotalPrice.Location = new System.Drawing.Point(302, 12);
            this.lbTotalPrice.Name = "lbTotalPrice";
            this.lbTotalPrice.Size = new System.Drawing.Size(28, 21);
            this.lbTotalPrice.TabIndex = 3;
            this.lbTotalPrice.Text = "00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label3.Location = new System.Drawing.Point(3, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = "Shipping Address: ";
            // 
            // lbShippingAddress
            // 
            this.lbShippingAddress.AutoSize = true;
            this.lbShippingAddress.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbShippingAddress.Location = new System.Drawing.Point(157, 52);
            this.lbShippingAddress.Name = "lbShippingAddress";
            this.lbShippingAddress.Size = new System.Drawing.Size(53, 21);
            this.lbShippingAddress.TabIndex = 5;
            this.lbShippingAddress.Text = "Home";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label4.Location = new System.Drawing.Point(3, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 21);
            this.label4.TabIndex = 6;
            this.label4.Text = "Payment Method: ";
            // 
            // lbPaymentMethod
            // 
            this.lbPaymentMethod.AutoSize = true;
            this.lbPaymentMethod.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbPaymentMethod.Location = new System.Drawing.Point(154, 90);
            this.lbPaymentMethod.Name = "lbPaymentMethod";
            this.lbPaymentMethod.Size = new System.Drawing.Size(39, 21);
            this.lbPaymentMethod.TabIndex = 7;
            this.lbPaymentMethod.Text = "visa";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(643, 88);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label5.Location = new System.Drawing.Point(433, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 21);
            this.label5.TabIndex = 9;
            this.label5.Text = "Status: ";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbStatus.Location = new System.Drawing.Point(499, 12);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(68, 21);
            this.lbStatus.TabIndex = 10;
            this.lbStatus.Text = "pending";
            // 
            // btnViewItems
            // 
            this.btnViewItems.Location = new System.Drawing.Point(643, 50);
            this.btnViewItems.Name = "btnViewItems";
            this.btnViewItems.Size = new System.Drawing.Size(102, 23);
            this.btnViewItems.TabIndex = 11;
            this.btnViewItems.Text = "View Items";
            this.btnViewItems.UseVisualStyleBackColor = true;
            this.btnViewItems.Click += new System.EventHandler(this.btnViewItems_Click);
            // 
            // lbCreatedAt
            // 
            this.lbCreatedAt.AutoSize = true;
            this.lbCreatedAt.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbCreatedAt.Location = new System.Drawing.Point(437, 52);
            this.lbCreatedAt.Name = "lbCreatedAt";
            this.lbCreatedAt.Size = new System.Drawing.Size(78, 21);
            this.lbCreatedAt.TabIndex = 13;
            this.lbCreatedAt.Text = "7/7/7707";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label7.Location = new System.Drawing.Point(371, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 21);
            this.label7.TabIndex = 12;
            this.label7.Text = "Date: ";
            // 
            // OrderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lbCreatedAt);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnViewItems);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lbPaymentMethod);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbShippingAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbTotalPrice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbProductsCount);
            this.Controls.Add(this.label1);
            this.Name = "OrderControl";
            this.Size = new System.Drawing.Size(761, 133);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbProductsCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbTotalPrice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbShippingAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbPaymentMethod;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Button btnViewItems;
        private System.Windows.Forms.Label lbCreatedAt;
        private System.Windows.Forms.Label label7;
    }
}
