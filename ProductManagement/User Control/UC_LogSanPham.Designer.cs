namespace MiniShop.User_Control
{
    partial class UC_LogThemSanPham
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            flpLogEntries = new FlowLayoutPanel();
            guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            btnBack = new Guna.UI2.WinForms.Guna2Button();
            SuspendLayout();
            // 
            // flpLogEntries
            // 
            flpLogEntries.AutoScroll = true;
            flpLogEntries.BackColor = Color.WhiteSmoke;
            flpLogEntries.Location = new Point(0, 98);
            flpLogEntries.Margin = new Padding(5, 4, 5, 4);
            flpLogEntries.Name = "flpLogEntries";
            flpLogEntries.Padding = new Padding(11, 13, 11, 13);
            flpLogEntries.Size = new Size(1311, 790);
            flpLogEntries.TabIndex = 0;
            // 
            // guna2HtmlLabel1
            // 
            guna2HtmlLabel1.BackColor = Color.Transparent;
            guna2HtmlLabel1.Font = new Font("Cambria", 22.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            guna2HtmlLabel1.Location = new Point(488, 18);
            guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            guna2HtmlLabel1.Size = new Size(360, 45);
            guna2HtmlLabel1.TabIndex = 1;
            guna2HtmlLabel1.Text = "NHẬT KÝ HOẠT ĐỘNG";
            // 
            // btnBack
            // 
            btnBack.Animated = true;
            btnBack.BorderRadius = 14;
            btnBack.CustomizableEdges = customizableEdges1;
            btnBack.DisabledState.BorderColor = Color.DarkGray;
            btnBack.DisabledState.CustomBorderColor = Color.DarkGray;
            btnBack.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnBack.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnBack.FillColor = Color.FromArgb(6, 76, 80);
            btnBack.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(18, 18);
            btnBack.Name = "btnBack";
            btnBack.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnBack.Size = new Size(120, 40);
            btnBack.TabIndex = 2;
            btnBack.Text = "Quay lại";
            btnBack.Click += btnBack_Click;
            // 
            // UC_LogThemSanPham
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnBack);
            Controls.Add(guna2HtmlLabel1);
            Controls.Add(flpLogEntries);
            Margin = new Padding(5, 4, 5, 4);
            Name = "UC_LogThemSanPham";
            Size = new Size(1345, 927);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpLogEntries;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2Button btnBack;
    }
}
