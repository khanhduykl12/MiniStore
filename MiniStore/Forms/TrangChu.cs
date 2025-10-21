using Guna.UI2.WinForms;
using MiniStore.UC;
using MiniStore.User_Control;

namespace MiniStore
{
    public partial class TrangChu : Form
    {
        public TrangChu()
        {
            InitializeComponent();
            UC_Dashboard uC_ = new UC_Dashboard();
            AddUserControl(uC_);
            
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

        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}