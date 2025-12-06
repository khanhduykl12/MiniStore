using Guna.UI2.WinForms;
using Microsoft.EntityFrameworkCore;
using MiniStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MiniShop.User_Control
{
    public partial class UC_ThongKe : UserControl
    {
        private List<HDBAN> currentInvoices = new List<HDBAN>();
        private int currentIndex = 0;
        public UC_ThongKe()
        {
            InitializeComponent();
        }

        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnFill_Click(object sender, EventArgs e)
        {
            bool hasLap = tryGetSelectedId(cboNguoiLap, out int idLap);
            bool hasProduct = tryGetSelectedProduct(out string maSP);

            System.Diagnostics.Debug.WriteLine("SelectedValue = " + cboMaSanPham.SelectedValue);
            System.Diagnostics.Debug.WriteLine("maSP = " + maSP);
            System.Diagnostics.Debug.WriteLine("hasProduct = " + hasProduct);

            DateOnly tuNgay = DateOnly.FromDateTime(dateTimePickerTuNgay.Value);
            DateOnly denNgay = DateOnly.FromDateTime(dateTimePickerDenNgay.Value);

            using (var db = new MiniStoreContext())
            {
                var query = db.HDBANs.Include(h => h.CHITIETHDBANs).Where(h => h.NGAYLAP >= tuNgay && h.NGAYLAP <= denNgay).ToList();
                if (hasLap && hasProduct)
                {
                    bool exists = db.HDBANs
                        .Any(h => h.NGUOILAP_ID == idLap &&
                                  h.CHITIETHDBANs.Any(ct => ct.MASP == maSP));

                    if (!exists)
                    {
                        lblErrorTrungKhop.Text = "Người lập này không có hóa đơn với sản phẩm đã chọn!";
                        panelHoaDon.Visible = false;
                        panelChiTiet.Visible = false;
                        pnlTotal.Visible = false;
                        return;
                    }
                }
                if (hasLap)
                {
                    query = query.Where(h => h.NGUOILAP_ID == idLap).ToList();
                    if (hasProduct)
                    {
                        query = query.Where(h => h.CHITIETHDBANs.Any(ct => ct.MASP == maSP)).ToList();
                    }
                }
                else if (hasProduct)
                {
                    query = query.Where(h => h.CHITIETHDBANs.Any(ct => ct.MASP == maSP)).ToList();

                }
                // ===== LẤY KẾT QUẢ =====
                var ketQua = query.OrderBy(h => h.NGAYLAP).ToList();

                if (ketQua.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    panelHoaDon.Visible = false;
                    return;
                }
                // ===== HIỂN THỊ LÊN PANEL =====
                currentInvoices = ketQua;
                currentIndex = 0;
                ShowInvoice();
            }
        }
        private bool tryGetSelectedProduct(out string masp)
        {
            masp = "";

            if (cboMaSanPham.SelectedIndex < 0 || cboMaSanPham.SelectedValue == null)
                return false;

            // Nếu SelectedValue đúng kiểu string → dùng luôn
            if (cboMaSanPham.SelectedValue is string s)
            {
                masp = s;
                return true;
            }

            // Nếu SelectedValue là object vô danh → lấy property MASP
            var prop = cboMaSanPham.SelectedValue.GetType().GetProperty("MASP");
            if (prop != null)
            {
                masp = prop.GetValue(cboMaSanPham.SelectedValue)?.ToString();
                return true;
            }

            return false;
        }

        private bool tryGetSelectedId(ComboBox cb, out int id)
        {
            id = 0;
            if (cb.SelectedValue == null)
            {
                return false;
            }
            try
            {
                id = Convert.ToInt32(cb.SelectedValue);
                return true;
            }
            catch
            {
                if (int.TryParse(cb.SelectedValue.ToString(), out id))
                    return true;
                return false;
            }
        }

        private void ShowInvoice()
        {
            if (currentInvoices == null || currentInvoices.Count == 0) return;

            using (var db = new MiniStoreContext())
            {
                var hd = currentInvoices[currentIndex];

                var nguoiLap = db.NGUOIDUNGs.FirstOrDefault(u => u.ID == hd.NGUOILAP_ID);
                var nguoiMua = db.NGUOIDUNGs.FirstOrDefault(u => u.ID == hd.NGUOIMUA_ID);

                lblMaHoaDonThongTin.Text = hd.MAHD;
                lblNgayLapThongTin.Text = hd.NGAYLAP.ToString("yyyy-MM-dd");
                lblNguoiLapThongTin.Text = nguoiLap?.HOTEN ?? "";
                lblNguoiMuaThongTin.Text = nguoiMua?.HOTEN ?? "";
                lblGhiChu.Text = hd.GHICHU ?? "Không";

                btnPrev.Enabled = currentIndex > 0;
                btnNext.Enabled = currentIndex < currentInvoices.Count - 1;

                panelHoaDon.Visible = true;
                lblMaHoaDonChiTiet.Text = lblMaHoaDonThongTin.Text;
                // ⭐ Quan trọng: Load chi tiết tại đây
                loadThongTinHoaDon();
                panelChiTiet.Visible = true;
                pnlTotal.Visible = true;
                btnExportReport.Enabled = true;
            }
        }

        bool isDateSelected = false;
        private void dateTimePickerTuNgay_ValueChanged(object sender, EventArgs e)
        {
            isDateSelected = true;
            validateTuNgay();
            // Chỉ kiểm tra ngày "Từ" không được lớn hơn ngày "Đến"
            if (dateTimePickerTuNgay.Value > dateTimePickerDenNgay.Value)
            {
                MessageBox.Show("Ngày 'Từ' không được lớn hơn ngày 'Đến'!",
                                "Lỗi ngày tháng",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                // Reset về ngày "Đến"
                dateTimePickerTuNgay.Value = dateTimePickerDenNgay.Value;
            }
        }
        private void validateTuNgay()
        {
            if (!isDateSelected)
            {
                MessageBox.Show("Bạn chưa chọn ngày lập hóa đơn từ ngày", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void dateTimePickerDenNgay_ValueChanged(object sender, EventArgs e)
        {
            isDateSelected = true;
            validateDenNgay();
            // Chỉ kiểm tra ngày "Đến" không được nhỏ hơn ngày "Từ"
            if (dateTimePickerDenNgay.Value < dateTimePickerTuNgay.Value)
            {
                MessageBox.Show("Ngày 'Đến' không được nhỏ hơn ngày 'Từ'!",
                                "Lỗi ngày tháng",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                // Reset về ngày "Từ"
                dateTimePickerDenNgay.Value = dateTimePickerTuNgay.Value;
            }
        }
        private void validateDenNgay()
        {
            if (!isDateSelected)
            {
                MessageBox.Show("Bạn chưa chọn ngày lập hóa đơn đến ngày", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cboNguoiMua_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


        private void UC_ThongKe_Load(object sender, EventArgs e)
        {
            lblErrorTrungKhop.Text = string.Empty;
            lblErrorTuNgay.Text = string.Empty;
            lblErrorDenNgay.Text = string.Empty;
            panelHoaDon.Visible = false;
            panelChiTiet.Visible = false;
            pnlTotal.Visible = false;
            btnExportReport.Enabled = false;
            using (var db = new MiniStoreContext())
            {

                var dataNguoiLap = db.HDBANs.Select(u => u.NGUOILAP_ID).Distinct().ToList();
                var list = db.NGUOIDUNGs.Where(u => dataNguoiLap.Contains(u.ID)).Select(u => new
                {
                    ID = u.ID,
                    Name = u.HOTEN
                }).ToList();

                cboNguoiLap.DataSource = list;
                cboNguoiLap.DisplayMember = "Name";
                cboNguoiLap.ValueMember = "ID";
                cboNguoiLap.SelectedIndex = -1;

                var dataMaSP = db.CHITIETHDBANs.Select(product => product.MASP).Distinct().ToList();
                var listNameSP = db.SANPHAMs.Where(u => dataMaSP.Contains(u.MASP)).Select(u => new
                {
                    MASP = u.MASP,
                    TENSP = u.TENSP,
                }).ToList();
                cboMaSanPham.DataSource = listNameSP;
                cboMaSanPham.DisplayMember = "TENSP";
                cboMaSanPham.ValueMember = "MASP";
                cboMaSanPham.SelectedIndex = -1;
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (currentIndex < currentInvoices.Count - 1)
            {
                currentIndex++;
                ShowInvoice();
            }
        }

        private void btnPrev_Click_1(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                ShowInvoice();
            }
        }

        private void flpdesc_Paint(object sender, PaintEventArgs e)
        {

        }
        private void loadThongTinHoaDon()
        {
            flpdesc.Controls.Clear();
            lblTotal.Text = "0";
            using (var db = new MiniStoreContext())
            {
                var chiTiet = db.CHITIETHDBANs.Where(desc => desc.MAHD == lblMaHoaDonThongTin.Text).Select(inf => new
                {
                    MASP = inf.MASP,
                    TENSP = inf.MASPNavigation.TENSP,
                    SOLUONG = inf.SOLUONG,
                    DONGIA = inf.DONGIA,
                    THANHTIEN = inf.THANHTIEN,
                }).ToList();
                decimal? tongTien = 0;
                foreach (var item in chiTiet)
                {
                    Panel row = new Panel();
                    row.Height = 40;
                    row.Dock = DockStyle.Top;
                    row.Width = flpdesc.ClientSize.Width;
                    row.BackColor = System.Drawing.Color.Transparent;

                    Label lblMaSP = new Label();
                    lblMaSP.Text = item.MASP;
                    lblMaSP.AutoSize = false;
                    lblMaSP.Width = 150;
                    lblMaSP.Location = new Point(10, 10);
                    // Tên SP
                    Label lblTen = new Label();
                    lblTen.Text = item.TENSP;
                    lblTen.AutoSize = false;
                    lblTen.Width = 250;
                    lblTen.Location = new Point(270, 10);

                    // Số lượng
                    Label lblSL = new Label();
                    lblSL.Text = item.SOLUONG.ToString();
                    lblSL.AutoSize = false;
                    lblSL.Width = 90;
                    lblSL.Location = new Point(580, 10);
                    lblSL.TextAlign = ContentAlignment.MiddleCenter;

                    // Đơn giá
                    Label lblDonGia = new Label();
                    lblDonGia.Text = (item.DONGIA ?? 0).ToString("#,##0") + " đ";
                    lblDonGia.AutoSize = false;
                    lblDonGia.Width = 120;
                    lblDonGia.Location = new Point(800, 10);
                    lblDonGia.TextAlign = ContentAlignment.MiddleRight;

                    // Thành tiền
                    Label lblThanhTien = new Label();
                    lblThanhTien.Text = (item.THANHTIEN ?? 0).ToString("#,##0") + " đ";
                    lblThanhTien.AutoSize = false;
                    lblThanhTien.Width = 150;
                    lblThanhTien.Location = new Point(1020, 10);
                    lblThanhTien.TextAlign = ContentAlignment.MiddleRight;

                    // Add vào panel row
                    row.Controls.Add(lblMaSP);
                    row.Controls.Add(lblTen);
                    row.Controls.Add(lblSL);
                    row.Controls.Add(lblDonGia);
                    row.Controls.Add(lblThanhTien);

                    // Cộng tổng
                    tongTien += item.THANHTIEN;

                    // Add panel vào FlowLayoutPanel
                    flpdesc.Controls.Add(row);
                }
                // Hiển thị tổng tiền
                lblTotal.Text = (tongTien ?? 0).ToString("#,##0") + " đ";

            }

        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // 1) Tắt event để tránh can thiệp khi reset
            cboNguoiLap.SelectedIndexChanged -= cboNguoiLap_SelectedIndexChanged;
            cboMaSanPham.SelectedIndexChanged -= cboMaSanPham_SelectedIndexChanged;

            // 2) Reset UI trước
            panelHoaDon.Visible = false;
            panelChiTiet.Visible = false;
            pnlTotal.Visible = false;

            dateTimePickerDenNgay.Value = DateTime.Today;
            dateTimePickerTuNgay.Value = DateTime.Today;

            lblErrorTrungKhop.Text = string.Empty;
            lblErrorTuNgay.Text = string.Empty;
            lblErrorDenNgay.Text = string.Empty;

            // 3) Tắt datasource và clear selection
            cboNguoiLap.DataSource = null;
            cboNguoiLap.Items.Clear();
            cboNguoiLap.SelectedIndex = -1;
            cboNguoiLap.Text = "";

            cboMaSanPham.DataSource = null;
            cboMaSanPham.Items.Clear();
            cboMaSanPham.SelectedIndex = -1;
            cboMaSanPham.Text = "";

            // 4) Load lại datasource
            using (var db = new MiniStoreContext())
            {
                var dataNguoiLap = db.HDBANs.Select(u => u.NGUOILAP_ID).Distinct().ToList();
                var list = db.NGUOIDUNGs
                    .Where(u => dataNguoiLap.Contains(u.ID))
                    .Select(u => new { ID = u.ID, Name = u.HOTEN })
                    .ToList();

                cboNguoiLap.DataSource = list;
                cboNguoiLap.DisplayMember = "Name";
                cboNguoiLap.ValueMember = "ID";
                
                // QUAN TRỌNG: Set SelectedIndex = -1 SAU KHI đã set DataSource
                cboNguoiLap.SelectedIndex = -1;

                var dataMaSP = db.CHITIETHDBANs.Select(p => p.MASP).Distinct().ToList();
                var listNameSP = db.SANPHAMs
                    .Where(u => dataMaSP.Contains(u.MASP))
                    .Select(u => new { MASP = u.MASP, TENSP = u.TENSP })
                    .ToList();

                cboMaSanPham.DataSource = listNameSP;
                cboMaSanPham.DisplayMember = "TENSP";
                cboMaSanPham.ValueMember = "MASP";
                
                // QUAN TRỌNG: Set SelectedIndex = -1 SAU KHI đã set DataSource
                cboMaSanPham.SelectedIndex = -1;
            }

            // 5) Bật lại event SAU KHI đã reset xong
            cboNguoiLap.SelectedIndexChanged += cboNguoiLap_SelectedIndexChanged;
            cboMaSanPham.SelectedIndexChanged += cboMaSanPham_SelectedIndexChanged;
        }


        private void btnExportReport_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem có hóa đơn nào đang được lọc không
                if (currentInvoices == null || currentInvoices.Count == 0)
                {
                    MessageBox.Show("Không có hóa đơn nào để xuất báo cáo!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy hóa đơn hiện tại
                var currentInvoice = currentInvoices[currentIndex];
                string maHD = currentInvoice.MAHD;

                // Xuất PDF bằng QuestPDF
                ExportInvoiceToPDF(maHD);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất báo cáo: {ex.Message}\n\nChi tiết: {ex.StackTrace}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportInvoiceToPDF(string maHD)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveDialog.FileName = $"HoaDon_{maHD}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    InvoiceData? invoiceData = null;

                    using (var db = new MiniStoreContext())
                    {
                        // Lấy thông tin hóa đơn
                        var hd = db.HDBANs
                            .Include(h => h.NGUOILAP)
                            .Include(h => h.NGUOIMUA)
                            .Include(h => h.CHITIETHDBANs)
                                .ThenInclude(ct => ct.MASPNavigation)
                            .FirstOrDefault(h => h.MAHD == maHD);

                        if (hd == null)
                        {
                            MessageBox.Show("Không tìm thấy hóa đơn!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Tính tổng tiền
                        decimal tongTien = hd.CHITIETHDBANs.Sum(ct => ct.THANHTIEN ?? 0);

                        // Tạo danh sách chi tiết
                        var chiTietList = hd.CHITIETHDBANs.Select(ct => new ChiTietItem
                        {
                            MASP = ct.MASP,
                            TENSP = ct.MASPNavigation?.TENSP ?? "",
                            SOLUONG = ct.SOLUONG ?? 0,
                            DONGIA = ct.DONGIA ?? 0,
                            THANHTIEN = ct.THANHTIEN ?? 0
                        }).ToList();

                        invoiceData = new InvoiceData
                        {
                            MAHD = hd.MAHD,
                            NGAYLAP = hd.NGAYLAP.ToDateTime(TimeOnly.MinValue),
                            TenNguoiLap = hd.NGUOILAP?.HOTEN ?? "",
                            TenNguoiMua = hd.NGUOIMUA?.HOTEN ?? "",
                            GHICHU = hd.GHICHU ?? "",
                            TongTien = tongTien,
                            ChiTiet = chiTietList
                        };
                    }

                    // Tạo PDF bằng QuestPDF
                    QuestPDF.Settings.License = LicenseType.Community;

                    var document = Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            page.Size(PageSizes.A4);
                            page.Margin(2, Unit.Centimetre);
                            page.PageColor(Colors.White);
                            page.DefaultTextStyle(x => x.FontSize(10));

                            page.Header()
                                .Text("HÓA ĐƠN BÁN HÀNG")
                                .FontSize(20)
                                .Bold()
                                .AlignCenter();

                            page.Content()
                                .PaddingVertical(1, Unit.Centimetre)
                                .Column(column =>
                                {
                                    column.Spacing(10);

                                    // Thông tin hóa đơn
                                    column.Item().Row(row =>
                                    {
                                        row.RelativeItem().Text($"Mã hóa đơn: {invoiceData.MAHD}");
                                        row.RelativeItem().Text($"Ngày lập: {invoiceData.NGAYLAP:dd/MM/yyyy}").AlignRight();
                                    });

                                    column.Item().Text($"Người lập: {invoiceData.TenNguoiLap}");
                                    column.Item().Text($"Người mua: {invoiceData.TenNguoiMua}");

                                    if (!string.IsNullOrEmpty(invoiceData.GHICHU))
                                    {
                                        column.Item().Text($"Ghi chú: {invoiceData.GHICHU}");
                                    }

                                    column.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                                    // Bảng chi tiết
                                    column.Item().Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn(2); // Mã SP
                                            columns.RelativeColumn(4); // Tên SP
                                            columns.RelativeColumn(1.5f); // Số lượng
                                            columns.RelativeColumn(2); // Đơn giá
                                            columns.RelativeColumn(2); // Thành tiền
                                        });

                                        // Header
                                        table.Header(header =>
                                        {
                                            header.Cell().Element(x => CellStyle(x)).Text("Mã SP").Bold();
                                            header.Cell().Element(x => CellStyle(x)).Text("Tên sản phẩm").Bold();
                                            header.Cell().Element(x => CellStyle(x)).AlignCenter().Text("Số lượng").Bold();
                                            header.Cell().Element(x => CellStyle(x)).AlignRight().Text("Đơn giá").Bold();
                                            header.Cell().Element(x => CellStyle(x)).AlignRight().Text("Thành tiền").Bold();
                                        });

                                        // Rows
                                        foreach (var item in invoiceData.ChiTiet)
                                        {
                                            table.Cell().Element(x => CellStyle(x)).Text(item.MASP);
                                            table.Cell().Element(x => CellStyle(x)).Text(item.TENSP);
                                            table.Cell().Element(x => CellStyle(x)).AlignCenter().Text(item.SOLUONG.ToString());
                                            table.Cell().Element(x => CellStyle(x)).AlignRight().Text($"{item.DONGIA:N0} đ");
                                            table.Cell().Element(x => CellStyle(x)).AlignRight().Text($"{item.THANHTIEN:N0} đ");
                                        }
                                    });

                                    column.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                                    // Tổng tiền
                                    column.Item().AlignRight().Text($"Tổng tiền: {invoiceData.TongTien:N0} đ")
                                        .FontSize(14)
                                        .Bold();
                                });

                            page.Footer()
                                .AlignCenter()
                                .Text(x =>
                                {
                                    x.Span("Cảm ơn quý khách đã sử dụng dịch vụ!").FontSize(8).FontColor(Colors.Grey.Medium);
                                    x.AlignCenter();
                                });
                        });
                    });

                    document.GeneratePdf(saveDialog.FileName);

                    MessageBox.Show(
                        $"Đã xuất PDF thành công!\nĐường dẫn: {saveDialog.FileName}",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất PDF: {ex.Message}\n\nChi tiết: {ex.StackTrace}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(8)
                .Background(Colors.White);
        }

        // Class để lưu dữ liệu hóa đơn
        private class InvoiceData
        {
            public string MAHD { get; set; } = "";
            public DateTime NGAYLAP { get; set; }
            public string TenNguoiLap { get; set; } = "";
            public string TenNguoiMua { get; set; } = "";
            public string GHICHU { get; set; } = "";
            public decimal TongTien { get; set; }
            public List<ChiTietItem> ChiTiet { get; set; } = new List<ChiTietItem>();
        }

        private class ChiTietItem
        {
            public string MASP { get; set; } = "";
            public string TENSP { get; set; } = "";
            public int SOLUONG { get; set; }
            public decimal DONGIA { get; set; }
            public decimal THANHTIEN { get; set; }
        }

        private void cboNguoiLap_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboMaSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
