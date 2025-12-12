using MiniStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace MiniShop.User_Control.UC_Extra
{
    public partial class UC_CapNhatSanPham : UserControl
    {
        private List<SANPHAM> _products = new();
        private string? _selectedImageFileName;
        public event EventHandler? RequestClose;
        public UC_CapNhatSanPham()
        {
            AutoScaleMode = AutoScaleMode.None;
            InitializeComponent();
        }

        private void guna2HtmlLabel6_Click(object sender, EventArgs e)
        {

        }

        private void UC_CapNhatSanPham_Load(object sender, EventArgs e)
        {
            using (var db = new MiniStoreContext())
            {
                _products = db.SANPHAMs
                    .Include(p => p.MALOAINavigation)
                    .Include(p => p.MANCCNavigation)
                    .ToList();
            }

            cboMaLoai.DisplayMember = nameof(SANPHAM.MASP);
            cboMaLoai.ValueMember = nameof(SANPHAM.MASP);
            cboMaLoai.DataSource = _products;
            cboMaLoai.SelectedIndexChanged += cboMaLoai_SelectedIndexChanged;

            if (_products.Count > 0)
            {
                cboMaLoai.SelectedIndex = 0;
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        private void cboMaLoai_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboMaLoai.SelectedItem is not SANPHAM sp) return;

            txtTenSanPham.Text = sp.TENSP;
            txtDVT.Text = sp.DVT;
            txtSoLuong.Text = sp.SOLUONG?.ToString();
            txtGia.Text = sp.GIABAN?.ToString();
            txtBarcode.Text = sp.BARCODE;
            txtGhiChu.Text = sp.GHICHU;

            picHinhSanPham.Image = LoadImageSafe(sp.HINH);
            _selectedImageFileName = sp.HINH;
        }

        private Image? LoadImageSafe(string? pathOrFileName)
        {
            if (string.IsNullOrWhiteSpace(pathOrFileName))
                return null;

            string fullPath = pathOrFileName;

            if (!Path.IsPathRooted(fullPath))
            {
                string candidate = Path.Combine(Application.StartupPath, "ImagesProduct", pathOrFileName);
                if (File.Exists(candidate))
                {
                    fullPath = candidate;
                }
                else
                {
                    // thử lấy theo tên file nếu pathOrFileName có chứa thư mục
                    string fileName = Path.GetFileName(pathOrFileName);
                    string candidate2 = Path.Combine(Application.StartupPath, "ImagesProduct", fileName);
                    if (File.Exists(candidate2))
                    {
                        fullPath = candidate2;
                    }
                }
                if (File.Exists(candidate))
                {
                    fullPath = candidate;
                }
            }

            if (!File.Exists(fullPath)) return null;

            try
            {
                using var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var ms = new MemoryStream();
                fs.CopyTo(ms);
                ms.Position = 0;
                return new Bitmap(ms);
            }
            catch
            {
                return null;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            picHinhSanPham.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void picHinhSanPham_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif",
                Title = "Chọn hình sản phẩm"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            try
            {
                string imagesFolder = Path.Combine(Application.StartupPath, "ImagesProduct");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                var fileName = Path.GetFileName(ofd.FileName);
                string destPath = Path.Combine(imagesFolder, fileName);

                picHinhSanPham.Image?.Dispose();

                // Nếu file đã nằm trong ImagesProduct thì không cần copy
                var sourceFull = Path.GetFullPath(ofd.FileName);
                var imagesFull = Path.GetFullPath(imagesFolder);
                bool sourceInImages = sourceFull.StartsWith(imagesFull, StringComparison.OrdinalIgnoreCase);

                if (sourceInImages)
                {
                    _selectedImageFileName = fileName;
                    picHinhSanPham.Image = LoadImageSafe(fileName);
                    return;
                }

                try
                {
                    if (!string.Equals(sourceFull, destPath, StringComparison.OrdinalIgnoreCase))
                    {
                        File.Copy(sourceFull, destPath, true);
                    }
                    _selectedImageFileName = fileName;
                    picHinhSanPham.Image = LoadImageSafe(fileName);
                }
                catch (IOException)
                {
                    // Nếu file đang bị lock, dùng luôn đường dẫn gốc (absolute) để hiển thị/lưu
                    _selectedImageFileName = sourceFull;
                    picHinhSanPham.Image = LoadImageSafe(sourceFull);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (cboMaLoai.SelectedItem is not SANPHAM spSelected)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using var db = new MiniStoreContext();
                var sp = db.SANPHAMs.FirstOrDefault(x => x.MASP == spSelected.MASP);
                if (sp == null)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!TryValidateInputs(out var soLuongMoi, out var giaMoi))
                {
                    return;
                }

                // Lưu thông tin cũ để ghi log
                var soLuongCu = sp.SOLUONG ?? 0;
                var giaBanCu = sp.GIABAN ?? 0;

                sp.TENSP = txtTenSanPham.Text;
                sp.DVT = txtDVT.Text;
                sp.SOLUONG = soLuongMoi;
                sp.GIABAN = giaMoi;
                sp.BARCODE = txtBarcode.Text;
                sp.GHICHU = txtGhiChu.Text;
                sp.HINH = _selectedImageFileName ?? sp.HINH;

                db.SaveChanges();

                // Ghi log cập nhật sản phẩm
                // Lưu username để tránh lỗi mã hóa tên Unicode trên cột VARCHAR
                var nguoiNhap = !string.IsNullOrWhiteSpace(MiniStore.Class.UserSession.Username)
                    ? MiniStore.Class.UserSession.Username
                    : (!string.IsNullOrWhiteSpace(MiniStore.Class.UserSession.FullName)
                        ? MiniStore.Class.UserSession.FullName
                        : "Không rõ");

                var log = new MiniStore.Models.LogCTHDNhap
                {
                    MAHDNHAP = null, // Cập nhật không có hóa đơn nhập
                    MASP = sp.MASP,
                    SOLUONGTN = sp.SOLUONG ?? 0,
                    DONGIANHAP = sp.GIABAN ?? 0,
                    GHICHU = "Cập nhật sản phẩm",
                    NGUOINHAP = nguoiNhap,
                    LoggedAt = DateTime.Now
                };
                db.LogCTHDNhaps.Add(log);
                db.SaveChanges();

                MessageBox.Show("Cập nhật sản phẩm thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cập nhật thất bại: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool TryValidateInputs(out int soLuong, out decimal giaBan)
        {
            soLuong = 0;
            giaBan = 0;

            if (!int.TryParse(txtSoLuong.Text, out soLuong))
            {
                MessageBox.Show("Số lượng phải là số nguyên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (soLuong <= 0)
            {
                MessageBox.Show("Số lượng không được âm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtGia.Text, out giaBan))
            {
                MessageBox.Show("Giá phải là số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (giaBan <= 0)
            {
                MessageBox.Show("Giá không được âm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

    }
}
