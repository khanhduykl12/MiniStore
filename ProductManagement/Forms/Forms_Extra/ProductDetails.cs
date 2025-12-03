using Microsoft.EntityFrameworkCore;
using MiniStore.Class;
using MiniStore.Models;
using System.ComponentModel;
using System.Data;

namespace MiniStore.Forms.Forms_Extra
{

    public partial class ProductDetails : Form
    {
        private MiniStoreContext db = new MiniStoreContext();
        private List<SANPHAM> fillsp = new();
        private string _MaSP;
        private string imageFile;
        public ProductDetails(string MaSP)
        {
            InitializeComponent();
            _MaSP = MaSP;


        }
        [Category("Product"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true), DefaultValue("")]
        public string LoaiHang
        {
            get => lblLoaiHang.Text;
            set => lblLoaiHang.Text = value ?? "";
        }
        [Category("Product"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true), DefaultValue("")]
        public string Ten
        {
            get => lblTen.Text;
            set => lblTen.Text = value ?? "";
        }
        [Category("Product"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true), DefaultValue("")]
        public string DVT
        {
            get => lblDVT.Text;
            set => lblDVT.Text = value ?? "";
        }
        [Category("Product"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true), DefaultValue(typeof(decimal), "0")]
        public decimal Gia
        {
            get => decimal.TryParse(lblGia.Tag?.ToString(), out var v) ? v : 0m;
            set { lblGia.Tag = value; lblGia.Text = $"{value:N0} đ"; }
        }
        [Category("Product"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true), DefaultValue(typeof(decimal), "0")]
        public decimal SoLuongTrenKe
        {
            get => int.TryParse(lblSoLuongTrenKe.Tag?.ToString(), out var v) ? v : 0;
            set { lblSoLuongTrenKe.Tag = value; lblSoLuongTrenKe.Text = $"Số Lượng Còn Trên Kệ: {value}"; }
        }
        [Category("Product"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true), DefaultValue("")]
        public string ImageFile
        {
            get => imageFile;
            set
            {
                imageFile = value ?? "";
                var p = Path.Combine(Application.StartupPath, "ImagesProduct", imageFile);
                if (File.Exists(p))
                {
                    using (var img = Image.FromFile(p))
                        picProduct.Image = (Image)img.Clone();
                }
                else
                {
                    picProduct.Image = Properties.Resources.no_image;
                }
                picProduct.Tag = imageFile;
            }
        }

        private async void ProductDetails_Load(object sender, EventArgs e)
        {
            var sp = await db.SANPHAMs
                .AsNoTracking()
                .Where(x => x.MASP == _MaSP)
                .Select(x => new
                {
                    x.MALOAINavigation.TENLOAI,
                    x.TENSP,
                    x.DVT,
                    x.GIABAN,
                    x.HANGTRUNGBAY.SOLUONG_TRENKE,
                    x.HINH
                }).FirstOrDefaultAsync();
            if (sp == null)
            {
                MessageBox.Show("Không tìm thấy sản phẩm.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            else
            {
                Ten = sp.TENSP ?? "";
                Gia = (decimal)(sp.GIABAN ?? 0);
                DVT = sp.DVT;
                LoaiHang = sp.TENLOAI ?? "";
                SoLuongTrenKe = sp.SOLUONG_TRENKE;
                ImageFile = sp.HINH;
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddCard_Click(object sender, EventArgs e)
        {
            if (int.TryParse(lblSoLuongTrenKe.Tag?.ToString(), out var soLuongTrenKe) && soLuongTrenKe == 0)
            {
                MessageBox.Show("Sản phẩm đã hết hàng trên kệ.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var gia = decimal.TryParse(lblGia.Tag?.ToString(), out var v) ? v : 0m;
            var soluong = (int)numSoLuong.Value;
            var ten = lblTen.Text ?? "";
            var DVT = lblDVT.Text ?? "";
            var HinhFile = string.IsNullOrEmpty(ImageFile) ? "no_image.png" : ImageFile;
            var item = new CartItem
            {
                MaSP = _MaSP,
                TenSP = ten,
                Gia = gia,
                DVT = DVT,
                SoLuong = soluong,
                Hinh = HinhFile
            };
            CartService.AddItem(item);
            MessageBox.Show("Đã thêm sản phẩm vào giỏ !!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
