using Guna.UI2.WinForms;
using Microsoft.VisualBasic.ApplicationServices;
using MiniStore.Models;
namespace MiniStore
{
    public partial class FormLogin : Form
    {
        
        public FormLogin()
        {
            InitializeComponent();
          
        }
        int count = 3;
        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            validateUserName();
            validatePassWord();
            bool hasErrors = !string.IsNullOrWhiteSpace(lblErrorUserName.Text) ||
                     !string.IsNullOrWhiteSpace(lblErrorPassword.Text);
            if (hasErrors)
            {
                MessageBox.Show("Vui lòng kiểm tra lại các thông tin đã nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = new MiniStoreContext())
            {
                var user = db.TAIKHOANs.Where(username => username.USERNAME == txtUserName.Text).FirstOrDefault();
                user.NGAYKHOA = DateOnly.FromDateTime(DateTime.Now.Date);
                user.NGAYMOKHOA = user.NGAYKHOA.Value.AddDays(1);
                if (user == null)
                {
                    lblErrorUserName.Text = "Tài khoản không tồn tại";
                    lblErrorUserName.ForeColor = Color.Red;
                    return;
                }
                else
                {

                    if (user.PASSWORD != txtPassWord.Text)
                    {
                        lblErrorPassword.Text = $"password không khớp tài khoản sẽ bị khóa trong {count} lần nhập sai ";
                        lblErrorPassword.ForeColor = Color.Red;
                        lblQuenMatKhau.Text = "Bạn quên mật khẩu hả?";
                        lblQuenMatKhau.ForeColor = Color.Blue;
                        if (count == 0)
                        {
                            MessageBox.Show($"Tài khoản của bạn đã bị khóa cho đến hết ngày {user.NGAYMOKHOA.Value}", "Thông báo tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            user.TRANGTHAI = "Khóa";
                            db.SaveChanges();
                            return;
                        }
                        count--;
                    }
                }
                if (user.MAROLE == "ADMIN" || user.MAROLE =="NV" || user.MAROLE == "KH")
                {
                    TrangChu tc = new TrangChu();
                    this.Hide();
                    tc.ShowDialog();
                    this.Show();
                    
                }
               
            }
            
            
        }

        private void btnRegisterForm_Click(object sender, EventArgs e)
        {
            FormRegister reg = new FormRegister();
            this.Hide();
            reg.ShowDialog();
            this.Show();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblQuenMatKhau_Click(object sender, EventArgs e)
        {
            PasswordForgot passwordForgot = new PasswordForgot();
            this.Hide();
            passwordForgot.ShowDialog();
            this.Show();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            lblErrorUserName.Text = string.Empty;
            lblErrorPassword.Text = string.Empty;
            lblQuenMatKhau.Text = string.Empty;
        }

        private void txtUserName_Leave(object sender, EventArgs e)
        {
            validateUserName();
        }

        private void txtPassWord_TextChanged(object sender, EventArgs e)
        {
            validatePassWord();
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            // Enable txtPassWord khi userName thay đổi
            txtPassWord.Enabled = true;
            // Xóa error message của userName khi có thay đổi
            lblErrorUserName.Text = string.Empty;
        }
    

        private void validateUserName()
        {
            using (var db = new MiniStoreContext())
            {
                var user = db.TAIKHOANs.Where(username => username.USERNAME == txtUserName.Text).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(txtUserName.Text))
                {
                    lblErrorUserName.Text = "Vui lòng không để trống username!";
                    lblErrorUserName.ForeColor = Color.Red;
                }
                else if (user == null)
                {
                    lblErrorUserName.Text = "Tài khoản không tồn tại";
                    lblErrorUserName.ForeColor = Color.Red;
                }
                else if (user.TRANGTHAI == "Khóa")
                {

                    if (user.NGAYKHOA.HasValue && DateOnly.FromDateTime(DateTime.Now) >= user.NGAYMOKHOA.Value)
                    {
                        user.TRANGTHAI = "Hoạt động";
                        user.NGAYKHOA = null;
                        user.NGAYMOKHOA = null;
                        db.SaveChanges();

                        lblErrorUserName.Text = string.Empty;
                        txtPassWord.Enabled = true;
                    }
                    else
                    {
                        lblErrorUserName.Text = $"Tài khoản của bạn bị khóa đến hết ngày {user.NGAYMOKHOA:dd/MM/yyyy}";
                        lblErrorUserName.ForeColor = Color.Red;
                        txtPassWord.Enabled = false;

                    }
                }
                else
                {
                    lblErrorUserName.Text = string.Empty;
                }

            }
        }

        private void validatePassWord()
        {

            if (string.IsNullOrWhiteSpace(txtPassWord.Text))
            {
                lblErrorPassword.Text = "Vui lòng không để trống password!";
                lblErrorPassword.ForeColor = Color.Red;
            }
            else
            {
                lblErrorPassword.Text = string.Empty;
            }
        }

       
    }
}
