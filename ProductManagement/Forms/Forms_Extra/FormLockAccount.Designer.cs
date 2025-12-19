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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lblTitle = new Label();
            rdbTemporary = new Guna.UI2.WinForms.Guna2RadioButton();
            rdbPermanent = new Guna.UI2.WinForms.Guna2RadioButton();
            numDays = new Guna.UI2.WinForms.Guna2NumericUpDown();
            lblDays = new Label();
            btnConfirm = new Guna.UI2.WinForms.Guna2Button();
            btnCancel = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)numDays).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(299, 29);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Chọn loại khóa tài khoản";
            // 
            // rdbTemporary
            // 
            rdbTemporary.AutoSize = true;
            rdbTemporary.CheckedState.BorderColor = Color.FromArgb(94, 148, 255);
            rdbTemporary.CheckedState.BorderThickness = 0;
            rdbTemporary.CheckedState.FillColor = Color.FromArgb(94, 148, 255);
            rdbTemporary.CheckedState.InnerColor = Color.White;
            rdbTemporary.Font = new Font("Microsoft Sans Serif", 12F);
            rdbTemporary.Location = new Point(25, 70);
            rdbTemporary.Name = "rdbTemporary";
            rdbTemporary.Size = new Size(153, 29);
            rdbTemporary.TabIndex = 1;
            rdbTemporary.Text = "Khóa tạm thời";
            rdbTemporary.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            rdbTemporary.UncheckedState.BorderThickness = 2;
            rdbTemporary.UncheckedState.FillColor = Color.Transparent;
            rdbTemporary.UncheckedState.InnerColor = Color.Transparent;
            rdbTemporary.CheckedChanged += rdbTemporary_CheckedChanged;
            // 
            // rdbPermanent
            // 
            rdbPermanent.AutoSize = true;
            rdbPermanent.CheckedState.BorderColor = Color.FromArgb(94, 148, 255);
            rdbPermanent.CheckedState.BorderThickness = 0;
            rdbPermanent.CheckedState.FillColor = Color.FromArgb(94, 148, 255);
            rdbPermanent.CheckedState.InnerColor = Color.White;
            rdbPermanent.Font = new Font("Microsoft Sans Serif", 12F);
            rdbPermanent.Location = new Point(25, 120);
            rdbPermanent.Name = "rdbPermanent";
            rdbPermanent.Size = new Size(162, 29);
            rdbPermanent.TabIndex = 2;
            rdbPermanent.Text = "Khóa vĩnh viễn";
            rdbPermanent.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            rdbPermanent.UncheckedState.BorderThickness = 2;
            rdbPermanent.UncheckedState.FillColor = Color.Transparent;
            rdbPermanent.UncheckedState.InnerColor = Color.Transparent;
            rdbPermanent.CheckedChanged += rdbPermanent_CheckedChanged;
            // 
            // numDays
            // 
            numDays.BackColor = Color.Transparent;
            numDays.Cursor = Cursors.IBeam;
            numDays.CustomizableEdges = customizableEdges1;
            numDays.Font = new Font("Microsoft Sans Serif", 12F);
            numDays.Location = new Point(200, 70);
            numDays.Margin = new Padding(3, 4, 3, 4);
            numDays.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            numDays.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDays.Name = "numDays";
            numDays.ShadowDecoration.CustomizableEdges = customizableEdges2;
            numDays.Size = new Size(100, 36);
            numDays.TabIndex = 3;
            numDays.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblDays
            // 
            lblDays.AutoSize = true;
            lblDays.Font = new Font("Microsoft Sans Serif", 10F);
            lblDays.Location = new Point(310, 78);
            lblDays.Name = "lblDays";
            lblDays.Size = new Size(44, 20);
            lblDays.TabIndex = 4;
            lblDays.Text = "ngày";
            // 
            // btnConfirm
            // 
            btnConfirm.BorderRadius = 8;
            btnConfirm.CustomizableEdges = customizableEdges3;
            btnConfirm.DisabledState.BorderColor = Color.DarkGray;
            btnConfirm.DisabledState.CustomBorderColor = Color.DarkGray;
            btnConfirm.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnConfirm.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnConfirm.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            btnConfirm.ForeColor = Color.White;
            btnConfirm.Location = new Point(200, 180);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnConfirm.Size = new Size(139, 40);
            btnConfirm.TabIndex = 5;
            btnConfirm.Text = "Xác nhận";
            btnConfirm.Click += btnConfirm_Click;
            // 
            // btnCancel
            // 
            btnCancel.BorderRadius = 8;
            btnCancel.CustomizableEdges = customizableEdges5;
            btnCancel.DisabledState.BorderColor = Color.DarkGray;
            btnCancel.DisabledState.CustomBorderColor = Color.DarkGray;
            btnCancel.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnCancel.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnCancel.FillColor = Color.FromArgb(220, 53, 69);
            btnCancel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(50, 180);
            btnCancel.Name = "btnCancel";
            btnCancel.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnCancel.Size = new Size(120, 40);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Hủy";
            btnCancel.Click += btnCancel_Click;
            // 
            // FormLockAccount
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 250);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Controls.Add(lblDays);
            Controls.Add(numDays);
            Controls.Add(rdbPermanent);
            Controls.Add(rdbTemporary);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormLockAccount";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Khóa tài khoản";
            Load += FormLockAccount_Load;
            ((System.ComponentModel.ISupportInitialize)numDays).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}

