namespace Shopping_App.User_Controls
{
    partial class OrderItemControl
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
            this.label2 = new System.Windows.Forms.Label();
            this.lbTotalPrice = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbProductName = new System.Windows.Forms.Label();
            this.lbQuentity = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label2.Location = new System.Drawing.Point(377, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 21);
            this.label2.TabIndex = 11;
            this.label2.Text = "Total Price: ";
            // 
            // lbTotalPrice
            // 
            this.lbTotalPrice.AutoSize = true;
            this.lbTotalPrice.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbTotalPrice.Location = new System.Drawing.Point(483, 16);
            this.lbTotalPrice.Name = "lbTotalPrice";
            this.lbTotalPrice.Size = new System.Drawing.Size(51, 21);
            this.lbTotalPrice.TabIndex = 9;
            this.lbTotalPrice.Text = "564.6";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label1.Location = new System.Drawing.Point(276, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 21);
            this.label1.TabIndex = 8;
            this.label1.Text = "X";
            // 
            // lbProductName
            // 
            this.lbProductName.AutoSize = true;
            this.lbProductName.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbProductName.Location = new System.Drawing.Point(3, 15);
            this.lbProductName.Name = "lbProductName";
            this.lbProductName.Size = new System.Drawing.Size(268, 21);
            this.lbProductName.TabIndex = 7;
            this.lbProductName.Text = "Product Name is a very long name ";
            // 
            // lbQuentity
            // 
            this.lbQuentity.AutoSize = true;
            this.lbQuentity.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbQuentity.Location = new System.Drawing.Point(302, 16);
            this.lbQuentity.Name = "lbQuentity";
            this.lbQuentity.Size = new System.Drawing.Size(19, 21);
            this.lbQuentity.TabIndex = 12;
            this.lbQuentity.Text = "5";
            // 
            // OrderItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lbQuentity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbTotalPrice);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbProductName);
            this.Name = "OrderItemControl";
            this.Size = new System.Drawing.Size(558, 48);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbTotalPrice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbProductName;
        private System.Windows.Forms.Label lbQuentity;
    }
}
