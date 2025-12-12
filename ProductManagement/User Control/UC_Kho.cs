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
using ZXing;
using ZXing.Common;
using System.Drawing.Imaging;
using ZXing.Windows.Compatibility;
using Microsoft.EntityFrameworkCore;
using MiniShop.User_Control;
using MiniShop.User_Control.UC_Extra;
using System.Drawing.Drawing2D;

namespace MiniStore.User_Control
{
    public partial class UC_Kho : UserControl
    {
        public UC_Kho()
        {
            InitializeComponent();
        }

        private void UC_Kho_Load(object sender, EventArgs e)
        {
            using (var db = new MiniStoreContext())
            {
                var productRaw = db.SANPHAMs.Select(p => new
                {
                    Ten = p.TENSP,
                    Ma = p.MASP,
                    Loai = p.MALOAINavigation.TENLOAI,
                    NgaySX = p.NSX,
                    DonVi = p.DVT,
                    SoLuong = p.SOLUONG,
                    GiaBan = p.GIABAN,
                    TenNhaCungCap = p.MANCCNavigation.TENNCC,
                    Barcode = p.BARCODE,
                    HinhPath = p.HINH,
                    GhiChu = p.GHICHU
                }).ToList();

                var product = productRaw.Select(p => new
                {
                    p.Ten,
                    p.Ma,
                    p.Loai,
                    p.NgaySX,
                    p.DonVi,
                    p.SoLuong,
                    p.GiaBan,
                    p.TenNhaCungCap,
                    p.Barcode,
                    Hinh = LoadImageSafe(p.HinhPath),
                    p.GhiChu
                }).ToList();
                DataGridViewKho.ColumnHeadersHeight = 40;
                DataGridViewKho.AutoGenerateColumns = false;
                DataGridViewKho.DataSource = product;

            }
        }

        private Image LoadImageSafe(string pathOrFileName)
        {
            if (string.IsNullOrWhiteSpace(pathOrFileName)) return null;

            string fullPath = pathOrFileName;

            if (!Path.IsPathRooted(fullPath))
            {

                string candidate2 = Path.Combine(Application.StartupPath, "ImagesProduct", pathOrFileName);
                if (File.Exists(candidate2))
                {
                    fullPath = candidate2;
                }
            }

            if (!File.Exists(fullPath)) return null;

            using (var temp = Image.FromFile(fullPath))
            {
                int width = 48;
                int height = 48;
                return new Bitmap(temp, new Size(width, height));
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var parent = this.Parent;
            if (parent == null) return;
            this.Visible = false;
            var ucNhapSanPham = new UC_NhapSanPham
            {
                Dock = DockStyle.Fill,
                Tag = this
            };
            ucNhapSanPham.Disposed += (sender, EventArgs) =>
            {
                if (this.IsDisposed)
                {
                    return;
                }
                this.Visible = true;
                this.BringToFront();
            };

            ucNhapSanPham.SanPhamDaThem += (s, ev) =>
            {
                LoadDanhSachSanPham(); // <- hàm reload DataGridView
                this.Visible = true;   // hiện lại UC hiện tại
                this.BringToFront();   // đưa lên trên
            };
            parent.Controls.Add(ucNhapSanPham);
            ucNhapSanPham.BringToFront();
        }

        private void btnXoaSanPham_Click(object sender, EventArgs e)
        {
            var parent = this.Parent;
            if (parent == null) return;

            // Hiển thị UC cập nhật trong một form overlay, không ẩn UC_Kho
            var ucCapNhat = new UC_CapNhatSanPham { Dock = DockStyle.Fill };

            var overlay = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterScreen,
                ShowInTaskbar = false,
                TopMost = true,
                BackColor = Color.White,
                Padding = new Padding(10),
                AutoScaleMode = AutoScaleMode.None
            };

            ucCapNhat.RequestClose += (s, ev) => overlay.Close();

            overlay.Controls.Add(ucCapNhat);

            // Giữ nguyên size đã thiết kế của UC, dock fill và khóa kích thước form theo UC
            var targetSize = ucCapNhat.PreferredSize;
            if (targetSize.Width == 0 || targetSize.Height == 0)
            {
                targetSize = ucCapNhat.Size;
            }
            if (targetSize.Width == 0 || targetSize.Height == 0)
            {
                targetSize = new Size(691, 520); // fallback theo thiết kế
            }

            var overlaySize = new Size(
                targetSize.Width + overlay.Padding.Horizontal,
                targetSize.Height + overlay.Padding.Vertical);

            overlay.ClientSize = overlaySize;
            overlay.MinimumSize = overlaySize;
            overlay.MaximumSize = overlaySize;

            overlay.FormClosed += (s, ev) =>
            {
                if (this.IsDisposed) return;
                LoadDanhSachSanPham();
                this.BringToFront();
            };

            var ownerForm = this.FindForm();
            if (ownerForm != null)
            {
                overlay.StartPosition = FormStartPosition.CenterParent;
                overlay.ShowDialog(ownerForm);
            }
            else
            {
                overlay.StartPosition = FormStartPosition.CenterScreen;
                overlay.ShowDialog();
            }
        }
        private void LoadDanhSachSanPham()
        {
            using (var db = new MiniStoreContext())
            {
                var productRaw = db.SANPHAMs.Select(p => new
                {
                    Ten = p.TENSP,
                    Ma = p.MASP,
                    Loai = p.MALOAINavigation.TENLOAI,
                    NgaySX = p.NSX,
                    DonVi = p.DVT,
                    SoLuong = p.SOLUONG,
                    GiaBan = p.GIABAN,
                    TenNhaCungCap = p.MANCCNavigation.TENNCC,
                    Barcode = p.BARCODE,
                    HinhPath = p.HINH,
                    GhiChu = p.GHICHU
                }).ToList();

                var product = productRaw.Select(p => new
                {
                    p.Ten,
                    p.Ma,
                    p.Loai,
                    p.NgaySX,
                    p.DonVi,
                    p.SoLuong,
                    p.GiaBan,
                    p.TenNhaCungCap,
                    p.Barcode,
                    Hinh = LoadImageSafe(p.HinhPath),
                    p.GhiChu
                }).ToList();

                DataGridViewKho.DataSource = product;
            }
        }
        private void DataGridViewKho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu click vào cột xóa
            if (e.ColumnIndex == DataGridViewKho.Columns["XoaColumn"].Index && e.RowIndex >= 0)
            {
                // Lấy mã sản phẩm từ dòng được chọn
                var row = DataGridViewKho.Rows[e.RowIndex];
                var maSP = row.Cells["MaSPColumn"].Value?.ToString();

                if (string.IsNullOrEmpty(maSP))
                {
                    MessageBox.Show("Không thể lấy mã sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Xác nhận xóa
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa sản phẩm có mã {maSP}?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    XoaSanPham(maSP);
                }
            }
        }

