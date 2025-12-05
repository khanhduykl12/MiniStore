using MiniShop.User_Control.UC_Extra;
using MiniStore.Class;
using MiniStore.User_Control.UC_Extra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniShop.Forms.Forms_Extra
{
    public partial class ShoppingCartStaff : Form
    {
        private UserControl _shippingOverlay;
        private UserControl _ScanbarcodeOverplay;
        private Control _cartView;
        public ShoppingCartStaff()
        {
            InitializeComponent();
            _cartView = panelCartView;
        }
        private void ShoppingCartStaff_Load(object sender, EventArgs e)
        {
            LoadCart();
            UpdateSum();
        }
        private void LoadCart()
        {
            flpCart.SuspendLayout();
            flpCart.Controls.Clear();

            foreach (var item in CartService.Items)
            {
                var cart = new UC_ShopPCart
                {
                    Width = flpCart.ClientSize.Width - 30
                };
                cart.BlindDuLieu(item);
                flpCart.Controls.Add(cart);
            }
            flpCart.ResumeLayout();

        }

        public void UpdateSum()
        {
            decimal productsSum = CartService.GetTotal();
            decimal transport = productsSum * 0.10m;
            decimal discount;
            discount = 10000m;
            decimal total = productsSum + transport - discount;
            if (total < 0m)
            {
                total = 0m;
                discount = 0m;
            }

            lblProductSum.Text = $"{productsSum:N0} đ";
            lblTransport.Text = $"{transport:N0} đ";
            lblDiscount.Text = $"{discount:N0} đ";
            lblTotal.Text = $"{total:N0} đ";
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void AddUCShip(UserControl uc)
        {

            uc.Dock = DockStyle.Fill;
            panelLeft.Controls.Add(uc);
            uc.BringToFront();
            _shippingOverlay = uc;
        }
        private void AddUCScan(UserControl uc)
        {

            uc.Dock = DockStyle.Fill;
            panelRight.Controls.Add(uc);
            uc.BringToFront();
            _ScanbarcodeOverplay = uc;
        }

        private void btnDatHang_Click(object sender, EventArgs e)
        {
            if (radMoMo.Checked || radBank.Checked)
            {
                var uc = new UC_Shipping();
                AddUCShip(uc);
            }
            else
            {
                var uc = new UC_ShippingCOD();
                AddUCShip(uc);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (flpCart.Controls.Count == 0) return;
            var results = MessageBox.Show("Bạn có muốn hủy đơn hàng không", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (results != DialogResult.Yes) return;
            CartService.Clear();
            flpCart.SuspendLayout();
            flpCart.Controls.Clear();
            flpCart.ResumeLayout();
            UpdateSum();
            if (_shippingOverlay != null)
            {
                panelLeft.Controls.Remove(_shippingOverlay);
                _shippingOverlay = null;
            }
        }
        public void RemoveItemAndRefresh(string maSP)
        {
            CartService.RemoveItem(maSP);
            LoadCart();
            UpdateSum();
            if (CartService.Items.Count == 0 && _shippingOverlay != null)
            {
                panelLeft.Controls.Remove(_shippingOverlay);
                _shippingOverlay = null;
            }
        }

        private void btnScanBarCode_Click(object sender, EventArgs e)
        {
            var uc = new UC_ScanBarCode();
            uc.ProductScanned += OnProductScanned;
            AddUCScan(uc);
        }
        private void OnProductScanned(object sender, UC_ScanBarCode.ProductScannedEventArgs e)
        {
            // Tìm trong giỏ đã có chưa
            var existing = CartService.Items.FirstOrDefault(x => x.MaSP == e.MaSP);
            if (existing != null)
            {
                existing.SoLuong += 1;
            }
            else
            {
                CartService.AddItem(new CartItem
                {
                    MaSP = e.MaSP,
                    TenSP = e.TenSP,
                    Gia = e.GiaBan,
                    SoLuong = 1,
                    DVT = e.DVT,
                    Hinh = e.Hinh
                });
            }

            LoadCart();
            UpdateSum();

            // ẩn overlay sau khi lưu (nếu muốn quét nhiều lần thì có thể bỏ đoạn này)
            if (_ScanbarcodeOverplay is UC_ScanBarCode scan)
            {
                scan.CloseScanner();
            }
            if (_ScanbarcodeOverplay != null)
            {
                panelRight.Controls.Remove(_ScanbarcodeOverplay);
                _ScanbarcodeOverplay = null;
            }
        }

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void btnHuy_Click_1(object sender, EventArgs e)
        {
            if (flpCart.Controls.Count == 0) return;
            var results = MessageBox.Show("Bạn có muốn hủy đơn hàng không", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (results != DialogResult.Yes) return;
            CartService.Clear();
            flpCart.SuspendLayout();
            flpCart.Controls.Clear();
            flpCart.ResumeLayout();
            UpdateSum();
            if (_shippingOverlay != null)
            {
                panelLeft.Controls.Remove(_shippingOverlay);
                _shippingOverlay = null;
            }
        }

        private void btnDatHang_Click_1(object sender, EventArgs e)
        {
            if (radMoMo.Checked || radBank.Checked)
            {
                var uc = new UC_Shipping();
                AddUCShip(uc);
            }
            else
            {
                var uc = new UC_ShippingCOD();
                AddUCShip(uc);
            }
        }
    }
}
