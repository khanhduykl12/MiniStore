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
            btnRevenue.Visible = (role == "ADMIN");

            UC_Dashboard uC_ = new UC_Dashboard();
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

            UC_Dashboard uC_ = new UC_Dashboard();
            AddUserControl(uC_);
        }
        private void btnProduct_Click(object sender, EventArgs e)
        {
            UC_Product uC_ = new UC_Product();
            AddUserControl(uC_);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRevenue_Click(object sender, EventArgs e)
        {
            UC_ThongKe uc=new UC_ThongKe();
            AddUserControl(uc);
        }
    }
}