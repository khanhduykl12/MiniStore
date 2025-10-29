namespace MiniStore.User_Control._UC
{
    partial class UC_ProductCard
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
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            lblGia = new Label();
            sANPHAMBindingSource = new BindingSource(components);
            lblTenSP = new Label();
            picProduct = new Guna.UI2.WinForms.Guna2PictureBox();
            guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)sANPHAMBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picProduct).BeginInit();
            SuspendLayout();
            // 
            // guna2Panel1
            // 
            guna2Panel1.BackColor = Color.Transparent;
            guna2Panel1.BorderRadius = 20;
            guna2Panel1.Controls.Add(lblGia);
            guna2Panel1.Controls.Add(lblTenSP);
            guna2Panel1.Controls.Add(picProduct);
            guna2Panel1.CustomizableEdges = customizableEdges3;
            guna2Panel1.Dock = DockStyle.Fill;
            guna2Panel1.Location = new Point(0, 0);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.Padding = new Padding(14, 14, 14, 12);
            guna2Panel1.ShadowDecoration.BorderRadius = 20;
            guna2Panel1.ShadowDecoration.Color = Color.FromArgb(248, 252, 250);
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Panel1.ShadowDecoration.Depth = 10;
            guna2Panel1.ShadowDecoration.Enabled = true;
            guna2Panel1.ShadowDecoration.Shadow = new Padding(0, 2, 6, 6);
            guna2Panel1.Size = new Size(227, 250);
            guna2Panel1.TabIndex = 0;
            guna2Panel1.Paint += guna2Panel1_Paint;
            // 
            // lblGia
            // 
            lblGia.AutoSize = true;
            lblGia.DataBindings.Add(new Binding("Text", sANPHAMBindingSource, "GIABAN", true));
            lblGia.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblGia.Location = new Point(79, 195);
            lblGia.Name = "lblGia";
            lblGia.Size = new Size(59, 23);
            lblGia.TabIndex = 2;
            lblGia.Text = "label1";
            // 
            // sANPHAMBindingSource
            // 
            sANPHAMBindingSource.DataSource = typeof(Models.SANPHAM);
            // 
            // lblTenSP
            // 
            lblTenSP.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblTenSP.AutoSize = true;
            lblTenSP.DataBindings.Add(new Binding("Text", sANPHAMBindingSource, "TENSP", true));
            lblTenSP.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTenSP.Location = new Point(14, 138);
            lblTenSP.Name = "lblTenSP";
            lblTenSP.Size = new Size(65, 28);
            lblTenSP.TabIndex = 1;
            lblTenSP.Text = "label1";
            lblTenSP.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picProduct
            // 
            picProduct.CustomizableEdges = customizableEdges1;
            picProduct.DataBindings.Add(new Binding("Image", sANPHAMBindingSource, "HINH", true));
            picProduct.Dock = DockStyle.Top;
            picProduct.ImageRotate = 0F;
            picProduct.Location = new Point(14, 14);
            picProduct.Name = "picProduct";
            picProduct.ShadowDecoration.CustomizableEdges = customizableEdges2;
            picProduct.Size = new Size(199, 121);
            picProduct.SizeMode = PictureBoxSizeMode.Zoom;
            picProduct.TabIndex = 0;
            picProduct.TabStop = false;
            // 
            // UC_ProductCard
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(guna2Panel1);
            Name = "UC_ProductCard";
            Size = new Size(227, 250);
            Load += UC_ProductCard_Load;
            guna2Panel1.ResumeLayout(false);
            guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)sANPHAMBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)picProduct).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private BindingSource sANPHAMBindingSource;
        private Guna.UI2.WinForms.Guna2PictureBox picProduct;
        private Label lblTenSP;
        private Label lblGia;
    }
}
