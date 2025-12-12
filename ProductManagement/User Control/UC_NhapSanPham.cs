using MiniStore.Models;
using MiniStore.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace MiniStore.User_Control
{
    public partial class UC_NhapSanPham : UserControl
    {
        public UC_NhapSanPham()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dtpNSX.MinDate = DateTime.Today;
            dtpNSX.Value = DateTime.Today;
            isDateSelected = false; // yêu cầu người dùng chọn/ xác nhận NSX
            LoadDonViTinh();
            LoadMaLoai();
            LoadMaNhaCC();
        }



        private void pbox_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn hình ảnh sản phẩm";
                ofd.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pbox.Image = Image.FromFile(ofd.FileName);
                    pbox.Tag = ofd.FileName;
                }
            }
        }


        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            var errors = ValidateInputs();
            if (errors.Any())
            {
                MessageBox.Show("Vui lòng kiểm tra:\n- " + string.Join("\n- ", errors),
                    "Thiếu/không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sourcePath = pbox.Tag.ToString();
            string folderPath = Path.Combine(Application.StartupPath, "ImagesProduct");

            string fileName = Path.GetFileNameWithoutExtension(sourcePath)
                  + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss")
                  + Path.GetExtension(sourcePath);
            string destPath = Path.Combine(folderPath, fileName);
            File.Copy(sourcePath, destPath, true);

            using (var db = new MiniStoreContext())
            {
                string maSP = txtMaSP.Text;
                string maLoai = cboMaLoai.Text;
                string maNcc = cboMaNhaCC.Text;

                // Kiểm tra khóa ngoại
                bool loaiTonTai = db.LOAISANPHAMs.Any(l => l.MALOAI == maLoai);
                bool nccTonTai = db.NHACUNGCAPs.Any(n => n.MANCC == maNcc);
                var fkErrors = new List<string>();
                if (!loaiTonTai) fkErrors.Add($"Mã loại '{maLoai}' không tồn tại.");
                if (!nccTonTai) fkErrors.Add($"Mã nhà cung cấp '{maNcc}' không tồn tại.");
                if (fkErrors.Any())
                {
                    MessageBox.Show("Vui lòng kiểm tra:\n- " + string.Join("\n- ", fkErrors),
                        "Thiếu/không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Kiểm tra xem sản phẩm đã tồn tại chưa
                var spExist = db.SANPHAMs.FirstOrDefault(p => p.MASP == maSP);
                
                if (spExist != null)
                {
                    MessageBox.Show($"Mã sản phẩm '{maSP}' đã tồn tại, không thể thêm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    // Sản phẩm chưa tồn tại: Thêm mới
                    var sp = new SANPHAM
                    {
                        MALOAI = maLoai,
                        MASP = txtMaSP.Text,
                        TENSP = txtTenSanPham.Text,
                        GHICHU = txtGhiChu.Text,
                        DVT = cboDVT.Text,
                        MANCC = maNcc,
                        GIABAN = decimal.Parse(txtGiaBan.Text),
                        SOLUONG = int.Parse(txtSoluong.Text),
                        NSX = DateOnly.FromDateTime(dtpNSX.Value),
                        HINH = fileName,
                    };

                    db.SANPHAMs.Add(sp);
                    
                    // Tự động tạo bản ghi HANGTRUNGBAY nếu chưa có
                    var hangTrungBay = new HANGTRUNGBAY
                    {
                        MASP = maSP,
                        SOLUONG_TRENKE = 0,
                        TRANGTHAI = "Đang bán"
                    };
                    db.HANGTRUNGBAYs.Add(hangTrungBay);

                    // Tạo hóa đơn nhập (HDNHAP) tự động
                    string nextMaHDN = GenerateNextMaHDN(db);
                    var username = string.IsNullOrWhiteSpace(UserSession.Username)
                        ? null
                        : UserSession.Username;
                    var usernameExists = !string.IsNullOrWhiteSpace(username) &&
                                          db.TAIKHOANs.Any(t => t.USERNAME == username);

                    // Ưu tiên lưu username vào log để tránh lỗi mã hóa tên Unicode trên cột VARCHAR
                    var nguoiNhap = !string.IsNullOrWhiteSpace(username) ? username : string.Empty;
                    if (string.IsNullOrWhiteSpace(nguoiNhap))
                    {
                        // fallback tên đầy đủ khi chưa có tài khoản (có thể bị lỗi mã hóa nếu cột không Unicode)
                        nguoiNhap = !string.IsNullOrWhiteSpace(UserSession.FullName)
                            ? UserSession.FullName
                            : "Không rõ";
                    }

                    var hdNhap = new HDNHAP
                    {
                        MAHDNHAP = nextMaHDN,
                        MANCC = maNcc,
                        USERNAME = usernameExists ? username : null, // tránh lỗi FK nếu chưa có user hợp lệ
                        NGAYLAP = DateTime.Now.Date,
                        GHICHU = "Tự động tạo khi thêm sản phẩm mới"
                    };
                    db.HDNHAPs.Add(hdNhap);

                    // Ghi log thêm sản phẩm vào bảng LogCTHDNhap để hiển thị ở "Nhật ký thêm sản phẩm"
                    var log = new LogCTHDNhap
                    {
                        MAHDNHAP = nextMaHDN,
                        MASP = maSP,
                        SOLUONGTN = int.Parse(txtSoluong.Text),
                        DONGIANHAP = 0, // chưa có giá nhập, đặt 0
                        GHICHU = "Thêm mới sản phẩm",
                        NGUOINHAP = nguoiNhap,
                        LoggedAt = DateTime.Now
                    };
                    db.LogCTHDNhaps.Add(log);
                    
                    db.SaveChanges();
                    SanPhamDaThem?.Invoke(this, EventArgs.Empty);
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                
                this.Dispose();
            }
        }
        bool isDateSelected = false;

        private void dtpNSX_ValueChanged_1(object sender, EventArgs e)
        {
            isDateSelected = true;
            ValidateNSX();
        }
        private void ValidateNSX()
        {
            if (!isDateSelected)
            {
                MessageBox.Show("Bạn quên chọn ngày sản xuất hả?", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public event EventHandler SanPhamDaThem;

        private List<string> ValidateInputs()
        {
            var errors = new List<string>();

            if (pbox.Tag == null)
                errors.Add("Chưa chọn hình sản phẩm.");

            if (string.IsNullOrWhiteSpace(cboMaLoai.Text))
                errors.Add("Mã loại không được để trống.");

            if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                errors.Add("Mã sản phẩm không được để trống.");

            if (string.IsNullOrWhiteSpace(cboDVT.Text))
                errors.Add("Đơn vị tính không được để trống.");

            if (string.IsNullOrWhiteSpace(txtGiaBan.Text) || !decimal.TryParse(txtGiaBan.Text, out var giaBan) || giaBan <= 0)
                errors.Add("Giá bán phải là số > 0.");

            if (string.IsNullOrWhiteSpace(txtSoluong.Text) || !int.TryParse(txtSoluong.Text, out var soLuong) || soLuong <= 0)
                errors.Add("Số lượng phải là số nguyên > 0.");

            if (string.IsNullOrWhiteSpace(cboMaNhaCC.Text))
                errors.Add("Mã nhà cung cấp không được để trống.");

            if (txtBarcode != null && string.IsNullOrWhiteSpace(txtBarcode.Text))
                errors.Add("Barcode không được để trống.");

            if (!isDateSelected)
                errors.Add("Bạn quên chọn ngày sản xuất.");
            else if (dtpNSX.Value.Date < DateTime.Now.Date)
                errors.Add("Ngày sản xuất không được chọn ngày của quá khứ.");

            return errors;
        }

        private string GenerateNextMaHDN(MiniStoreContext db)
        {
            var lastCode = db.HDNHAPs
                .Select(h => h.MAHDNHAP)
                .Where(s => s != null && s.StartsWith("HDN"))
                .ToList();

            int maxNum = 0;
            var regex = new Regex(@"^HDN(\d+)$");
            foreach (var code in lastCode)
            {
                var match = regex.Match(code);
                if (match.Success && int.TryParse(match.Groups[1].Value, out int n))
                {
                    if (n > maxNum) maxNum = n;
                }
            }

            int next = maxNum + 1;
            return $"HDN{next}";
        }

        private void LoadDonViTinh()
        {
            try
            {
                using var db = new MiniStoreContext();
                var dvtList = db.SANPHAMs
                    .Select(s => s.DVT)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct()
                    .ToList();

                cboDVT.Items.Clear();
                if (dvtList.Any())
                {
                    cboDVT.Items.AddRange(dvtList.ToArray());
                    cboDVT.SelectedIndex = 0;
                }
            }
            catch
            {
                // ignore loading errors
            }
        }

        private void LoadMaLoai()
        {
            try
            {
                using var db = new MiniStoreContext();
                var list = db.LOAISANPHAMs
                    .Select(l => l.MALOAI)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct()
                    .ToList();
                cboMaLoai.Items.Clear();
                if (list.Any())
                {
                    cboMaLoai.Items.AddRange(list.ToArray());
                    cboMaLoai.SelectedIndex = -1;
                }
            }
            catch { }
        }

        private void LoadMaNhaCC()
        {
            try
            {
                using var db = new MiniStoreContext();
                var list = db.NHACUNGCAPs
                    .Select(n => n.MANCC)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct()
                    .ToList();
                cboMaNhaCC.Items.Clear();
                if (list.Any())
                {
                    cboMaNhaCC.Items.AddRange(list.ToArray());
                    cboMaNhaCC.SelectedIndex = -1;
                }
            }
            catch { }
        }
    }
}

