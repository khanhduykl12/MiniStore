namespace MiniStore
{
    partial class FormLogin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(components);
            txtUserName = new Guna.UI2.WinForms.Guna2TextBox();
            txtPassWord = new Guna.UI2.WinForms.Guna2TextBox();
            guna2GradientButton1 = new Guna.UI2.WinForms.Guna2GradientButton();
            btnRegisterForm = new Guna.UI2.WinForms.Guna2Button();
            guna2ImageButton1 = new Guna.UI2.WinForms.Guna2ImageButton();
            lblQuenMatKhau = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblErrorPassword = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblErrorUserName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            videoView1 = new LibVLCSharp.WinForms.VideoView();
            ((System.ComponentModel.ISupportInitialize)videoView1).BeginInit();
            SuspendLayout();
            // 
            // guna2DragControl1
            // 
            guna2DragControl1.DockIndicatorTransparencyValue = 0.6D;
            guna2DragControl1.TargetControl = this;
            guna2DragControl1.UseTransparentDrag = true;
            // 
            // txtUserName
            // 
            txtUserName.BackColor = Color.Transparent;
            txtUserName.CustomizableEdges = customizableEdges1;
            txtUserName.DefaultText = "";
            txtUserName.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtUserName.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtUserName.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtUserName.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtUserName.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtUserName.Font = new Font("Segoe UI", 9F);
            txtUserName.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtUserName.Location = new Point(330, 185);
            txtUserName.Margin = new Padding(3, 4, 3, 4);
            txtUserName.Name = "txtUserName";
            txtUserName.PlaceholderText = "UserName";
            txtUserName.SelectedText = "";
            txtUserName.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtUserName.Size = new Size(286, 60);
            txtUserName.TabIndex = 1;
            txtUserName.TextChanged += txtUserName_TextChanged;
            txtUserName.Leave += txtUserName_Leave;
            // 
            // txtPassWord
            // 
            txtPassWord.BackColor = Color.Transparent;
            txtPassWord.CustomizableEdges = customizableEdges4;
            txtPassWord.DefaultText = "";
            txtPassWord.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPassWord.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPassWord.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPassWord.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPassWord.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPassWord.Font = new Font("Segoe UI", 9F);
            txtPassWord.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPassWord.Location = new Point(330, 290);
            txtPassWord.Margin = new Padding(3, 4, 3, 4);
            txtPassWord.Name = "txtPassWord";
            txtPassWord.PasswordChar = '*';
            txtPassWord.PlaceholderText = "Password";
            txtPassWord.SelectedText = "";
            txtPassWord.ShadowDecoration.CustomizableEdges = customizableEdges5;
            txtPassWord.Size = new Size(286, 60);
            txtPassWord.TabIndex = 2;
            txtPassWord.TextChanged += txtPassWord_TextChanged;
            // 
            // guna2GradientButton1
            // 
            guna2GradientButton1.BackColor = Color.Transparent;
            guna2GradientButton1.CustomizableEdges = customizableEdges6;
            guna2GradientButton1.DisabledState.BorderColor = Color.DarkGray;
            guna2GradientButton1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2GradientButton1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2GradientButton1.DisabledState.FillColor2 = Color.FromArgb(169, 169, 169);
            guna2GradientButton1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2GradientButton1.Font = new Font("Segoe UI", 9F);
            guna2GradientButton1.ForeColor = Color.White;
            guna2GradientButton1.Location = new Point(656, 424);
            guna2GradientButton1.Name = "guna2GradientButton1";
            guna2GradientButton1.ShadowDecoration.CustomizableEdges = customizableEdges7;
            guna2GradientButton1.Size = new Size(224, 56);
            guna2GradientButton1.TabIndex = 3;
            guna2GradientButton1.Text = "Đăng Nhập";
            guna2GradientButton1.Click += guna2GradientButton1_Click;
            // 
            // btnRegisterForm
            // 
            btnRegisterForm.BackColor = Color.White;
            btnRegisterForm.CustomizableEdges = customizableEdges8;
            btnRegisterForm.DisabledState.BorderColor = Color.DarkGray;
            btnRegisterForm.DisabledState.CustomBorderColor = Color.DarkGray;
            btnRegisterForm.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnRegisterForm.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnRegisterForm.Font = new Font("Segoe UI", 9F);
            btnRegisterForm.ForeColor = Color.White;
            btnRegisterForm.Location = new Point(112, 424);
            btnRegisterForm.Name = "btnRegisterForm";
            btnRegisterForm.ShadowDecoration.CustomizableEdges = customizableEdges9;
            btnRegisterForm.Size = new Size(225, 56);
            btnRegisterForm.TabIndex = 4;
            btnRegisterForm.Text = "Đăng Ký";
            btnRegisterForm.Click += btnRegisterForm_Click;
            // 
            // guna2ImageButton1
            // 
            guna2ImageButton1.BackColor = Color.Transparent;
            guna2ImageButton1.CheckedState.ImageSize = new Size(64, 64);
            guna2ImageButton1.HoverState.ImageSize = new Size(64, 64);
            guna2ImageButton1.Image = (Image)resources.GetObject("guna2ImageButton1.Image");
            guna2ImageButton1.ImageOffset = new Point(0, 0);
            guna2ImageButton1.ImageRotate = 0F;
            guna2ImageButton1.Location = new Point(866, 12);
            guna2ImageButton1.Name = "guna2ImageButton1";
            guna2ImageButton1.PressedState.ImageSize = new Size(64, 64);
            guna2ImageButton1.ShadowDecoration.CustomizableEdges = customizableEdges3;
            guna2ImageButton1.Size = new Size(80, 68);
            guna2ImageButton1.TabIndex = 8;
            guna2ImageButton1.Click += guna2ImageButton1_Click;
            // 
            // lblQuenMatKhau
            // 
            lblQuenMatKhau.BackColor = Color.Transparent;
            lblQuenMatKhau.Location = new Point(415, 373);
            lblQuenMatKhau.Name = "lblQuenMatKhau";
            lblQuenMatKhau.Size = new Size(110, 22);
            lblQuenMatKhau.TabIndex = 9;
            lblQuenMatKhau.Text = "Quên mật khẩu?";
            lblQuenMatKhau.Click += lblQuenMatKhau_Click;
            // 
            // lblErrorPassword
            // 
            lblErrorPassword.BackColor = Color.Transparent;
            lblErrorPassword.Location = new Point(332, 261);
            lblErrorPassword.Name = "lblErrorPassword";
            lblErrorPassword.Size = new Size(129, 22);
            lblErrorPassword.TabIndex = 6;
            lblErrorPassword.Text = "thong bao loi valid";
            // 
            // lblErrorUserName
            // 
            lblErrorUserName.BackColor = Color.Transparent;
            lblErrorUserName.Location = new Point(363, 165);
            lblErrorUserName.Name = "lblErrorUserName";
            lblErrorUserName.Size = new Size(129, 22);
            lblErrorUserName.TabIndex = 5;
            lblErrorUserName.Text = "thong bao loi valid";
            // 
            // videoView1
            // 
            videoView1.BackColor = Color.Black;
            videoView1.Dock = DockStyle.Fill;
            videoView1.Location = new Point(0, 0);
            videoView1.MediaPlayer = null;
            videoView1.Name = "videoView1";
            videoView1.Size = new Size(975, 549);
            videoView1.TabIndex = 13;
            videoView1.Text = "videoView1";
            videoView1.Click += videoView1_Click_1;
            // 
            // FormLogin
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            ClientSize = new Size(975, 549);
            Controls.Add(txtUserName);
            Controls.Add(lblQuenMatKhau);
            Controls.Add(guna2ImageButton1);
            Controls.Add(lblErrorPassword);
            Controls.Add(txtPassWord);
            Controls.Add(lblErrorUserName);
            Controls.Add(guna2GradientButton1);
            Controls.Add(btnRegisterForm);
            Controls.Add(videoView1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập";
            Load += FormLogin_Load;
            ((System.ComponentModel.ISupportInitialize)videoView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblQuenMatKhau;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblErrorPassword;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblErrorUserName;
        private Guna.UI2.WinForms.Guna2Button btnRegisterForm;
        private Guna.UI2.WinForms.Guna2GradientButton guna2GradientButton1;
        private Guna.UI2.WinForms.Guna2TextBox txtPassWord;
        private Guna.UI2.WinForms.Guna2TextBox txtUserName;
        private LibVLCSharp.WinForms.VideoView videoView1;
    }
}
