using Microsoft.EntityFrameworkCore;
using ProductManagement.Forms.Forms_Extra;
using ProductManagement.Models;
using ProductManagement.User_Control._UC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductManagement.User_Control
{
    public partial class UC_Product : UserControl
    {
        private readonly MiniStoreContext db = new MiniStoreContext();

        private int _page = 0;
        private const int _pageSize = 60;
        private bool _isLoading = false;
        private List<SANPHAM> _filtered = new();

        public UC_Product()
        {
            InitializeComponent();

            EnableDoubleBuffer(this);
            EnableDoubleBuffer(flpProduct);

            flpProduct.Scroll += FlpProduct_Scroll;

            this.Disposed += (s, e) => db.Dispose();
        }

        private void EnableDoubleBuffer(Control c)
        {
            c.GetType().GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(c, true, null);
        }

        private async void UC_Product_Load(object sender, EventArgs e)
        {
            var loais = await db.LOAISANPHAMs.AsNoTracking().ToListAsync();
            loais.Insert(0, new LOAISANPHAM { MALOAI = "ALL", TENLOAI = "Tất Cả Loại Hàng" });

            lOAISANPHAMBindingSource.DataSource = loais;

            if (cboAllCate.Items.Count > 0)
                cboAllCate.SelectedIndex = 0;

            cboAllCate.SelectedIndexChanged += CboAllCate_SelectedIndexChanged;

            await ApplyFilterAndResetAsync();
        }

        private async void CboAllCate_SelectedIndexChanged(object sender, EventArgs e)
        {
            await ApplyFilterAndResetAsync();
        }

        private async Task ApplyFilterAndResetAsync()
        {
            var selected = cboAllCate.SelectedItem as LOAISANPHAM;
            string maloai = selected?.MALOAI ?? "ALL";

            IQueryable<SANPHAM> q = db.SANPHAMs.AsNoTracking();

            if (!string.IsNullOrEmpty(maloai) && maloai != "ALL")
                q = q.Where(x => x.MALOAI == maloai);

            _filtered = await q
                .OrderBy(x => x.TENSP)
                .ToListAsync();

            _page = 0;

            flpProduct.SuspendLayout();
            flpProduct.Controls.Clear();
            flpProduct.ResumeLayout();

            await LoadNextPageAsync();
        }

        private async Task LoadNextPageAsync()
        {
            if (_isLoading) return;

            var skip = _page * _pageSize;
            if (skip >= _filtered.Count) return;

            _isLoading = true;
            try
            {
                var chunk = _filtered.Skip(skip).Take(_pageSize).ToList();
                _page++;

                flpProduct.SuspendLayout();
                foreach (var sp in chunk)
                {
                    var card = new UC_ProductCart
                    {
                        MaSP = sp.MASP,
                        Title = sp.TENSP,
                        Price = (decimal)(sp.GIABAN ?? 0),
                        ImageFile = sp.HINH
                    };

                    card.ProductClicked += Card_ProductClicked;

                    flpProduct.Controls.Add(card);
                }
                flpProduct.ResumeLayout();
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async void FlpProduct_Scroll(object sender, ScrollEventArgs e)
        {
            int remaining = flpProduct.DisplayRectangle.Height
                            - (-flpProduct.AutoScrollPosition.Y + flpProduct.ClientSize.Height);

            if (remaining < 400)
                await LoadNextPageAsync();
        }

        public async Task ReloadAllAsync()
        {
            await ApplyFilterAndResetAsync();
        }

        private void btnOption_Click(object sender, EventArgs e)
        {
            var pt = new Point(0, btnOption.Height);
            menuOption.Show(btnOption, pt);
        }

        private void Card_ProductClicked(object sender, EventArgs e)
        {
            if (sender is UC_ProductCart card && !string.IsNullOrWhiteSpace(card.MaSP))
            {
                using var frm = new ProductDetails(card.MaSP);
                frm.ShowDialog();
            }
        }

        private void btnShopCard_Click(object sender, EventArgs e)
        {
            ShoppingCart sc = new ShoppingCart();
            sc.Show();
        }

        private void btnKho_Click(object sender, EventArgs e)
        {
            var parent = this.Parent;
            if (parent == null) return;

            this.Visible = false;

            var ucKho = new UC_Kho
            {
                Dock = DockStyle.Fill,
                Tag = this
            };


            ucKho.Disposed += (s, ev) =>
            {
                if (this.IsDisposed) return;
                this.Visible = true;
                this.BringToFront();
            };

            parent.Controls.Add(ucKho);
            ucKho.BringToFront();
        }
    }
}
