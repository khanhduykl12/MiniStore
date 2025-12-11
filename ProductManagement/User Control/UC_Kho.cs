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

namespace MiniStore.User_Control
{
    public partial class UC_Kho : UserControl
    {
        public UC_Kho()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Dispose();
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
    }
}
