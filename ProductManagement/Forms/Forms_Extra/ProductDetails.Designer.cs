namespace ProductManagement.Forms.Forms_Extra
{
    partial class ProductDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductDetails));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            picProduct = new Guna.UI2.WinForms.Guna2PictureBox();
            sANPHAMBindingSource = new BindingSource(components);
            lblLoaiHang = new Label();
            lOAISANPHAMBindingSource = new BindingSource(components);
            lblTen = new Label();
            lblGia = new Label();
            lblDVT = new Label();
            btnAddCard = new Guna.UI2.WinForms.Guna2Button();
            guna2CustomGradientPanel1 = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            guna2CustomGradientPanel2 = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            guna2CustomGradientPanel3 = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            guna2CustomGradientPanel4 = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            btnExit = new Guna.UI2.WinForms.Guna2Button();
            numSoLuong = new ReaLTaiizor.Controls.DungeonNumeric();
            btnBuy = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)picProduct).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sANPHAMBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lOAISANPHAMBindingSource).BeginInit();
            SuspendLayout();
            // 
            // picProduct
            // 
            picProduct.CustomizableEdges = customizableEdges1;
            picProduct.DataBindings.Add(new Binding("Image", sANPHAMBindingSource, "HINH", true));
            picProduct.ImageRotate = 0F;
            picProduct.Location = new Point(28, 40);
            picProduct.Name = "picProduct";
            picProduct.ShadowDecoration.CustomizableEdges = customizableEdges2;
            picProduct.Size = new Size(349, 374);
            picProduct.SizeMode = PictureBoxSizeMode.Zoom;
            picProduct.TabIndex = 0;
            picProduct.TabStop = false;
            // 
            // sANPHAMBindingSource
            // 
            sANPHAMBindingSource.DataSource = typeof(Models.SANPHAM);
            // 
            // lblLoaiHang
            // 
            lblLoaiHang.AutoSize = true;
            lblLoaiHang.DataBindings.Add(new Binding("Text", lOAISANPHAMBindingSource, "TENLOAI", true));
            lblLoaiHang.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLoaiHang.Location = new Point(414, 60);
            lblLoaiHang.Name = "lblLoaiHang";
            lblLoaiHang.Size = new Size(59, 25);
            lblLoaiHang.TabIndex = 1;
            lblLoaiHang.Text = "label1";
            // 
            // lOAISANPHAMBindingSource
            // 
            lOAISANPHAMBindingSource.DataSource = typeof(Models.LOAISANPHAM);
            // 
            // lblTen
            // 
            lblTen.AutoSize = true;
            lblTen.DataBindings.Add(new Binding("Text", sANPHAMBindingSource, "TENSP", true));
            lblTen.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTen.Location = new Point(414, 112);
            lblTen.Name = "lblTen";
            lblTen.Size = new Size(70, 28);
            lblTen.TabIndex = 2;
            lblTen.Text = "label2";
            // 
            // lblGia
            // 
            lblGia.AutoSize = true;
            lblGia.DataBindings.Add(new Binding("Text", sANPHAMBindingSource, "GHICHU", true));
            lblGia.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblGia.Location = new Point(414, 181);
            lblGia.Name = "lblGia";
            lblGia.Size = new Size(60, 25);
            lblGia.TabIndex = 4;
            lblGia.Text = "label3";
            // 
            // lblDVT
            // 
            lblDVT.AutoSize = true;
            lblDVT.DataBindings.Add(new Binding("Text", sANPHAMBindingSource, "DVT", true));
            lblDVT.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDVT.Location = new Point(414, 246);
            lblDVT.Name = "lblDVT";
            lblDVT.Size = new Size(56, 23);
            lblDVT.TabIndex = 5;
            lblDVT.Text = "label4";
            // 
            // btnAddCard
            // 
            btnAddCard.BorderRadius = 18;
            btnAddCard.CustomizableEdges = customizableEdges3;
            btnAddCard.DisabledState.BorderColor = Color.DarkGray;
            btnAddCard.DisabledState.CustomBorderColor = Color.DarkGray;
            btnAddCard.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnAddCard.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnAddCard.FillColor = Color.Green;
            btnAddCard.Font = new Font("Segoe UI", 9F);
            btnAddCard.ForeColor = Color.White;
            btnAddCard.Image = (Image)resources.GetObject("btnAddCard.Image");
            btnAddCard.ImageAlign = HorizontalAlignment.Right;
            btnAddCard.Location = new Point(524, 343);
            btnAddCard.Name = "btnAddCard";
            btnAddCard.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnAddCard.Size = new Size(144, 43);
            btnAddCard.TabIndex = 6;
            btnAddCard.Text = "Thêm vào giỏ";
            btnAddCard.TextAlign = HorizontalAlignment.Left;
            btnAddCard.Click += btnAddCard_Click;
            // 
            // guna2CustomGradientPanel1
            // 
            guna2CustomGradientPanel1.CustomizableEdges = customizableEdges5;
            guna2CustomGradientPanel1.Dock = DockStyle.Bottom;
            guna2CustomGradientPanel1.FillColor = Color.Red;
            guna2CustomGradientPanel1.FillColor2 = Color.FromArgb(192, 255, 255);
            guna2CustomGradientPanel1.FillColor3 = Color.FromArgb(255, 192, 192);
            guna2CustomGradientPanel1.FillColor4 = Color.FromArgb(192, 255, 192);
            guna2CustomGradientPanel1.Location = new Point(0, 454);
            guna2CustomGradientPanel1.Name = "guna2CustomGradientPanel1";
            guna2CustomGradientPanel1.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2CustomGradientPanel1.Size = new Size(875, 13);
            guna2CustomGradientPanel1.TabIndex = 12;
            // 
            // guna2CustomGradientPanel2
            // 
            guna2CustomGradientPanel2.CustomizableEdges = customizableEdges7;
            guna2CustomGradientPanel2.Dock = DockStyle.Top;
            guna2CustomGradientPanel2.FillColor = Color.Red;
            guna2CustomGradientPanel2.FillColor2 = Color.FromArgb(192, 255, 255);
            guna2CustomGradientPanel2.FillColor3 = Color.FromArgb(255, 128, 128);
            guna2CustomGradientPanel2.FillColor4 = Color.FromArgb(192, 255, 192);
            guna2CustomGradientPanel2.Location = new Point(0, 0);
            guna2CustomGradientPanel2.Name = "guna2CustomGradientPanel2";
            guna2CustomGradientPanel2.ShadowDecoration.CustomizableEdges = customizableEdges8;
            guna2CustomGradientPanel2.Size = new Size(875, 13);
            guna2CustomGradientPanel2.TabIndex = 13;
            // 
            // guna2CustomGradientPanel3
            // 
            guna2CustomGradientPanel3.CustomizableEdges = customizableEdges9;
            guna2CustomGradientPanel3.Dock = DockStyle.Right;
            guna2CustomGradientPanel3.FillColor = Color.Red;
            guna2CustomGradientPanel3.FillColor2 = Color.FromArgb(192, 255, 255);
            guna2CustomGradientPanel3.FillColor3 = Color.FromArgb(255, 128, 128);
            guna2CustomGradientPanel3.FillColor4 = Color.FromArgb(192, 255, 192);
            guna2CustomGradientPanel3.Location = new Point(862, 13);
            guna2CustomGradientPanel3.Name = "guna2CustomGradientPanel3";
            guna2CustomGradientPanel3.ShadowDecoration.CustomizableEdges = customizableEdges10;
            guna2CustomGradientPanel3.Size = new Size(13, 441);
            guna2CustomGradientPanel3.TabIndex = 14;
            // 
            // guna2CustomGradientPanel4
            // 
            guna2CustomGradientPanel4.CustomizableEdges = customizableEdges11;
            guna2CustomGradientPanel4.Dock = DockStyle.Left;
            guna2CustomGradientPanel4.FillColor = Color.Red;
            guna2CustomGradientPanel4.FillColor2 = Color.FromArgb(192, 255, 255);
            guna2CustomGradientPanel4.FillColor3 = Color.FromArgb(255, 128, 128);
            guna2CustomGradientPanel4.FillColor4 = Color.FromArgb(192, 255, 192);
            guna2CustomGradientPanel4.Location = new Point(0, 13);
            guna2CustomGradientPanel4.Name = "guna2CustomGradientPanel4";
            guna2CustomGradientPanel4.ShadowDecoration.CustomizableEdges = customizableEdges12;
            guna2CustomGradientPanel4.Size = new Size(13, 441);
            guna2CustomGradientPanel4.TabIndex = 15;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.Transparent;
            btnExit.BorderColor = Color.FromArgb(53, 41, 123);
            btnExit.BorderRadius = 35;
            btnExit.CheckedState.BorderColor = Color.White;
            btnExit.CheckedState.FillColor = Color.White;
            btnExit.CheckedState.Image = (Image)resources.GetObject("resource.Image");
            btnExit.Cursor = Cursors.Hand;
            btnExit.CustomizableEdges = customizableEdges13;
            btnExit.DisabledState.BorderColor = Color.DarkGray;
            btnExit.DisabledState.CustomBorderColor = Color.DarkGray;
            btnExit.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnExit.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnExit.FillColor = Color.Transparent;
            btnExit.Font = new Font("Segoe UI", 9F);
            btnExit.ForeColor = Color.White;
            btnExit.Image = (Image)resources.GetObject("btnExit.Image");
            btnExit.ImageSize = new Size(37, 37);
            btnExit.Location = new Point(780, 12);
            btnExit.Name = "btnExit";
            btnExit.ShadowDecoration.CustomizableEdges = customizableEdges14;
            btnExit.Size = new Size(83, 68);
            btnExit.TabIndex = 16;
            btnExit.UseTransparentBackground = true;
            btnExit.Click += btnExit_Click;
            // 
            // numSoLuong
            // 
            numSoLuong.BackColor = Color.Transparent;
            numSoLuong.BackColorA = Color.FromArgb(246, 246, 246);
            numSoLuong.BackColorB = Color.FromArgb(254, 254, 254);
            numSoLuong.BorderColor = Color.FromArgb(180, 180, 180);
            numSoLuong.ButtonForeColorA = Color.FromArgb(75, 75, 75);
            numSoLuong.ButtonForeColorB = Color.FromArgb(75, 75, 75);
            numSoLuong.Font = new Font("Tahoma", 11F);
            numSoLuong.ForeColor = Color.FromArgb(76, 76, 76);
            numSoLuong.Location = new Point(414, 350);
            numSoLuong.Maximum = 100L;
            numSoLuong.Minimum = 1L;
            numSoLuong.MinimumSize = new Size(93, 28);
            numSoLuong.Name = "numSoLuong";
            numSoLuong.Size = new Size(93, 28);
            numSoLuong.TabIndex = 22;
            numSoLuong.Text = "dungeonNumeric1";
            numSoLuong.TextAlignment = ReaLTaiizor.Controls.DungeonNumeric._TextAlignment.Near;
            numSoLuong.Value = 1L;
            // 
            // btnBuy
            // 
            btnBuy.BorderRadius = 18;
            btnBuy.CustomizableEdges = customizableEdges15;
            btnBuy.DisabledState.BorderColor = Color.DarkGray;
            btnBuy.DisabledState.CustomBorderColor = Color.DarkGray;
            btnBuy.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnBuy.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnBuy.FillColor = Color.FromArgb(192, 64, 0);
            btnBuy.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBuy.ForeColor = Color.White;
            btnBuy.Location = new Point(690, 343);
            btnBuy.Name = "btnBuy";
            btnBuy.ShadowDecoration.CustomizableEdges = customizableEdges16;
            btnBuy.Size = new Size(144, 43);
            btnBuy.TabIndex = 23;
            btnBuy.Text = "Mua ngay";
            // 
            // ProductDetails
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(875, 467);
            Controls.Add(btnBuy);
            Controls.Add(numSoLuong);
            Controls.Add(btnExit);
            Controls.Add(guna2CustomGradientPanel4);
            Controls.Add(guna2CustomGradientPanel3);
            Controls.Add(guna2CustomGradientPanel2);
            Controls.Add(guna2CustomGradientPanel1);
            Controls.Add(btnAddCard);
            Controls.Add(lblDVT);
            Controls.Add(lblGia);
            Controls.Add(lblTen);
            Controls.Add(lblLoaiHang);
            Controls.Add(picProduct);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ProductDetails";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ProductDetails";
            Load += ProductDetails_Load;
            ((System.ComponentModel.ISupportInitialize)picProduct).EndInit();
            ((System.ComponentModel.ISupportInitialize)sANPHAMBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)lOAISANPHAMBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox picProduct;
        private Label lblLoaiHang;
        private Label lblTen;
        private Label lblGia;
        private Label lblDVT;
        private Guna.UI2.WinForms.Guna2Button btnAddCard;
        private BindingSource sANPHAMBindingSource;
        private BindingSource lOAISANPHAMBindingSource;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel guna2CustomGradientPanel1;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel guna2CustomGradientPanel2;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel guna2CustomGradientPanel3;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel guna2CustomGradientPanel4;
        private Guna.UI2.WinForms.Guna2Button btnExit;
        private ReaLTaiizor.Controls.DungeonNumeric numSoLuong;
        private Guna.UI2.WinForms.Guna2Button btnBuy;
    }
}