        private void XoaSanPham(string maSP)
        {
            try
            {
                using (var db = new MiniStoreContext())
                {
                    // Tìm sản phẩm cần xóa
                    var sanPham = db.SANPHAMs.FirstOrDefault(p => p.MASP == maSP);

                    if (sanPham == null)
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Lưu thông tin sản phẩm trước khi xóa để ghi log
                    var tenSanPham = sanPham.TENSP ?? "";
                    var soLuong = sanPham.SOLUONG ?? 0;
                    var giaBan = sanPham.GIABAN ?? 0;

                    // Lấy thông tin người xóa
                    // Lưu username để tránh lỗi mã hóa Unicode khi cột NGUOINHAP là VARCHAR
                    var nguoiNhap = !string.IsNullOrWhiteSpace(MiniStore.Class.UserSession.Username)
                        ? MiniStore.Class.UserSession.Username
                        : (!string.IsNullOrWhiteSpace(MiniStore.Class.UserSession.FullName)
                            ? MiniStore.Class.UserSession.FullName
                            : "Không rõ");

                    // Kiểm tra hàng trưng bày
                    var hangTrungBay = db.HANGTRUNGBAYs.FirstOrDefault(h => h.MASP == maSP);

                    if (hangTrungBay != null && hangTrungBay.SOLUONG_TRENKE > 0)
                    {
                        // Nếu còn số lượng trên kệ > 0: Chỉ xóa số lượng trong kho (SOLUONG = 0)
                        // Giữ nguyên sản phẩm để tiếp tục bán hàng trên kệ
                        sanPham.SOLUONG = 0;
                        hangTrungBay.TRANGTHAI = "Chỉ còn trên kệ";
                        db.SaveChanges();

                        // Ghi log xóa khỏi kho
                        var log = new MiniStore.Models.LogCTHDNhap
                        {
                            MAHDNHAP = null,
                            MASP = maSP,
                            SOLUONGTN = 0,
                            DONGIANHAP = giaBan,
                            GHICHU = "Xóa sản phẩm khỏi kho",
                            NGUOINHAP = nguoiNhap,
                            LoggedAt = DateTime.Now
                        };
                        db.LogCTHDNhaps.Add(log);
                        db.SaveChanges();

                        MessageBox.Show(
                            $"Đã xóa sản phẩm khỏi kho!\n" +
                            $"Sản phẩm vẫn còn {hangTrungBay.SOLUONG_TRENKE} sản phẩm trên kệ để tiếp tục bán.\n" +
                            $"Trạng thái: Chỉ còn trên kệ",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Nếu không còn hàng trên kệ: Xóa hoàn toàn sản phẩm
                        // Cần xóa HANGTRUNGBAY trước (nếu tồn tại) để tránh lỗi Entity Framework
                        // Trigger sẽ tự động xử lý việc xóa SANPHAM
                        // Cần tắt FOREIGN KEY tạm thời nếu sản phẩm có trong hóa đơn

                        // Ghi log xóa hoàn toàn trước khi xóa
                        var log = new MiniStore.Models.LogCTHDNhap
                        {
                            MAHDNHAP = null,
                            MASP = maSP,
                            SOLUONGTN = soLuong,
                            DONGIANHAP = giaBan,
                            GHICHU = "Xóa sản phẩm",
                            NGUOINHAP = nguoiNhap,
                            LoggedAt = DateTime.Now
                        };
                        db.LogCTHDNhaps.Add(log);
                        db.SaveChanges();

                        // Xóa HANGTRUNGBAY trước nếu tồn tại (khi SOLUONG_TRENKE = 0)
                        if (hangTrungBay != null)
                        {
                            // Detach entity để tránh conflict
                            db.Entry(hangTrungBay).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                            // Xóa trực tiếp qua SQL với parameter để tránh lỗi key và SQL injection
                            db.Database.ExecuteSqlRaw("DELETE FROM HANGTRUNGBAY WHERE MASP = {0}", maSP);
                        }

                        // Kiểm tra xem sản phẩm có trong hóa đơn không
                        bool coTrongHoaDon = db.CHITIETHDBANs.Any(c => c.MASP == maSP) ||
                                            db.CHITIETHDNHAPs.Any(c => c.MASP == maSP);

                        if (coTrongHoaDon)
                        {
                            // Tắt FOREIGN KEY tạm thời để xóa sản phẩm (giữ lại lịch sử hóa đơn)
                            db.Database.ExecuteSqlRaw("ALTER TABLE CHITIETHDBAN NOCHECK CONSTRAINT FK_CHITIETHDBAN_SANPHAM");
                            db.Database.ExecuteSqlRaw("ALTER TABLE CHITIETHDNHAP NOCHECK CONSTRAINT FK_CHITIETHDNHAP_SANPHAM");

                            try
                            {
                                // Detach entity để tránh conflict với navigation property
                                db.Entry(sanPham).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                                // Xóa sản phẩm qua SQL với parameter (trigger sẽ tự động xử lý)
                                db.Database.ExecuteSqlRaw("DELETE FROM SANPHAM WHERE MASP = {0}", maSP);

                                MessageBox.Show("Xóa sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            finally
                            {
                                // Bật lại FOREIGN KEY
                                db.Database.ExecuteSqlRaw("ALTER TABLE CHITIETHDBAN WITH CHECK CHECK CONSTRAINT FK_CHITIETHDBAN_SANPHAM");
                                db.Database.ExecuteSqlRaw("ALTER TABLE CHITIETHDNHAP WITH CHECK CHECK CONSTRAINT FK_CHITIETHDNHAP_SANPHAM");
                            }
                        }
                        else
                        {
                            // Không có trong hóa đơn, xóa bình thường
                            // Detach entity để tránh conflict với navigation property
                            db.Entry(sanPham).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                            // Xóa sản phẩm qua SQL với parameter (trigger sẽ tự động xử lý)
                            db.Database.ExecuteSqlRaw("DELETE FROM SANPHAM WHERE MASP = {0}", maSP);

                            MessageBox.Show("Xóa sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                    // Reload danh sách sản phẩm
                    LoadDanhSachSanPham();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa sản phẩm: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddImageBar_Click(object sender, EventArgs e)
        {
            string folder = Path.Combine(Application.StartupPath, "Barcodes");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            using (var db = new MiniStoreContext())
            {
                var list = db.SANPHAMs.ToList();

                foreach (var sp in list)
                {
                    if (string.IsNullOrEmpty(sp.BARCODE))
                        continue;

                    string filePath = Path.Combine(folder, $"{sp.BARCODE}.png");

                    // Generate image
                    var writer = new BarcodeWriter
                    {
                        Format = BarcodeFormat.CODE_128,
                        Options = new EncodingOptions
                        {
                            Width = 400,
                            Height = 150,
                            Margin = 1,
                            PureBarcode = false
                        }
                    };

                    Bitmap bmp = writer.Write(sp.BARCODE);
                    bmp.Save(filePath, ImageFormat.Png);

                    sp.BARCODE_IMAGE = filePath;
                }

                db.SaveChanges();
            }

            MessageBox.Show("Đã tạo xong toàn bộ ảnh barcode!", "Done");
        }

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnXemLog_Click(object sender, EventArgs e)
        {
            var parent = this.Parent;
            if (parent == null) return;

            // Hiển thị UC_LogThemSanPham đè lên UC_Kho (bring to front)
            var ucLogThemSanPham = new UC_LogThemSanPham
            {
                Dock = DockStyle.Fill,
                Tag = this // để UC_LogThemSanPham biết quay lại UC_Kho
            };

            ucLogThemSanPham.Disposed += (s, ev) =>
            {
                if (this.IsDisposed) return;
                this.Visible = true;
                this.BringToFront();
            };

            // Load dữ liệu log từ database
            LoadLogData(ucLogThemSanPham);

            parent.Controls.Add(ucLogThemSanPham);
            ucLogThemSanPham.BringToFront();
            this.Visible = false;
        }

        private string GetStatusFromGhiChu(string ghiChu)
        {
            if (string.IsNullOrWhiteSpace(ghiChu))
                return "Thêm";

            var normalized = ghiChu.Trim().ToLowerInvariant();
            if (normalized.Contains("cập nhật"))
                return "Cập nhật";
            if (normalized.Contains("xóa"))
                return "Xóa";
            
            return "Thêm";
        }

        private void LoadLogData(UC_LogThemSanPham ucLog)
        {
            try
            {
                using (var db = new MiniStoreContext())
                {
                    // Query log entries với LINQ method syntax
                    var logEntries = db.LogCTHDNhaps
                        .GroupJoin(db.HDNHAPs, 
                            log => log.MAHDNHAP, 
                            hd => hd.MAHDNHAP, 
                            (log, hdGroup) => new { log, hdGroup })
                        .SelectMany(x => x.hdGroup.DefaultIfEmpty(), 
                            (x, hd) => new { x.log, hd })
                        .GroupJoin(db.SANPHAMs, 
                            x => x.log.MASP, 
                            sp => sp.MASP, 
                            (x, spGroup) => new { x.log, x.hd, spGroup })
                        .SelectMany(x => x.spGroup.DefaultIfEmpty(), 
                            (x, sp) => new { x.log, x.hd, sp })
                        .GroupJoin(db.NHACUNGCAPs, 
                            x => x.hd != null ? x.hd.MANCC : null, 
                            ncc => ncc.MANCC, 
                            (x, nccGroup) => new { x.log, x.hd, x.sp, nccGroup })
                        .SelectMany(x => x.nccGroup.DefaultIfEmpty(), 
                            (x, ncc) => new { x.log, x.hd, x.sp, ncc })
                        .GroupJoin(db.TAIKHOANs, 
                            x => x.hd != null ? x.hd.USERNAME : x.log.NGUOINHAP, 
                            tk => tk.USERNAME, 
                            (x, tkGroup) => new { x.log, x.hd, x.sp, x.ncc, tkGroup })
                        .SelectMany(x => x.tkGroup.DefaultIfEmpty(), 
                            (x, tk) => new { x.log, x.hd, x.sp, x.ncc, tk })
                        .GroupJoin(db.NGUOIDUNGs, 
                            x => x.tk != null ? x.tk.USERNAME : x.log.NGUOINHAP, 
                            nd => nd.USERNAME, 
                            (x, ndGroup) => new { x.log, x.hd, x.sp, x.ncc, x.tk, ndGroup })
                        .SelectMany(x => x.ndGroup.DefaultIfEmpty(), 
                            (x, nd) => new { x.log, x.hd, x.sp, x.ncc, x.tk, nd })
                        .OrderByDescending(x => x.log.LoggedAt)
                        .Select(x => new
                        {
                            x.log.MASP,
                            x.log.SOLUONGTN,
                            x.log.DONGIANHAP,
                            x.log.LoggedAt,
                            TenSanPham = x.sp != null ? x.sp.TENSP : "",
                            TenNhaCungCap = x.ncc != null ? x.ncc.TENNCC : "",
                            TenNhanVien = x.nd != null ? x.nd.HOTEN : (x.log.NGUOINHAP ?? ""),
                            GhiChu = x.log.GHICHU ?? ""
                        })
                        .ToList();

                    var entries = logEntries.Select(l => new UC_LogThemSanPham.LogEntry
                    {
                        EmployeeName = !string.IsNullOrEmpty(l.TenNhanVien) ? l.TenNhanVien : "Không xác định",
                        ProductName = !string.IsNullOrEmpty(l.TenSanPham) ? l.TenSanPham : "Không xác định",
                        ProductCode = l.MASP ?? "",
                        Quantity = l.SOLUONGTN ?? 0,
                        Price = l.DONGIANHAP ?? 0,
                        SupplierName = !string.IsNullOrEmpty(l.TenNhaCungCap) ? l.TenNhaCungCap : "Không xác định",
                        AddedAt = l.LoggedAt ?? DateTime.Now,
                        Status = GetStatusFromGhiChu(l.GhiChu)
                    }).ToList();

                    ucLog.SetLogs(entries);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu log: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
