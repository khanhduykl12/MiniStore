using Guna.UI2.WinForms;
using LibVLCSharp.Shared;
using Microsoft.VisualBasic.ApplicationServices;
using MiniStore.Models;
using MiniStore.User_Control;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
                // Kiểm tra xem form có đang đóng không
                if (this.IsDisposed || this.Disposing)
                {
                    _validationTimer.Stop();
                    return;
                }
                
                _validationTimer.Stop();
                if (!_isValidating && !this.IsDisposed)
                {
                    try
                    {
                        await ValidateUserNameAsync();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error in timer validation: {ex.Message}");
                    }
                }
            };
            
            // Đảm bảo form có thể nhận focus và đóng được
            this.FormClosing += FormLogin_FormClosing;
            this.Shown += FormLogin_Shown;
            this.Activated += FormLogin_Activated;
            this.VisibleChanged += FormLogin_VisibleChanged;
        }
        
        private void FormLogin_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                // Khi form được hiển thị, đảm bảo nó hoạt động đúng
                this.BeginInvoke(new Action(() =>
                {
                    // Đảm bảo videoView không chặn
                    videoView1.SendToBack();
                    SetVideoViewMouseEvents(false);
                    
                    this.Activate();
                    this.BringToFront();
                    this.Enabled = true;
                    
                    // Đảm bảo button đóng luôn ở trên cùng
                    guna2ImageButton1.BringToFront();
                    guna2ImageButton1.Enabled = true;
                    guna2ImageButton1.Visible = true;
                    guna2ImageButton1.Invalidate();
                    guna2ImageButton1.Update();
                    
                    Application.DoEvents();
                }));
            }
        }
        
        private void FormLogin_Activated(object sender, EventArgs e)
        {
            // Đảm bảo nút đóng luôn có thể click khi form được activate
            videoView1.SendToBack();
            SetVideoViewMouseEvents(false);
            guna2ImageButton1.BringToFront();
            guna2ImageButton1.Enabled = true;
            guna2ImageButton1.Visible = true;
            guna2ImageButton1.Invalidate();
            guna2ImageButton1.Update();
        }
        
        private void FormLogin_Shown(object sender, EventArgs e)
        {
            // Đảm bảo form được activate và focus
            this.Activate();
            this.BringToFront();
            this.Focus();
            // Đảm bảo videoView không chặn các control
            videoView1.SendToBack();
            SetVideoViewMouseEvents(false);
            // Đảm bảo nút đóng luôn có thể click
            guna2ImageButton1.BringToFront();
            guna2ImageButton1.Enabled = true;
            guna2ImageButton1.Visible = true;
            guna2ImageButton1.Invalidate();
            guna2ImageButton1.Update();
        }
        
        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cho phép form đóng bình thường
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = false;
            }
        }
        
        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(value);
            if (value)
            {
                // Khi form được hiển thị, đảm bảo nó hoạt động đúng
                this.BeginInvoke(new Action(() =>
                {
                    // Đảm bảo videoView không chặn
                    videoView1.SendToBack();
                    SetVideoViewMouseEvents(false);
                    
                    this.Activate();
                    this.BringToFront();
                    this.Enabled = true;
                    
                    // Đảm bảo button đóng luôn ở trên cùng
                    guna2ImageButton1.BringToFront();
                    guna2ImageButton1.Enabled = true;
                    guna2ImageButton1.Visible = true;
                    guna2ImageButton1.Invalidate();
                    guna2ImageButton1.Update();
                    
                    Application.DoEvents();
                }));
            }
        }
        int count = 3;
        private async void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            // Validate username khi click button đăng nhập
            await ValidateUserNameAsync();
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

                // Kiểm tra trạng thái tài khoản trước khi kiểm tra mật khẩu
                if (user.TRANGTHAI == "Khóa vĩnh viễn")
                {
                    // Tài khoản bị khóa vĩnh viễn - không cho đăng nhập
                    lblErrorUserName.Text = "Tài khoản của bạn đã bị khóa vĩnh viễn. Vui lòng liên hệ quản trị viên.";
                    lblErrorUserName.ForeColor = Color.Red;
                    txtPassWord.Enabled = false;
                    return;
                }
                else if (user.TRANGTHAI == "Khóa")
                {
                    // Kiểm tra xem đã đến ngày mở khóa chưa
                    if (user.NGAYKHOA.HasValue && user.NGAYMOKHOA.HasValue)
                    {
                        if (DateOnly.FromDateTime(DateTime.Now) < user.NGAYMOKHOA.Value)
                        {
                            // Tài khoản vẫn còn bị khóa
                            lblErrorUserName.Text = $"Tài khoản của bạn bị khóa đến hết ngày {user.NGAYMOKHOA.Value:dd/MM/yyyy}";
                            lblErrorUserName.ForeColor = Color.Red;
                            txtPassWord.Enabled = false;
                            return;
                        }
                        else
                        {
                            // Tự động mở khóa nếu đã hết hạn
                            user.TRANGTHAI = "Hoạt động";
                            user.NGAYKHOA = null;
                            user.NGAYMOKHOA = null;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        // Tài khoản bị khóa nhưng không có ngày mở khóa
                        lblErrorUserName.Text = "Tài khoản của bạn đang bị khóa. Vui lòng liên hệ quản trị viên.";
                        lblErrorUserName.ForeColor = Color.Red;
                        txtPassWord.Enabled = false;
                        return;
                    }
                }

                // Kiểm tra mật khẩu
                if (user.PASSWORD != txtPassWord.Text)
                {
                    // Dừng validation timer trước khi hiển thị lỗi
                    if (_validationTimer != null)
                    {
                        _validationTimer.Stop();
                    }
                    _isValidating = false;
                    
                    lblErrorPassword.Text = $"password không khớp tài khoản sẽ bị khóa trong {count} lần nhập sai ";
                    lblErrorPassword.ForeColor = Color.Red;
                    lblQuenMatKhau.Text = "Bạn quên mật khẩu hả?";
                    lblQuenMatKhau.ForeColor = Color.Blue;
                    
                    // Đảm bảo button đóng vẫn hoạt động sau khi sai mật khẩu
                    EnsureCloseButtonEnabled();
                    
                    // Force refresh UI để button có thể click được
                    Application.DoEvents();
                    
                    if (count == 0)
                    {
                        user.NGAYKHOA = DateOnly.FromDateTime(DateTime.Now.Date);
                        user.NGAYMOKHOA = user.NGAYKHOA.Value.AddDays(1);
                        MessageBox.Show($"Tài khoản của bạn đã bị khóa cho đến hết ngày {user.NGAYMOKHOA.Value}", "Thông báo tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        user.TRANGTHAI = "Khóa";
                        db.SaveChanges();
                        EnsureCloseButtonEnabled();
                        Application.DoEvents(); // Process messages sau MessageBox
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
                    tc.Dispose(); // Giải phóng form TrangChu
                    
                    // Hiển thị lại form ngay lập tức
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    this.Enabled = true;
                    this.Visible = true;
                    
                    // Đảm bảo form được activate
                    this.Activate();
                    this.BringToFront();
                    
                    // Đảm bảo các control được enable và có thể tương tác
                    this.Enabled = true;
                    
                    // Đảm bảo videoView không chặn các control (phải làm trước)
                    videoView1.SendToBack();
                    SetVideoViewMouseEvents(false);
                    
                    // Đảm bảo button đóng luôn ở trên cùng và hoạt động
                    guna2ImageButton1.BringToFront();
                    guna2ImageButton1.Enabled = true;
                    guna2ImageButton1.Visible = true;
                    guna2ImageButton1.Invalidate();
                    guna2ImageButton1.Update();
                    
                    txtUserName.Enabled = true;
                    txtPassWord.Enabled = true;
                    btnRegisterForm.Enabled = true;
                    guna2GradientButton1.Enabled = true;
                    
                    // Reset form về trạng thái ban đầu
                    txtUserName.Text = string.Empty;
                    txtPassWord.Text = string.Empty;
                    lblErrorUserName.Text = string.Empty;
                    lblErrorPassword.Text = string.Empty;
                    lblQuenMatKhau.Text = string.Empty;
                    count = 3; // Reset lại số lần nhập sai
                    
                    // Force refresh form
                    this.Refresh();
                    this.Update();
                    Application.DoEvents(); // Process pending messages
                    
                    // Focus vào form và ô username
                    this.Focus();
                    txtUserName.Focus();
                }

            }


        }

        private void btnRegisterForm_Click(object sender, EventArgs e)
        {
            FormRegister reg = new FormRegister();
            this.Hide();
            reg.ShowDialog();
            // Đảm bảo form được hiển thị và nhận focus đúng cách
            this.Show();
            this.Activate();
            this.BringToFront();
            this.Focus();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            // EXIT NGAY LẬP TỨC - KHÔNG CHỜ ĐỢI, KHÔNG BLOCK
            System.Diagnostics.Debug.WriteLine("guna2ImageButton1_Click called");
            
            // Disable button ngay
            guna2ImageButton1.Enabled = false;
            
            // Environment.Exit(0) sẽ kill process ngay lập tức
            // Không cần cleanup, không cần dừng operations
            // Đây là cách nhanh nhất để thoát app
            Environment.Exit(0);
        }
        
        // Method để dừng tất cả operations
        private void StopAllOperations()
        {
            try
            {
                // Dừng validation timer
                if (_validationTimer != null)
                {
                    _validationTimer.Stop();
                    _validationTimer.Tick -= null; // Remove event handlers
                    _validationTimer.Dispose();
                    _validationTimer = null;
                }
                
                // Dừng validation flag
                _isValidating = false;
                
                // Dừng video
                if (_mediaPlayer != null)
                {
                    try
                    {
                        _mediaPlayer.Stop();
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in StopAllOperations: {ex.Message}");
            }
        }
        
        // Thêm handler cho MouseDown để đảm bảo nút luôn nhận được events
        private void guna2ImageButton1_MouseDown(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("guna2ImageButton1_MouseDown called");
            // Exit ngay lập tức
            guna2ImageButton1.Enabled = false;
            Environment.Exit(0);
        }
        
        // Thêm handler cho MouseUp để đảm bảo button luôn hoạt động
        private void guna2ImageButton1_MouseUp(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("guna2ImageButton1_MouseUp called");
            // Exit ngay
            guna2ImageButton1.Enabled = false;
            Environment.Exit(0);
        }
        
        // Method để đảm bảo button đóng luôn hoạt động
        private void EnsureCloseButtonEnabled()
        {
            try
            {
                // Đảm bảo videoView không chặn
                videoView1.SendToBack();
                SetVideoViewMouseEvents(false);
                
                // Đảm bảo button ở trên cùng và enabled
                guna2ImageButton1.BringToFront();
                guna2ImageButton1.Enabled = true;
                guna2ImageButton1.Visible = true;
                guna2ImageButton1.Invalidate();
                guna2ImageButton1.Update();
                
                // Đảm bảo form enabled
                this.Enabled = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in EnsureCloseButtonEnabled: {ex.Message}");
            }
        }
        
        // Method để disable mouse events của videoView nhưng vẫn cho phép phát video
        private void SetVideoViewMouseEvents(bool enable)
        {
            try
            {
                // Sử dụng SetStyle để control mouse events
                if (videoView1 != null)
                {
                    videoView1.TabStop = false;
                    // VideoView vẫn có thể phát video nhưng không nhận mouse events
                    // bằng cách đảm bảo nó luôn ở dưới các control khác
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in SetVideoViewMouseEvents: {ex.Message}");
            }
        }

        private void lblQuenMatKhau_Click(object sender, EventArgs e)
        {
            PasswordForgot passwordForgot = new PasswordForgot();
            this.Hide();
            passwordForgot.ShowDialog();
            // Đảm bảo form được hiển thị và nhận focus đúng cách
            this.Show();
            this.Activate();
            this.BringToFront();
            this.Focus();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            lblErrorUserName.Text = string.Empty;
            lblErrorPassword.Text = string.Empty;
            lblQuenMatKhau.Text = string.Empty;

            // Đảm bảo videoView không chặn mouse events
            SetVideoViewMouseEvents(false);
            
            // Đảm bảo nút đóng luôn nhận được events
            guna2ImageButton1.MouseDown += guna2ImageButton1_MouseDown;
            guna2ImageButton1.MouseUp += guna2ImageButton1_MouseUp;
            guna2ImageButton1.MouseClick += (s, args) => 
            {
                System.Diagnostics.Debug.WriteLine("guna2ImageButton1_MouseClick called");
                // Exit ngay lập tức - không chờ đợi
                guna2ImageButton1.Enabled = false;
                Environment.Exit(0);
            };
            
            // Đảm bảo button ở trên cùng ngay từ đầu
            guna2ImageButton1.BringToFront();
            guna2ImageButton1.Enabled = true;
            guna2ImageButton1.Visible = true;

            Core.Initialize();

            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

            videoView1.MediaPlayer = _mediaPlayer;
            videoView1.Dock = DockStyle.Fill;   // Quan trọng: Video phải fill toàn màn hình
            videoView1.SendToBack();            // Video nằm dưới panel login
            // Đảm bảo videoView không chặn các control khác - chỉ disable mouse events
            videoView1.TabStop = false;
            // Không disable videoView vì cần phát video, nhưng đảm bảo nó không nhận mouse events
            SetVideoViewMouseEvents(false);

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
                // Lưu đường dẫn video để dùng lại
                string videoFilePath = fullPath;
                
                // Method để phát video (dùng lại khi cần lặp)
                Action playVideo = () =>
                {
                    try
                    {
                        // Tạo media mới mỗi lần để đảm bảo lặp lại đúng cách
                        var media = new Media(_libVLC, videoFilePath, FromType.FromPath,
                                            ":input-repeat=-1",
                                            ":no-video-title-show");
                        
                        _mediaPlayer.Media = media;
                        _mediaPlayer.Mute = true;
                        _mediaPlayer.Play();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error playing video: {ex.Message}");
                    }
                };
                
                // Đảm bảo video lặp lại vô hạn khi kết thúc
                _mediaPlayer.EndReached += (sender, e) =>
                {
                    System.Diagnostics.Debug.WriteLine("Video ended, replaying...");
                    
                    // Phải invoke trên UI thread
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            playVideo();
                        }));
                    }
                    else
                    {
                        playVideo();
                    }
                };
                
                // Bắt đầu phát video lần đầu
                playVideo();
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
            // Không validate khi rời khỏi ô - chỉ validate khi click button đăng nhập
            // Dừng timer nếu đang chạy
            _validationTimer.Stop();
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
            
            // Không validate khi nhập - chỉ validate khi click button đăng nhập
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
                    else if (user.TRANGTHAI == "Khóa vĩnh viễn")
                    {
                        UpdateUI(() =>
                        {
                            lblErrorUserName.Text = "Tài khoản của bạn đã bị khóa vĩnh viễn.";
                            lblErrorUserName.ForeColor = Color.Red;
                            txtPassWord.Enabled = false;
                        });
                        _lastValidatedUserName = currentUserName;
                    }
                    else if (user.TRANGTHAI == "Khóa")
                    {
                        if (user.NGAYKHOA.HasValue && user.NGAYMOKHOA.HasValue && DateOnly.FromDateTime(DateTime.Now) >= user.NGAYMOKHOA.Value)
                        {
                            // Mở khóa tài khoản tự động khi hết hạn
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
                        else if (user.NGAYMOKHOA.HasValue)
                        {
                            UpdateUI(() =>
                            {
                                lblErrorUserName.Text = $"Tài khoản của bạn bị khóa đến hết ngày {user.NGAYMOKHOA.Value:dd/MM/yyyy}";
                                lblErrorUserName.ForeColor = Color.Red;
                                txtPassWord.Enabled = false;
                            });
                            _lastValidatedUserName = currentUserName;
                        }
                        else
                        {
                            UpdateUI(() =>
                            {
                                lblErrorUserName.Text = "Tài khoản của bạn đang bị khóa. Vui lòng liên hệ quản trị viên.";
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
