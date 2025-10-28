namespace MiniStore
{
    partial class FormVerifyOTP
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
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(components);
            guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(components);
            guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblErrorXacThuc = new Guna.UI2.WinForms.Guna2HtmlLabel();
            btnRegisterForm = new Guna.UI2.WinForms.Guna2Button();
            btnCancel = new Guna.UI2.WinForms.Guna2GradientButton();
            txtXacThuc = new Guna.UI2.WinForms.Guna2TextBox();
            SuspendLayout();
            // 
            // guna2AnimateWindow1
            // 
            guna2AnimateWindow1.AnimationType = Guna.UI2.WinForms.Guna2AnimateWindow.AnimateWindowType.AW_VER_POSITIVE;
            guna2AnimateWindow1.TargetForm = this;
            // 
            // guna2DragControl1
            // 
            guna2DragControl1.DockIndicatorTransparencyValue = 0.6D;
            guna2DragControl1.TargetControl = this;
            guna2DragControl1.UseTransparentDrag = true;
            // 
            // guna2HtmlLabel1
            // 
            guna2HtmlLabel1.BackColor = Color.Transparent;
            guna2HtmlLabel1.Font = new Font("Segoe UI", 20F);
            guna2HtmlLabel1.Location = new Point(242, 52);
            guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            guna2HtmlLabel1.Size = new Size(228, 47);
            guna2HtmlLabel1.TabIndex = 42;
            guna2HtmlLabel1.Text = "Quên mật khẩu";
            // 
            // lblErrorXacThuc
            // 
            lblErrorXacThuc.BackColor = Color.Transparent;
            lblErrorXacThuc.Location = new Point(203, 169);
            lblErrorXacThuc.Name = "lblErrorXacThuc";
            lblErrorXacThuc.Size = new Size(129, 22);
            lblErrorXacThuc.TabIndex = 41;
            lblErrorXacThuc.Text = "thong bao loi valid";
            // 
            // btnRegisterForm
            // 
            btnRegisterForm.BorderRadius = 30;
            btnRegisterForm.CustomizableEdges = customizableEdges1;
            btnRegisterForm.DisabledState.BorderColor = Color.DarkGray;
            btnRegisterForm.DisabledState.CustomBorderColor = Color.DarkGray;
            btnRegisterForm.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnRegisterForm.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnRegisterForm.Font = new Font("Segoe UI", 9F);
            btnRegisterForm.ForeColor = Color.White;
            btnRegisterForm.Location = new Point(541, 151);
            btnRegisterForm.Name = "btnRegisterForm";
            btnRegisterForm.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnRegisterForm.Size = new Size(225, 56);
            btnRegisterForm.TabIndex = 40;
            btnRegisterForm.Text = "Xác Thực";
            btnRegisterForm.Click += btnRegisterForm_Click;
            // 
            // btnCancel
            // 
            btnCancel.BorderRadius = 30;
            btnCancel.CustomizableEdges = customizableEdges3;
            btnCancel.DisabledState.BorderColor = Color.DarkGray;
            btnCancel.DisabledState.CustomBorderColor = Color.DarkGray;
            btnCancel.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnCancel.DisabledState.FillColor2 = Color.FromArgb(169, 169, 169);
            btnCancel.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnCancel.Font = new Font("Segoe UI", 9F);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(541, 238);
            btnCancel.Name = "btnCancel";
            btnCancel.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnCancel.Size = new Size(225, 56);
            btnCancel.TabIndex = 39;
            btnCancel.Text = "Hủy";
            btnCancel.Click += btnCancel_Click;
            // 
            // txtXacThuc
            // 
            txtXacThuc.CustomizableEdges = customizableEdges5;
            txtXacThuc.DefaultText = "";
            txtXacThuc.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtXacThuc.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtXacThuc.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtXacThuc.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtXacThuc.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtXacThuc.Font = new Font("Segoe UI", 9F);
            txtXacThuc.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtXacThuc.Location = new Point(196, 198);
            txtXacThuc.Margin = new Padding(3, 4, 3, 4);
            txtXacThuc.Name = "txtXacThuc";
            txtXacThuc.PlaceholderText = "Mã xác nhận ";
            txtXacThuc.SelectedText = "";
            txtXacThuc.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtXacThuc.Size = new Size(295, 51);
            txtXacThuc.TabIndex = 38;
            txtXacThuc.TextChanged += txtXacThuc_TextChanged;
            // 
            // FormVerifyOTP
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(800, 450);
            Controls.Add(guna2HtmlLabel1);
            Controls.Add(lblErrorXacThuc);
            Controls.Add(btnRegisterForm);
            Controls.Add(btnCancel);
            Controls.Add(txtXacThuc);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormVerifyOTP";
            Text = "FormForgot_1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow1;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblErrorXacThuc;
        private Guna.UI2.WinForms.Guna2Button btnRegisterForm;
        private Guna.UI2.WinForms.Guna2GradientButton btnCancel;
        private Guna.UI2.WinForms.Guna2TextBox txtXacThuc;
    }
}