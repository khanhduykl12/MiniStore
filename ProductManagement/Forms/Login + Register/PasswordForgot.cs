using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ProductManagement
{
    public partial class PasswordForgot : Form
    {
        public PasswordForgot()
        {
            InitializeComponent();
        }

        private void btnRegisterForm_Click(object sender, EventArgs e)
        {
            validateEmail();
            bool hasErrors = string.IsNullOrWhiteSpace(txtEmail.Text);
            if (hasErrors)
            {
                MessageBox.Show("Vui lòng kiểm tra lại các thông tin đã nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string otp = generateOtp();
            Session.Email = txtEmail.Text;
            Session.OTP = otp;
            Session.Expiry = DateTime.Now.AddMinutes(5);
            try
            {
                sendVerifyCode(Session.Email, Session.OTP);
                MessageBox.Show("Mã xác thực đã được gửi đến email của bạn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                FormVerifyOTP formVerify = new FormVerifyOTP();
                this.Hide();
                formVerify.ShowDialog();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi Emaill thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PasswordForgot_Load(object sender, EventArgs e)
        {
            lblErrorEmail.Text = string.Empty;
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            validateEmail();
        }



        private void validateEmail()
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblErrorEmail.Text = "Vui lòng nhập email!";
                lblErrorEmail.ForeColor = Color.Red;
            }
            using (var db = new MiniStoreContext())
            {
                var userEmail = db.TAIKHOANs.Where(UserEmail => UserEmail.EMAIL == txtEmail.Text.Trim()).FirstOrDefault();
                if (userEmail != null)
                {
                    lblErrorEmail.Text = string.Empty;
                }
                else
                {
                    lblErrorEmail.Text = "Email không tồn tại!";
                    lblErrorEmail.ForeColor = Color.Red;
                }
            }
        }
        private string generateOtp()
        {
            Random rand = new Random();
            return rand.Next(100000, 999999).ToString();
        }
        private void sendVerifyCode(string email, string otp)
        {
            string appEmail = "lamthuan271019@gmail.com";
            string appPassword = "puhz emgf wkym gmkj";
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(appEmail);
            mail.To.Add(email);
            mail.Subject = "Mã xác thực ";
            mail.Body = $"Xin chào!\n\nMã xác thực của bạn là: {otp}\nMã này có hiệu lực trong 5 phút.";

            //conect to simple mail transfer protocol
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(appEmail, appPassword);
            smtp.EnableSsl = true;
            smtp.Send(mail);

        }

    }
}
