namespace MiniShop.Forms.Forms_Extra
{
    partial class FillterPrice
    {
        private Guna.UI2.WinForms.Guna2TextBox txtMin;
        private Guna.UI2.WinForms.Guna2TextBox txtMax;
        private Guna.UI2.WinForms.Guna2Button btnOk;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
        private Label lblMin;
        private Label lblMax;

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
            txtMin = new Guna.UI2.WinForms.Guna2TextBox();
            txtMax = new Guna.UI2.WinForms.Guna2TextBox();
            btnOk = new Guna.UI2.WinForms.Guna2Button();
            btnCancel = new Guna.UI2.WinForms.Guna2Button();
            lblMin = new Label();
            lblMax = new Label();
            SuspendLayout();
            // txtMin
            txtMin.Name = "txtMin";
            txtMin.AutoRoundedCorners = true;
            txtMin.BorderRadius = 18;
            txtMin.PlaceholderText = "Giá min";
            txtMin.Location = new System.Drawing.Point(40, 60);
            txtMin.Size = new System.Drawing.Size(220, 38);
            txtMin.TabIndex = 0;
            // txtMax
            txtMax.Name = "txtMax";
            txtMax.AutoRoundedCorners = true;
            txtMax.BorderRadius = 18;
            txtMax.PlaceholderText = "Giá max";
            txtMax.Location = new System.Drawing.Point(320, 60);
            txtMax.Size = new System.Drawing.Size(220, 38);
            txtMax.TabIndex = 1;
            // lblMin
            lblMin.AutoSize = true;
            lblMin.Text = "Min";
            lblMin.Location = new System.Drawing.Point(40, 35);
            // lblMax
            lblMax.AutoSize = true;
            lblMax.Text = "Max";
            lblMax.Location = new System.Drawing.Point(320, 35);
            // btnOk
            btnOk.Name = "btnOk";
            btnOk.Text = "Lọc";
            btnOk.AutoRoundedCorners = true;
            btnOk.BorderRadius = 20;
            btnOk.Location = new System.Drawing.Point(320, 130);
            btnOk.Size = new System.Drawing.Size(100, 42);
            btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // btnCancel
            btnCancel.Name = "btnCancel";
            btnCancel.Text = "Hủy";
            btnCancel.AutoRoundedCorners = true;
            btnCancel.BorderRadius = 20;
            btnCancel.Location = new System.Drawing.Point(440, 130);
            btnCancel.Size = new System.Drawing.Size(100, 42);
            btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // Form
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(582, 200);
            Controls.Add(lblMin);
            Controls.Add(lblMax);
            Controls.Add(txtMin);
            Controls.Add(txtMax);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Name = "FillterPrice";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Lọc theo giá";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}