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
    public partial class FormVerifyOTP : Form
    {
        public FormVerifyOTP()
        {
            InitializeComponent();
        }
        private void btnRegisterForm_Click(object sender, EventArgs e)
        {
            validateXacThuc();
            bool hasError = string.IsNullOrEmpty(txtXacThuc.Text);
            if (hasError)
            {
                MessageBox.Show("Vui lòng kiểm tra lại các thông tin đã nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string inputOtp = txtXacThuc.Text.Trim();
            if (string.IsNullOrWhiteSpace(inputOtp))
            {
                lblErrorXacThuc.Text = "Vui lòng nhập mã xác thực!";
                lblErrorXacThuc.ForeColor = Color.Red;
                return;
            }
            if (inputOtp == Session.OTP && DateTime.Now <= Session.Expiry)
            {
                MessageBox.Show("Xác thực thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FormResetPass frm = new FormResetPass();
                frm.Show();
                this.Hide();
            }
            else
            {

                lblErrorXacThuc.Text = "Mã xác thực không hợp lệ hoặc đã hết hạn!";
                lblErrorXacThuc.ForeColor = Color.Red;
            }
        }

        private void txtXacThuc_TextChanged(object sender, EventArgs e)
        {
            validateXacThuc();
        }
        private void validateXacThuc()
        {
            if (string.IsNullOrWhiteSpace(txtXacThuc.Text))
            {
                lblErrorXacThuc.Text = "Vui lòng không để trống mã xác thực!";
                lblErrorXacThuc.ForeColor = Color.Red;
            }
            else
            {
                lblErrorXacThuc.Text = string.Empty;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
