using Guna.UI2.WinForms;

namespace MiniStore
{
    public partial class TrangChu : Form
    {
        public TrangChu()
        {
            InitializeComponent();
        }
        private void moveSlide(object sender)
        {
            Guna2Button b = (Guna2Button)sender;
            imgSlide.Location = new Point(b.Location.X + 42, b.Location.Y - 42);
            imgSlide.SendToBack();
        }
        private void btnHome_CheckedChanged(object sender, EventArgs e)
        {
            moveSlide(sender);
        }
    }
}
