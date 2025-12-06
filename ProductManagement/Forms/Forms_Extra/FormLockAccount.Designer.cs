namespace MiniShop.Forms_Extra
{
    partial class FormLockAccount
    {
        private System.ComponentModel.IContainer components = null;
        private Guna.UI2.WinForms.Guna2RadioButton rdbTemporary;
        private Guna.UI2.WinForms.Guna2RadioButton rdbPermanent;
        private Guna.UI2.WinForms.Guna2NumericUpDown numDays;
        private Guna.UI2.WinForms.Guna2Button btnConfirm;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDays;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.rdbTemporary = new Guna.UI2.WinForms.Guna2RadioButton();
            this.rdbPermanent = new Guna.UI2.WinForms.Guna2RadioButton();
            this.numDays = new Guna.UI2.WinForms.Guna2NumericUpDown();
            this.lblDays = new System.Windows.Forms.Label();
            this.btnConfirm = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.numDays)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Inter", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(300, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Chọn loại khóa tài khoản";
            // 
            // rdbTemporary
            // 
            this.rdbTemporary.AutoSize = true;
            this.rdbTemporary.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.rdbTemporary.CheckedState.BorderThickness = 0;
            this.rdbTemporary.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.rdbTemporary.CheckedState.InnerColor = System.Drawing.Color.White;
            this.rdbTemporary.Font = new System.Drawing.Font("Inter", 12F);
            this.rdbTemporary.Location = new System.Drawing.Point(25, 70);
            this.rdbTemporary.Name = "rdbTemporary";
            this.rdbTemporary.Size = new System.Drawing.Size(152, 28);
            this.rdbTemporary.TabIndex = 1;
            this.rdbTemporary.Text = "Khóa tạm thời";
            this.rdbTemporary.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.rdbTemporary.UncheckedState.BorderThickness = 2;
            this.rdbTemporary.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            this.rdbTemporary.UncheckedState.InnerColor = System.Drawing.Color.Transparent;
            this.rdbTemporary.CheckedChanged += new System.EventHandler(this.rdbTemporary_CheckedChanged);
            // 
            // rdbPermanent
            // 
            this.rdbPermanent.AutoSize = true;
            this.rdbPermanent.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.rdbPermanent.CheckedState.BorderThickness = 0;
            this.rdbPermanent.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.rdbPermanent.CheckedState.InnerColor = System.Drawing.Color.White;
            this.rdbPermanent.Font = new System.Drawing.Font("Inter", 12F);
            this.rdbPermanent.Location = new System.Drawing.Point(25, 120);
            this.rdbPermanent.Name = "rdbPermanent";
            this.rdbPermanent.Size = new System.Drawing.Size(160, 28);
            this.rdbPermanent.TabIndex = 2;
            this.rdbPermanent.Text = "Khóa vĩnh viễn";
            this.rdbPermanent.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.rdbPermanent.UncheckedState.BorderThickness = 2;
            this.rdbPermanent.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            this.rdbPermanent.UncheckedState.InnerColor = System.Drawing.Color.Transparent;
            this.rdbPermanent.CheckedChanged += new System.EventHandler(this.rdbPermanent_CheckedChanged);
            // 
            // numDays
            // 
            this.numDays.BackColor = System.Drawing.Color.Transparent;
            this.numDays.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.numDays.Font = new System.Drawing.Font("Inter", 12F);
            this.numDays.Location = new System.Drawing.Point(200, 70);
            this.numDays.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDays.Name = "numDays";
            this.numDays.Size = new System.Drawing.Size(100, 36);
            this.numDays.TabIndex = 3;
            this.numDays.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblDays
            // 
            this.lblDays.AutoSize = true;
            this.lblDays.Font = new System.Drawing.Font("Inter", 10F);
            this.lblDays.Location = new System.Drawing.Point(310, 78);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(50, 20);
            this.lblDays.TabIndex = 4;
            this.lblDays.Text = "ngày";
            // 
            // btnConfirm
            // 
            this.btnConfirm.BorderRadius = 8;
            this.btnConfirm.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnConfirm.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnConfirm.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnConfirm.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnConfirm.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.btnConfirm.Font = new System.Drawing.Font("Inter", 12F, System.Drawing.FontStyle.Bold);
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(200, 180);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(120, 40);
            this.btnConfirm.TabIndex = 5;
            this.btnConfirm.Text = "Xác nhận";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BorderRadius = 8;
            this.btnCancel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCancel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCancel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnCancel.Font = new System.Drawing.Font("Inter", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(50, 180);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 40);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FormLockAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 250);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.lblDays);
            this.Controls.Add(this.numDays);
            this.Controls.Add(this.rdbPermanent);
            this.Controls.Add(this.rdbTemporary);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLockAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Khóa tài khoản";
            this.Load += new System.EventHandler(this.FormLockAccount_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

