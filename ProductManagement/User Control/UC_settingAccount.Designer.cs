namespace MiniShop.User_Control
{
    partial class UC_settingAccount
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
            guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            flpSettingAcc = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // guna2HtmlLabel1
            // 
            guna2HtmlLabel1.BackColor = Color.Transparent;
            guna2HtmlLabel1.Font = new Font("Cambria", 22.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            guna2HtmlLabel1.Location = new Point(508, 39);
            guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            guna2HtmlLabel1.Size = new Size(344, 45);
            guna2HtmlLabel1.TabIndex = 0;
            guna2HtmlLabel1.Text = "QUẢN LÝ TÀI KHOẢN";
            // 
            // flpSettingAcc
            // 
            flpSettingAcc.AutoScroll = true;
            flpSettingAcc.Location = new Point(58, 113);
            flpSettingAcc.Name = "flpSettingAcc";
            flpSettingAcc.Size = new Size(1254, 811);
            flpSettingAcc.TabIndex = 1;
            // 
            // UC_settingAccount
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(flpSettingAcc);
            Controls.Add(guna2HtmlLabel1);
            Name = "UC_settingAccount";
            Size = new Size(1345, 927);
            Load += UC_settingAccount_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private FlowLayoutPanel flpSettingAcc;
    }
}
