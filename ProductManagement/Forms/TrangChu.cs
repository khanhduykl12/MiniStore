using Guna.UI2.WinForms;
using MiniShop.User_Control;
using MiniStore.UC;
using MiniStore.User_Control;
using System.Data;

namespace MiniStore
{
    public partial class TrangChu : Form
    {
        public string userRole { get; set; }

        public TrangChu(string role)
        {
            InitializeComponent();
            userRole = role;
            btnStaff.Visible = false;
            btnRevenue.Visible = false;
            btnSettingListAcc.Visible = false;

            if (role == "ADMIN")
            {
                btnStaff.Visible = true;
                btnRevenue.Visible = true;
                btnSettingListAcc.Visible = true;
            }
            else if (role == "NV")
            {
                btnRevenue.Visible = true;
                btnRevenue.Location = btnStaff.Location;
            }

            UC_Dashboard uC_ = new UC_Dashboard(userRole);
            AddUserControl(uC_);
            this.AutoScaleMode = AutoScaleMode.Dpi;
        }

        protected override CreateParams CreateParams
        {
            get { var cp = base.CreateParams; cp.ExStyle |= 0x02000000; return cp; }
        }
        private void moveSlide(object sender)
        {
            Guna2Button b = (Guna2Button)sender;
            imgSlide.Location = new Point(b.Location.X + 42, b.Location.Y - 42);
            imgSlide.SendToBack();

        }
        private void AddUserControl(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            panelContainer.Controls.Clear();
            uc.BringToFront();
            panelContainer.Controls.Add(uc);
        }
        private void btnHome_CheckedChanged(object sender, EventArgs e)
        {
            moveSlide(sender);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {

            UC_Dashboard uC_ = new UC_Dashboard(userRole);
            AddUserControl(uC_);
        }
        private void btnProduct_Click(object sender, EventArgs e)
        {
            UC_Product uC_ = new UC_Product(userRole);
            AddUserControl(uC_);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       

        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            moveSlide(sender);
            UC_settingAccount uC_SettingAccount = new UC_settingAccount();
            AddUserControl(uC_SettingAccount);
        }



        private void btnSettingListAcc_Click(object sender, EventArgs e)
        {
            moveSlide(sender);
            UC_settingAccount uC_SettingAccount = new UC_settingAccount();
            AddUserControl(uC_SettingAccount);
        }

        private void btnRevenue_Click_1(object sender, EventArgs e)
        {
            UC_ThongKe uc = new UC_ThongKe();
            AddUserControl(uc);
        }
    }
}