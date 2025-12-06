using MiniStore.Models;
using MiniShop.Forms_Extra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniShop.User_Control
{
    public partial class UC_settingAccount : UserControl
    {
        public UC_settingAccount()
        {
            InitializeComponent();
        }

        private void UC_settingAccount_Load(object sender, EventArgs e)
        {
            using (var db = new MiniStoreContext())
            {
                var users = db.TAIKHOANs.ToList();
                foreach (var u in users)
                {
                    flpSettingAcc.Controls.Add(CreateAccountPanel(u));
                }
            }
        }
        private Guna.UI2.WinForms.Guna2Panel CreateAccountPanel(TAIKHOAN user)
        {
            var panel = new Guna.UI2.WinForms.Guna2Panel();
            panel.Width = flpSettingAcc.Width - 30;
            panel.Height = 100;
            panel.BorderRadius = 10;
            panel.FillColor = Color.White;
            panel.ShadowDecoration.Enabled = true;
            panel.ShadowDecoration.Shadow = new Padding(15, 0, 0, 10);
            panel.Margin = new Padding(20);

            // Label tên user
            var lblUser = new Label();
            lblUser.Text = $"User: {user.USERNAME}";
            lblUser.Font = new Font("Inter", 12, FontStyle.Bold);
            lblUser.Location = new Point(15, 10);
            lblUser.AutoSize = true;

            // Email
            var lblEmail = new Label();
            lblEmail.Text = $"Email: {user.EMAIL}";
            lblEmail.Font = new Font("Inter", 10);
            lblEmail.Location = new Point(15, 40);
            lblEmail.AutoSize = true;

            // Trạng thái
            var lblStatus = new Label();
            string statusText = user.TRANGTHAI ?? "Chưa xác định";
            if (user.TRANGTHAI == "Khóa vĩnh viễn")
            {
                statusText = "Khóa vĩnh viễn";
            }
            else if (user.TRANGTHAI == "Khóa" && user.NGAYKHOA.HasValue && user.NGAYMOKHOA.HasValue)
            {
                statusText = $"Khóa đến {user.NGAYMOKHOA.Value:dd/MM/yyyy}";
            }
            lblStatus.Text = $"Trạng thái: {statusText}";
            lblStatus.Font = new Font("Inter", 10);
            lblStatus.Location = new Point(15, 70);
            lblStatus.AutoSize = true;

            // Button hành động
            var btnAction = new Guna.UI2.WinForms.Guna2Button();
            btnAction.Width = 120;
            btnAction.Height = 40;
            btnAction.BorderRadius = 8;
            btnAction.Location = new Point(panel.Width - 150, 30);

            if (user.TRANGTHAI == "Hoạt động")
            {
                btnAction.Text = "Khóa";
                btnAction.FillColor = Color.Red;
                btnAction.Tag = user;  // IMPORTANT: lưu user vào Tag
                btnAction.Click += BtnBan_Click;
            }
            else if (user.TRANGTHAI == "Khóa" || user.TRANGTHAI == "Khóa vĩnh viễn")
            {
                btnAction.Text = "Mở khóa";
                btnAction.FillColor = Color.Green;
                btnAction.Tag = user;
                btnAction.Click += BtnUnban_Click;
            }

            // Add controls vào panel
            panel.Controls.Add(lblUser);
            panel.Controls.Add(lblEmail);
            panel.Controls.Add(lblStatus);
            panel.Controls.Add(btnAction);

            return panel;
        }
        private void BtnBan_Click(object sender, EventArgs e)
        {
            var btn = (Guna.UI2.WinForms.Guna2Button)sender;
            var user = (TAIKHOAN)btn.Tag;

            // Hiển thị form dialog để chọn loại khóa
            using (var formLock = new FormLockAccount())
            {
                if (formLock.ShowDialog() == DialogResult.OK)
                {
                    using (var db = new MiniStoreContext())
                    {
                        var u = db.TAIKHOANs.Find(user.USERNAME);

                        if (formLock.IsPermanentLock)
                        {
                            // Khóa vĩnh viễn
                            u.TRANGTHAI = "Khóa vĩnh viễn";
                            u.NGAYKHOA = DateOnly.FromDateTime(DateTime.Now);
                            u.NGAYMOKHOA = null; // Không có ngày mở khóa cho khóa vĩnh viễn
                            
                            db.SaveChanges();
                            MessageBox.Show($"Đã khóa vĩnh viễn tài khoản {user.USERNAME}!\nTài khoản này sẽ không thể đăng nhập cho đến khi được mở khóa bởi quản trị viên.", 
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            // Khóa tạm thời
                            u.TRANGTHAI = "Khóa";
                            u.NGAYKHOA = DateOnly.FromDateTime(DateTime.Now);
                            u.NGAYMOKHOA = u.NGAYKHOA.Value.AddDays(formLock.LockDays ?? 1);
                            
                            db.SaveChanges();
                            MessageBox.Show($"Đã khóa tài khoản {user.USERNAME} trong {formLock.LockDays} ngày!\nTài khoản sẽ tự động mở khóa vào ngày {u.NGAYMOKHOA.Value:dd/MM/yyyy}.", 
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                    ReloadAccounts();
                }
            }
        }
        private void BtnUnban_Click(object sender, EventArgs e)
        {
            var btn = (Guna.UI2.WinForms.Guna2Button)sender;
            var user = (TAIKHOAN)btn.Tag;

            string lockType = user.TRANGTHAI == "Khóa vĩnh viễn" ? "vĩnh viễn" : "tạm thời";
            
            // Hiển thị MessageBox xác nhận trước khi mở khóa
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn mở khóa tài khoản {user.USERNAME}?\n(Tài khoản đang bị khóa {lockType})",
                "Xác nhận mở khóa tài khoản",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            // Chỉ thực hiện khi người dùng chọn Yes
            if (result == DialogResult.Yes)
            {
                using (var db = new MiniStoreContext())
                {
                    var u = db.TAIKHOANs.Find(user.USERNAME);

                    u.TRANGTHAI = "Hoạt động";
                    u.NGAYMOKHOA = DateOnly.FromDateTime(DateTime.Now);
                    u.NGAYKHOA = null; // Xóa ngày khóa khi mở khóa

                    db.SaveChanges();
                }

                MessageBox.Show("Đã mở khóa tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReloadAccounts();
            }
        }
        private void ReloadAccounts()
        {
            flpSettingAcc.Controls.Clear();
            UC_settingAccount_Load(null, null);
        }



    }
}
