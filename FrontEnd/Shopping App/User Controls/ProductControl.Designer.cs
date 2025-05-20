namespace Shopping_App.User_Controls
{
    partial class ProductControl
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
            this.lbProductName = new System.Windows.Forms.Label();
            this.lbCategory = new System.Windows.Forms.Label();
            this.lbPrice = new System.Windows.Forms.Label();
            this.lbDescription = new System.Windows.Forms.Label();
            this.btnAddRemove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbProductName
            // 
            this.lbProductName.AutoSize = true;
            this.lbProductName.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbProductName.Location = new System.Drawing.Point(3, 9);
            this.lbProductName.Name = "lbProductName";
            this.lbProductName.Size = new System.Drawing.Size(114, 21);
            this.lbProductName.TabIndex = 0;
            this.lbProductName.Text = "Product Name";
            // 
            // lbCategory
            // 
            this.lbCategory.AutoSize = true;
            this.lbCategory.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbCategory.Location = new System.Drawing.Point(3, 41);
            this.lbCategory.Name = "lbCategory";
            this.lbCategory.Size = new System.Drawing.Size(137, 21);
            this.lbCategory.TabIndex = 1;
            this.lbCategory.Text = "Product Category";
            // 
            // lbPrice
            // 
            this.lbPrice.AutoSize = true;
            this.lbPrice.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbPrice.Location = new System.Drawing.Point(3, 74);
            this.lbPrice.Name = "lbPrice";
            this.lbPrice.Size = new System.Drawing.Size(46, 21);
            this.lbPrice.TabIndex = 2;
            this.lbPrice.Text = "Price";
            // 
            // lbDescription
            // 
            this.lbDescription.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbDescription.Location = new System.Drawing.Point(3, 110);
            this.lbDescription.Name = "lbDescription";
            this.lbDescription.Size = new System.Drawing.Size(255, 34);
            this.lbDescription.TabIndex = 3;
            this.lbDescription.Text = "Description";
            // 
            // btnAddRemove
            // 
            this.btnAddRemove.Location = new System.Drawing.Point(128, 154);
            this.btnAddRemove.Name = "btnAddRemove";
            this.btnAddRemove.Size = new System.Drawing.Size(129, 23);
            this.btnAddRemove.TabIndex = 4;
            this.btnAddRemove.Text = "Add To Cart";
            this.btnAddRemove.UseVisualStyleBackColor = true;
            this.btnAddRemove.Click += new System.EventHandler(this.btnAddRemove_Click);
            // 
            // ProductControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnAddRemove);
            this.Controls.Add(this.lbDescription);
            this.Controls.Add(this.lbPrice);
            this.Controls.Add(this.lbCategory);
            this.Controls.Add(this.lbProductName);
            this.Name = "ProductControl";
            this.Size = new System.Drawing.Size(260, 180);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbProductName;
        private System.Windows.Forms.Label lbCategory;
        private System.Windows.Forms.Label lbPrice;
        private System.Windows.Forms.Label lbDescription;
        private System.Windows.Forms.Button btnAddRemove;
    }
}
