namespace ProductManagement.User_Control.UC_Extra
{
    partial class UC_ShopPCart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_ShopPCart));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            picProduct = new PictureBox();
            lblTen = new Label();
            lblDVT = new Label();
            lblGia = new Label();
            btnDelete = new Guna.UI2.WinForms.Guna2ImageButton();
            numSoLuong = new NumericUpDown();
            panelContain = new Guna.UI2.WinForms.Guna2Panel();
            ((System.ComponentModel.ISupportInitialize)picProduct).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSoLuong).BeginInit();
            SuspendLayout();
            // 
            // picProduct
            // 
            picProduct.Location = new Point(10, 3);
            picProduct.Name = "picProduct";
            picProduct.Size = new Size(71, 57);
            picProduct.SizeMode = PictureBoxSizeMode.Zoom;
            picProduct.TabIndex = 0;
            picProduct.TabStop = false;
            // 
            // lblTen
            // 
            lblTen.AutoSize = true;
            lblTen.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTen.Location = new Point(89, 6);
            lblTen.Name = "lblTen";
            lblTen.Size = new Size(36, 23);
            lblTen.TabIndex = 1;
            lblTen.Text = "Ten";
            // 
            // lblDVT
            // 
            lblDVT.AutoSize = true;
            lblDVT.Location = new Point(89, 32);
            lblDVT.Name = "lblDVT";
            lblDVT.Size = new Size(37, 20);
            lblDVT.TabIndex = 2;
            lblDVT.Text = "DVT";
            // 
            // lblGia
            // 
            lblGia.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblGia.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblGia.Location = new Point(405, 19);
            lblGia.Name = "lblGia";
            lblGia.Size = new Size(114, 23);
            lblGia.TabIndex = 4;
            lblGia.Text = "Gia";
            lblGia.TextAlign = ContentAlignment.MiddleRight;
            lblGia.UseWaitCursor = true;
            // 
            // btnDelete
            // 
            btnDelete.CheckedState.ImageSize = new Size(64, 64);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.HoverState.Image = (Image)resources.GetObject("resource.Image");
            btnDelete.HoverState.ImageSize = new Size(33, 33);
            btnDelete.Image = (Image)resources.GetObject("btnDelete.Image");
            btnDelete.ImageOffset = new Point(0, 0);
            btnDelete.ImageRotate = 0F;
            btnDelete.ImageSize = new Size(33, 33);
            btnDelete.Location = new Point(530, 11);
            btnDelete.Name = "btnDelete";
            btnDelete.PressedState.ImageSize = new Size(64, 64);
            btnDelete.ShadowDecoration.CustomizableEdges = customizableEdges1;
            btnDelete.Size = new Size(37, 35);
            btnDelete.TabIndex = 5;
            btnDelete.Click += btnDelete_Click;
            // 
            // numSoLuong
            // 
            numSoLuong.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numSoLuong.Location = new Point(319, 15);
            numSoLuong.Name = "numSoLuong";
            numSoLuong.Size = new Size(71, 31);
            numSoLuong.TabIndex = 6;
            numSoLuong.ValueChanged += numSoLuong_ValueChanged;
            // 
            // panelContain
            // 
            panelContain.BackColor = Color.Transparent;
            panelContain.BorderColor = Color.DimGray;
            panelContain.BorderRadius = 10;
            panelContain.BorderThickness = 1;
            panelContain.CustomizableEdges = customizableEdges2;
            panelContain.Dock = DockStyle.Fill;
            panelContain.FillColor = Color.White;
            panelContain.Location = new Point(0, 0);
            panelContain.Name = "panelContain";
            panelContain.ShadowDecoration.CustomizableEdges = customizableEdges3;
            panelContain.Size = new Size(579, 63);
            panelContain.TabIndex = 7;
            // 
            // UC_ShopPCart
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(numSoLuong);
            Controls.Add(btnDelete);
            Controls.Add(lblGia);
            Controls.Add(lblDVT);
            Controls.Add(lblTen);
            Controls.Add(picProduct);
            Controls.Add(panelContain);
            Name = "UC_ShopPCart";
            Size = new Size(579, 63);
            ((System.ComponentModel.ISupportInitialize)picProduct).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSoLuong).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picProduct;
        private Label lblTen;
        private Label lblDVT;
        private Label lblGia;
        private Guna.UI2.WinForms.Guna2ImageButton btnDelete;
        private NumericUpDown numSoLuong;
        private Guna.UI2.WinForms.Guna2Panel panelContain;
    }
}
