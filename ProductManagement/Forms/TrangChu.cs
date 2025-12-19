using Guna.UI2.WinForms;
using System;
using MiniShop.User_Control;
using MiniStore.UC;
using MiniStore.User_Control;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace MiniStore
{
    public partial class TrangChu : Form
    {
        private readonly Dictionary<string, UserControl> _controlCache = new();
        private bool _initialControlLoaded;
        public string userRole { get; set; }

        public TrangChu(string role)
        {
            InitializeComponent();
            userRole = role;
           
            btnRevenue.Visible = false;
            btnSettingListAcc.Visible = false;

            if (role == "ADMIN")
            {
                
                btnRevenue.Visible = true;
                btnSettingListAcc.Visible = true;
            }
            else if (role == "NV")
            {
                btnRevenue.Visible = true;
                
            }

            EnableDoubleBufferingForContainer();
            this.AutoScaleMode = AutoScaleMode.Dpi;
        }

        protected override CreateParams CreateParams
        {
            get { var cp = base.CreateParams; cp.ExStyle |= 0x02000000; return cp; }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (!_initialControlLoaded)
            {
                ShowControl("dashboard", () => new UC_Dashboard(userRole));
                _initialControlLoaded = true;
            }
        }
        private void moveSlide(object sender)
        {
            Guna2Button b = (Guna2Button)sender;
            imgSlide.Location = new Point(b.Location.X + 42, b.Location.Y - 42);
            imgSlide.SendToBack();

        }
        private void EnableDoubleBufferingForContainer()
        {
            // reduce flicker when switching controls
            typeof(Panel)
                .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(panelContainer, true);
        }

        private void ShowControl(string key, Func<UserControl> factory)
        {
            if (!_controlCache.TryGetValue(key, out var uc))
            {
                uc = factory();
                uc.Dock = DockStyle.Fill;
                panelContainer.Controls.Add(uc);
                _controlCache[key] = uc;
            }

            panelContainer.SuspendLayout();
            foreach (Control c in panelContainer.Controls)
            {
                c.Visible = false;
            }

            uc.Visible = true;
            uc.BringToFront();
            panelContainer.ResumeLayout();
        }
        private void btnHome_CheckedChanged(object sender, EventArgs e)
        {
            moveSlide(sender);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {

            ShowControl("dashboard", () => new UC_Dashboard(userRole));
        }
        private void btnProduct_Click(object sender, EventArgs e)
        {
            ShowControl("product", () => new UC_Product(userRole));
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            // Ẩn TrangChu trước để tránh hiển thị cả hai form cùng lúc
            this.Hide();
            
            // Tìm FormLogin trong các form đang mở (bao gồm cả form bị ẩn)
            FormLogin loginForm = null;
            foreach (Form form in Application.OpenForms)
            {
                if (form is FormLogin)
                {
                    loginForm = form as FormLogin;
                    break;
                }
            }
            
            // Nếu không tìm thấy FormLogin, tạo mới
            if (loginForm == null)
            {
                loginForm = new FormLogin();
            }
            
            // Đảm bảo FormLogin được hiển thị và active
            loginForm.Show();
            loginForm.WindowState = FormWindowState.Normal;
            loginForm.Visible = true;
            loginForm.Activate();
            loginForm.BringToFront();
            loginForm.Focus();
            
            // Đóng TrangChu - điều này sẽ làm ShowDialog() trong FormLogin trả về
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

       

        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            moveSlide(sender);
            ShowControl("settingAccount", () => new UC_settingAccount());
        }



        private void btnSettingListAcc_Click(object sender, EventArgs e)
        {
            moveSlide(sender);
            ShowControl("settingAccount", () => new UC_settingAccount());
        }

        private void btnRevenue_Click_1(object sender, EventArgs e)
        {
            
        }
    }
}