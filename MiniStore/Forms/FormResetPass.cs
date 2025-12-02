using MiniStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MiniStore
{
    public partial class FormResetPass : Form
    {
        public FormResetPass()
        {
            InitializeComponent();
        }



        private void btnRegisterForm_Click(object sender, EventArgs e)
        {
            string newPass = txtPassword.Text.Trim();
            string xacThucPass = txtXacThucPassword.Text.Trim();
            if (string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(xacThucPass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (newPass != xacThucPass)
            {
                lblErrorXacThucPass.Text = "Mật khẩu không khớp!";
                lblErrorXacThucPass.ForeColor = Color.Red;
            }
            using (var db = new MiniStoreContext())
            {
                var user = db.TAIKHOANs.FirstOrDefault(u => u.EMAIL == Session.Email);
                if (user != null)
                {
                    user.PASSWORD = newPass;
                    db.SaveChanges();
                    MessageBox.Show("Đặt lại mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            ValidatePassword();
        }
        private void ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblErrorPass.Text = "Vui lòng không để trống Password!";
                lblErrorPass.ForeColor = Color.Red;
            }
            else if (txtPassword.Text.Length < 6)
            {
                lblErrorPass.Text = "Mật khẩu phải có ít nhất 6 ký tự!";
                lblErrorPass.ForeColor = Color.Red;
            }
            else if (!txtPassword.Text.Any(char.IsUpper))
            {
                lblErrorPass.Text = "Mật khẩu phải có ký tự in hoa!";
                lblErrorPass.ForeColor = Color.Red;
            }
            else if (!txtPassword.Text.Any(char.IsDigit))
            {
                lblErrorPass.Text = "Mật khẩu phải có chữ số!";
                lblErrorPass.ForeColor = Color.Red;
            }
            else if (!txtPassword.Text.Any(c => !char.IsLetterOrDigit(c)))
            {
                lblErrorPass.Text = "Mật khẩu phải có ký tự đặc biệt!";
                lblErrorPass.ForeColor = Color.Red;
            }
            else
            {
                lblErrorPass.Text = string.Empty;
            }
        }

        private void FormResetPass_Load(object sender, EventArgs e)
        {
            lblErrorPass.Text=string.Empty;
            lblErrorXacThucPass.Text=string.Empty;
        }
    }
}
