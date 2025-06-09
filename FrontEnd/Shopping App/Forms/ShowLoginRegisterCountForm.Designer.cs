namespace Shopping_App.Forms
{
    partial class ShowLoginRegisterCountForm
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
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbCountTitle = new System.Windows.Forms.Label();
            this.lbCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox
            // 
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(96, 36);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(121, 24);
            this.comboBox.TabIndex = 0;
            this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Duration";
            // 
            // lbCountTitle
            // 
            this.lbCountTitle.AutoSize = true;
            this.lbCountTitle.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbCountTitle.Location = new System.Drawing.Point(12, 87);
            this.lbCountTitle.Name = "lbCountTitle";
            this.lbCountTitle.Size = new System.Drawing.Size(74, 21);
            this.lbCountTitle.TabIndex = 2;
            this.lbCountTitle.Text = "Duration";
            // 
            // lbCount
            // 
            this.lbCount.AutoSize = true;
            this.lbCount.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbCount.Location = new System.Drawing.Point(149, 87);
            this.lbCount.Name = "lbCount";
            this.lbCount.Size = new System.Drawing.Size(51, 21);
            this.lbCount.TabIndex = 3;
            this.lbCount.Text = "count";
            // 
            // ShowLoginRegisterCountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 142);
            this.Controls.Add(this.lbCount);
            this.Controls.Add(this.lbCountTitle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox);
            this.Name = "ShowLoginRegisterCountForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShowLoginRegisterCountForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbCountTitle;
        private System.Windows.Forms.Label lbCount;
    }
}