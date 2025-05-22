namespace Shopping_App.Forms
{
    partial class ShippingInfoForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbShippingAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbPaymentMethod = new System.Windows.Forms.ComboBox();
            this.btnConfirme = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F);
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Shipping Address: ";
            // 
            // tbShippingAddress
            // 
            this.tbShippingAddress.Location = new System.Drawing.Point(193, 34);
            this.tbShippingAddress.Name = "tbShippingAddress";
            this.tbShippingAddress.Size = new System.Drawing.Size(226, 24);
            this.tbShippingAddress.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F);
            this.label2.Location = new System.Drawing.Point(12, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "Payment Method: ";
            // 
            // cbPaymentMethod
            // 
            this.cbPaymentMethod.FormattingEnabled = true;
            this.cbPaymentMethod.Location = new System.Drawing.Point(191, 87);
            this.cbPaymentMethod.Name = "cbPaymentMethod";
            this.cbPaymentMethod.Size = new System.Drawing.Size(228, 24);
            this.cbPaymentMethod.TabIndex = 2;
            // 
            // btnConfirme
            // 
            this.btnConfirme.Location = new System.Drawing.Point(331, 135);
            this.btnConfirme.Name = "btnConfirme";
            this.btnConfirme.Size = new System.Drawing.Size(88, 23);
            this.btnConfirme.TabIndex = 3;
            this.btnConfirme.Text = "Confirme";
            this.btnConfirme.UseVisualStyleBackColor = true;
            this.btnConfirme.Click += new System.EventHandler(this.btnConfirme_Click);
            // 
            // ShippingInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 170);
            this.Controls.Add(this.btnConfirme);
            this.Controls.Add(this.cbPaymentMethod);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbShippingAddress);
            this.Controls.Add(this.label1);
            this.Name = "ShippingInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShippingInfoForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbShippingAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbPaymentMethod;
        private System.Windows.Forms.Button btnConfirme;
    }
}