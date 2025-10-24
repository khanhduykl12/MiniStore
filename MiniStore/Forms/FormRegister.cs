using Guna.UI2.WinForms;
using MiniStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace MiniStore
{
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormRegister_Load(object sender, EventArgs e)
        {
            lblErrorEmail.Text = string.Empty;
            lblErrorPassword.Text = string.Empty;
            lblErrorXacNhanPass.Text = string.Empty;
            lblErrorEmail.Text = string.Empty;
            lblErrorGender.Text = string.Empty;
            lblErrorNgayThangNamSinh.Text = string.Empty;
            lblErrorSDT.Text = string.Empty;
            lblErrorUserName.Text = string.Empty;
            lblErrorName.Text = string.Empty;
        }

        private void btnRegisterForm_Click(object sender, EventArgs e)
        {
            // Validate tất cả các trường trước khi xử lý
            ValidateName();
            ValidateEmail();
            ValidatePhone();
            ValidateUserName();
            ValidatePassword();
            ValidateConfirmPassword();
            ValidateAddress();
            ValidateGender();
            ValidateDateOfBirth();

            // Kiểm tra xem có lỗi nào không
            bool hasErrors = !string.IsNullOrWhiteSpace(lblErrorName.Text) ||
                           !string.IsNullOrWhiteSpace(lblErrorEmail.Text) ||
                           !string.IsNullOrWhiteSpace(lblErrorSDT.Text) ||
                           !string.IsNullOrWhiteSpace(lblErrorUserName.Text) ||
                           !string.IsNullOrWhiteSpace(lblErrorPassword.Text) ||
                           !string.IsNullOrWhiteSpace(lblErrorXacNhanPass.Text) ||
                           !string.IsNullOrWhiteSpace(lblErrorAddress.Text) ||
                           !string.IsNullOrWhiteSpace(lblErrorGender.Text) ||
                           !string.IsNullOrWhiteSpace(lblErrorNgayThangNamSinh.Text);

            if (hasErrors)
            {
                MessageBox.Show("Vui lòng kiểm tra lại các thông tin đã nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = new MiniStoreContext())
            {
                string hoTen = txtName.Text;
                string email = txtEmail.Text;
                string sodienthoai = txtPhone.Text;
                string userName = txtUserName.Text;
                string password = txtPassWord.Text;
                string role = "";
                decimal luong = 0;
                string chucVu = "";
                DateOnly? ngaysinh = DateOnly.FromDateTime(guna2DateTimePicker1.Value);
                string gioiTinh = rdNam.Checked ? "Nam" : "Nữ";

                string[] part = email.Split('@');
                if (part[1] == "company.com" && part.Length == 2)
                {
                    if (part[0].StartsWith("admin"))
                    {
                        role = "ADMIN";
                    }
                    else
                    {
                        role = "NV";
                    }
                    FormLuongChucVu formluong = new FormLuongChucVu();
                    this.Hide();
                    formluong.ShowDialog();
                    this.Show();
                    luong = formluong.Luong_formLuongChucVu;
                    chucVu = formluong.ChucVu_formLuongChucVu;
                }
                else
                {
                    role = "KH";
                }
                TAIKHOAN taikhoan = new TAIKHOAN()
                {
                    USERNAME = userName,
                    PASSWORD = password,
                    MAROLE = role,
                    EMAIL = email,
                    TRANGTHAI = "Hoạt động"
                };
                db.TAIKHOANs.Add(taikhoan);
                NGUOIDUNG nguoidung = new NGUOIDUNG()
                {
                    USERNAME = userName,
                    HOTEN = hoTen,
                    NGAYSINH = ngaysinh,
                    GioiTinh = gioiTinh,
                    SDT = sodienthoai,
                    DIACHI = txtAddress.Text,
                    MAROLE = role,
                    LUONG = luong,
                    CHUCVU = chucVu,
                    DIEMTICHLUY = 0
                };
                db.NGUOIDUNGs.Add(nguoidung);
                db.SaveChanges();

                MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

            }

        }
        bool isDateSelected = false;
        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            isDateSelected = true;
            ValidateDateOfBirth();
        }
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            ValidateName();
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            ValidateEmail();
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            ValidatePhone();
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            ValidateUserName();
        }

        private void txtPassWord_TextChanged(object sender, EventArgs e)
        {
            ValidatePassword();
        }

        private void txtXacNhanPass_TextChanged(object sender, EventArgs e)
        {
            ValidateConfirmPassword();
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            ValidateAddress();
        }

        private void rdNam_CheckedChanged(object sender, EventArgs e)
        {
            ValidateGender();
        }

        private void rdNu_CheckedChanged(object sender, EventArgs e)
        {
            ValidateGender();
        }

        // Các phương thức validation riêng biệt
        private void ValidateName()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                lblErrorName.Text = "Vui lòng không để trống họ và tên!";
                lblErrorName.ForeColor = Color.Red;
            }
            else
            {
                lblErrorName.Text = string.Empty;
            }
        }

        private void ValidateEmail()
        {
            string email = txtEmail.Text;
            if (string.IsNullOrWhiteSpace(email))
            {
                lblErrorEmail.Text = "Vui lòng không để trống email!";
                lblErrorEmail.ForeColor = Color.Red;
            }
            else if (!IsValidEmail(email))
            {
                lblErrorEmail.Text = "Email không hợp lệ!";
                lblErrorEmail.ForeColor = Color.Red;
            }
            else
            {
                lblErrorEmail.Text = string.Empty;
            }
        }

        private void ValidatePhone()
        {
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                lblErrorSDT.Text = "Vui lòng không để trống số điện thoại!";
                lblErrorSDT.ForeColor = Color.Red;
            }
            else if (!Regex.IsMatch(txtPhone.Text, @"^\d{10}$"))
            {
                lblErrorSDT.Text = "SDT chỉ được chứa số và không quá 10 ký tự";
                lblErrorSDT.ForeColor = Color.Red;
            }
            else
            {
                lblErrorSDT.Text = string.Empty;
            }
        }

        private void ValidateUserName()
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                lblErrorUserName.Text = "Vui lòng không để trống User Name!";
                lblErrorUserName.ForeColor = Color.Red;
            }
            else
            {
                using (var db = new MiniStoreContext())
                {
                    if (db.TAIKHOANs.Any(name => name.USERNAME == txtUserName.Text))
                    {
                        lblErrorUserName.Text = "Tên user đã tồn tại";
                        lblErrorUserName.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblErrorUserName.Text = string.Empty;
                    }
                }
            }
        }

        private void ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(txtPassWord.Text))
            {
                lblErrorPassword.Text = "Vui lòng không để trống Password!";
                lblErrorPassword.ForeColor = Color.Red;
            }
            else if (txtPassWord.Text.Length < 6)
            {
                lblErrorPassword.Text = "Mật khẩu phải có ít nhất 6 ký tự!";
                lblErrorPassword.ForeColor = Color.Red;
            }
            else if (!txtPassWord.Text.Any(char.IsUpper))
            {
                lblErrorPassword.Text = "Mật khẩu phải có ký tự in hoa!";
                lblErrorPassword.ForeColor = Color.Red;
            }
            else if (!txtPassWord.Text.Any(char.IsDigit))
            {
                lblErrorPassword.Text = "Mật khẩu phải có chữ số!";
                lblErrorPassword.ForeColor = Color.Red;
            }
            else if (!txtPassWord.Text.Any(c => !char.IsLetterOrDigit(c)))
            {
                lblErrorPassword.Text = "Mật khẩu phải có ký tự đặc biệt!";
                lblErrorPassword.ForeColor = Color.Red;
            }
            else
            {
                lblErrorPassword.Text = string.Empty;
            }
            ValidateConfirmPassword();
        }

        private void ValidateConfirmPassword()
        {
            if (string.IsNullOrWhiteSpace(txtXacNhanPass.Text))
            {
                lblErrorXacNhanPass.Text = "Vui lòng không để trống xác nhận password!";
                lblErrorXacNhanPass.ForeColor = Color.Red;
            }
            else if (txtXacNhanPass.Text != txtPassWord.Text)
            {
                lblErrorXacNhanPass.Text = "Mật khẩu không khớp!";
                lblErrorXacNhanPass.ForeColor = Color.Red;
            }
            else
            {
                lblErrorXacNhanPass.Text = string.Empty;
            }
        }

        private void ValidateAddress()
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                lblErrorAddress.Text = "Vui lòng không để trống địa chỉ";
                lblErrorAddress.ForeColor = Color.Red;
            }
            else
            {
                lblErrorAddress.Text = string.Empty;
            }
        }

        private void ValidateGender()
        {
            if (!rdNam.Checked && !rdNu.Checked)
            {
                lblErrorGender.Text = "Vui lòng chọn giới tính của bạn đi";
                lblErrorGender.ForeColor = Color.Red;
            }
            else
            {
                lblErrorGender.Text = string.Empty;
            }
        }

        private void ValidateDateOfBirth()
        {
            if (!isDateSelected)
            {
                lblErrorNgayThangNamSinh.Text = "Bạn chưa chọn ngày tháng năm sinh kìa";
                lblErrorNgayThangNamSinh.ForeColor = Color.Red;
            }
            else
            {
                int age = DateTime.Now.Year - guna2DateTimePicker1.Value.Year;
                if (age > 100)
                {
                    lblErrorNgayThangNamSinh.Text = $"Bạn có chọn lộn không bạn: {age} à";
                    lblErrorNgayThangNamSinh.ForeColor = Color.Red;
                }
                else
                {
                    lblErrorNgayThangNamSinh.Text = string.Empty;
                }
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

      
    }
}
