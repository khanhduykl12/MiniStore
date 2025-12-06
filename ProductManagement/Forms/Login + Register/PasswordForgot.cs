using MiniStore.Models;
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
using MiniStore.Class;

namespace MiniStore
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
            bool hasErrors = string.IsNullOrWhiteSpace(txtEmail.Text) || 
                            !string.IsNullOrWhiteSpace(lblErrorEmail.Text);
            if (hasErrors)
            {
                MessageBox.Show("Vui lòng kiểm tra lại các thông tin đã nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string otp = generateOtp();
            Session.Email = txtEmail.Text.Trim();
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
                MessageBox.Show("Gửi Email thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                return; // Dừng lại nếu email rỗng, không kiểm tra database
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
            // LƯU Ý: Để sử dụng Gmail SMTP, bạn cần:
            // 1. Bật 2-Step Verification cho tài khoản Gmail
            // 2. Tạo App Password tại: https://myaccount.google.com/apppasswords
            // 3. Sử dụng App Password (16 ký tự, không có dấu cách) thay vì mật khẩu thông thường
            
            string appEmail = "lamthuan271019@gmail.com";
            string appPassword = "emxp ylnx utye anlg"; // Loại bỏ dấu cách trong App Password
            
            // Cấu hình bảo mật TLS/SSL
            System.Net.ServicePointManager.SecurityProtocol = 
                System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
            
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(appEmail, "MiniStore - Hệ thống quản lý");
                mail.To.Add(email);
                mail.Subject = "Mã xác thực đặt lại mật khẩu";
                mail.Body = $"Xin chào!\n\nMã xác thực của bạn là: {otp}\nMã này có hiệu lực trong 5 phút.\n\nVui lòng không chia sẻ mã này với bất kỳ ai.\n\nTrân trọng,\nĐội ngũ MiniStore";
                mail.IsBodyHtml = false;
                mail.Priority = MailPriority.Normal;

                bool emailSent = false;
                Exception lastException = null;
                string errorDetails = "";

                // Thử gửi với port 587 (TLS) trước
                try
                {
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(appEmail, appPassword);
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Timeout = 60000; // Tăng timeout lên 60 giây
                        
                        smtp.Send(mail);
                        emailSent = true;
                    }
                }
                catch (SmtpException smtpEx)
                {
                    lastException = smtpEx;
                    errorDetails = $"SMTP Error: {smtpEx.StatusCode} - {smtpEx.Message}";
                    
                    // Nếu port 587 thất bại, thử port 465 (SSL)
                    try
                    {
                        using (SmtpClient smtp465 = new SmtpClient("smtp.gmail.com", 465))
                        {
                            smtp465.UseDefaultCredentials = false;
                            smtp465.Credentials = new NetworkCredential(appEmail, appPassword);
                            smtp465.EnableSsl = true;
                            smtp465.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp465.Timeout = 60000;
                            
                            smtp465.Send(mail);
                            emailSent = true;
                        }
                    }
                    catch (Exception ex465)
                    {
                        lastException = ex465;
                        errorDetails += $"\nPort 465 Error: {ex465.Message}";
                    }
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    errorDetails = $"General Error: {ex.Message}";
                    
                    // Thử port 465 như fallback
                    try
                    {
                        using (SmtpClient smtp465 = new SmtpClient("smtp.gmail.com", 465))
                        {
                            smtp465.UseDefaultCredentials = false;
                            smtp465.Credentials = new NetworkCredential(appEmail, appPassword);
                            smtp465.EnableSsl = true;
                            smtp465.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp465.Timeout = 60000;
                            
                            smtp465.Send(mail);
                            emailSent = true;
                        }
                    }
                    catch (Exception ex465)
                    {
                        lastException = ex465;
                        errorDetails += $"\nPort 465 Error: {ex465.Message}";
                    }
                }

                // Nếu cả hai cách đều thất bại, throw exception với thông tin chi tiết
                if (!emailSent)
                {
                    string fullError = $"Không thể gửi email.\n\n" +
                                     $"Chi tiết lỗi: {errorDetails}\n\n" +
                                     $"Nguyên nhân có thể:\n" +
                                     $"1. App Password không đúng hoặc đã hết hạn\n" +
                                     $"2. Chưa bật 2-Step Verification\n" +
                                     $"3. Tài khoản Gmail bị hạn chế\n\n" +
                                     $"Vui lòng:\n" +
                                     $"1. Kiểm tra và tạo App Password mới tại: https://myaccount.google.com/apppasswords\n" +
                                     $"2. Đảm bảo đã bật 2-Step Verification\n" +
                                     $"3. Cập nhật App Password trong code (dòng 114)";
                    
                    throw new Exception(fullError);
                }
            }
        }

    }
}
