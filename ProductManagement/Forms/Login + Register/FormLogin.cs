using Guna.UI2.WinForms;
using LibVLCSharp.Shared;
using Microsoft.VisualBasic.ApplicationServices;
using MiniStore.Models;
using MiniStore.User_Control;
using System.IO;
namespace MiniStore
{
    public partial class FormLogin : Form
    {
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private System.Windows.Forms.Timer _validationTimer;
        private string _lastValidatedUserName = string.Empty;
        private bool _isValidating = false;
        
        public FormLogin()
        {
            InitializeComponent();
            // Khởi tạo timer cho debouncing validation
            _validationTimer = new System.Windows.Forms.Timer();
            _validationTimer.Interval = 300; // 300ms debounce
            _validationTimer.Tick += async (s, e) =>
            {
                _validationTimer.Stop();
                if (!_isValidating)
                {
                    await ValidateUserNameAsync();
                }
            };
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
                
                if (user == null)
                {
                    lblErrorUserName.Text = "Tài khoản không tồn tại";
                    lblErrorUserName.ForeColor = Color.Red;
                    return;
                }

                // Kiểm tra mật khẩu
                if (user.PASSWORD != txtPassWord.Text)
                {
                    lblErrorPassword.Text = $"password không khớp tài khoản sẽ bị khóa trong {count} lần nhập sai ";
                    lblErrorPassword.ForeColor = Color.Red;
                    lblQuenMatKhau.Text = "Bạn quên mật khẩu hả?";
                    lblQuenMatKhau.ForeColor = Color.Blue;
                    
                    if (count == 0)
                    {
                        user.NGAYKHOA = DateOnly.FromDateTime(DateTime.Now.Date);
                        user.NGAYMOKHOA = user.NGAYKHOA.Value.AddDays(1);
                        MessageBox.Show($"Tài khoản của bạn đã bị khóa cho đến hết ngày {user.NGAYMOKHOA.Value}", "Thông báo tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        user.TRANGTHAI = "Khóa";
                        db.SaveChanges();
                        return;
                    }
                    count--;
                    return; // Quan trọng: return ngay khi password sai để không mở form TrangChu
                }

                // Chỉ mở TrangChu khi password đúng
                if (user.MAROLE == "ADMIN" || user.MAROLE == "NV" || user.MAROLE == "KH")
                {
                    TrangChu tc = new TrangChu(user.MAROLE);
                    UC_Product prod = new UC_Product(user.MAROLE);
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


            Core.Initialize();

            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

            videoView1.MediaPlayer = _mediaPlayer;
            videoView1.Dock = DockStyle.Fill;   // Quan trọng: Video phải fill toàn màn hình
            videoView1.SendToBack();            // Video nằm dưới panel login

            // Đường dẫn video từ thư mục Forms
            string videoPath = Path.Combine(Application.StartupPath, "..", "..", "..", "Forms", "videos", "mystore!(1).mp4");
            string fullPath = Path.GetFullPath(videoPath);

            // Nếu không tìm thấy, thử đường dẫn trong thư mục bin
            if (!File.Exists(fullPath))
            {
                string altPath = Path.Combine(Application.StartupPath, "videos", "mystore!(1).mp4");
                if (File.Exists(altPath))
                {
                    fullPath = altPath;
                }
                else
                {
                    // Thử đường dẫn khác: Forms\videos
                    string altPath2 = Path.Combine(Application.StartupPath, "..", "..", "..", "..", "Forms", "videos", "mystore!(1).mp4");
                    string fullPath2 = Path.GetFullPath(altPath2);
                    if (File.Exists(fullPath2))
                    {
                        fullPath = fullPath2;
                    }
                }
            }

            if (File.Exists(fullPath))
            {
                var media = new Media(_libVLC, fullPath, FromType.FromPath,
                                      ":input-repeat=-1",
                                      ":no-video-title-show");

                _mediaPlayer.Media = media;
                _mediaPlayer.Play();
                _mediaPlayer.Mute = true;

                // Đảm bảo video lặp lại khi kết thúc
                _mediaPlayer.EndReached += (sender, e) =>
                {
                    // Tạo media mới và phát lại
                    var newMedia = new Media(_libVLC, fullPath, FromType.FromPath,
                                            ":input-repeat=-1",
                                            ":no-video-title-show");
                    _mediaPlayer.Media = newMedia;
                    _mediaPlayer.Play();
                };
            }
            else
            {
                MessageBox.Show("Không tìm thấy video. Đã thử:\n" +
                              Path.GetFullPath(Path.Combine(Application.StartupPath, "..", "..", "..", "Forms", "videos", "mystore!(1).mp4")) + "\n" +
                              Path.Combine(Application.StartupPath, "videos", "mystore!(1).mp4"));
            }




        }

        private async void txtUserName_Leave(object sender, EventArgs e)
        {
            // Dừng timer nếu đang chạy
            _validationTimer.Stop();
            // Validate ngay lập tức khi rời khỏi ô
            await ValidateUserNameAsync();
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
            
            // Debounce validation - chỉ validate sau khi người dùng ngừng gõ 300ms
            _validationTimer.Stop();
            _validationTimer.Start();
        }


        private async Task ValidateUserNameAsync()
        {
            // Tránh validate đồng thời nhiều lần
            if (_isValidating) return;
            
            string currentUserName = txtUserName.Text;
            
            // Nếu username không thay đổi và đã validate rồi thì bỏ qua
            if (currentUserName == _lastValidatedUserName && !string.IsNullOrEmpty(_lastValidatedUserName))
            {
                return;
            }
            
            _isValidating = true;
            
            try
            {
                // Kiểm tra trống trước (không cần database) - cập nhật UI ngay
                if (string.IsNullOrWhiteSpace(currentUserName))
                {
                    UpdateUI(() =>
                    {
                        lblErrorUserName.Text = "Vui lòng không để trống username!";
                        lblErrorUserName.ForeColor = Color.Red;
                    });
                    _lastValidatedUserName = currentUserName;
                    return;
                }
                
                // Chạy database query bất đồng bộ để không block UI
                TAIKHOAN user = null;
                using (var db = new MiniStoreContext())
                {
                    // Query database trong background
                    user = await Task.Run(() => 
                        db.TAIKHOANs.FirstOrDefault(username => username.USERNAME == currentUserName));
                    
                    // Kiểm tra lại xem username có thay đổi không (tránh race condition)
                    if (txtUserName.Text != currentUserName)
                    {
                        return;
                    }
                    
                    // Xử lý kết quả và cập nhật UI
                    if (user == null)
                    {
                        UpdateUI(() =>
                        {
                            lblErrorUserName.Text = "Tài khoản không tồn tại";
                            lblErrorUserName.ForeColor = Color.Red;
                        });
                        _lastValidatedUserName = currentUserName;
                    }
                    else if (user.TRANGTHAI == "Khóa")
                    {
                        if (user.NGAYKHOA.HasValue && DateOnly.FromDateTime(DateTime.Now) >= user.NGAYMOKHOA.Value)
                        {
                            // Mở khóa tài khoản
                            user.TRANGTHAI = "Hoạt động";
                            user.NGAYKHOA = null;
                            user.NGAYMOKHOA = null;
                            await Task.Run(() => db.SaveChanges());

                            UpdateUI(() =>
                            {
                                lblErrorUserName.Text = string.Empty;
                                txtPassWord.Enabled = true;
                            });
                            _lastValidatedUserName = currentUserName;
                        }
                        else
                        {
                            UpdateUI(() =>
                            {
                                lblErrorUserName.Text = $"Tài khoản của bạn bị khóa đến hết ngày {user.NGAYMOKHOA:dd/MM/yyyy}";
                                lblErrorUserName.ForeColor = Color.Red;
                                txtPassWord.Enabled = false;
                            });
                            _lastValidatedUserName = currentUserName;
                        }
                    }
                    else
                    {
                        UpdateUI(() =>
                        {
                            lblErrorUserName.Text = string.Empty;
                        });
                        _lastValidatedUserName = currentUserName;
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi một cách im lặng để không làm gián đoạn UX
                System.Diagnostics.Debug.WriteLine($"Validation error: {ex.Message}");
            }
            finally
            {
                _isValidating = false;
            }
        }
        
        private void UpdateUI(Action action)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(action);
            }
            else
            {
                action();
            }
        }
        
        // Giữ lại method cũ để tương thích với code hiện tại
        private void validateUserName()
        {
            // Chuyển sang async version
            _ = ValidateUserNameAsync();
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

        private void backgroundLogin_Enter(object sender, EventArgs e)
        {

        }

        private void videoView1_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2CustomGradientPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void videoView1_Click_1(object sender, EventArgs e)
        {

        }
        
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Giải phóng timer
            if (_validationTimer != null)
            {
                _validationTimer.Stop();
                _validationTimer.Dispose();
            }
            
            // Giải phóng media player
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Stop();
                _mediaPlayer.Dispose();
            }
            
            if (_libVLC != null)
            {
                _libVLC.Dispose();
            }
            
            base.OnFormClosed(e);
        }
    }
}
