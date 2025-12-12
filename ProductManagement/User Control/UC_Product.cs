using Microsoft.EntityFrameworkCore;
using MiniShop.Forms.Forms_Extra;
using MiniStore.Forms.Forms_Extra;
using MiniStore.Models;
using MiniStore.User_Control._UC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniStore.User_Control
{
    public partial class UC_Product : UserControl
    {

        private int _page = 0;
        private const int _pageSize = 60;
        private bool _isLoading = false;
        private List<SANPHAM> _filtered = new();
        private CancellationTokenSource _loadCts = new();
        private bool _isDisposed;
        private readonly SemaphoreSlim _filterSemaphore = new SemaphoreSlim(1, 1);
        public string userRole { get; set; }
        public UC_Product(string role)
        {
            InitializeComponent();
            userRole = role;
            btnKho.Visible = (role == "ADMIN" || role == "NV");
            EnableDoubleBuffer(this);
            EnableDoubleBuffer(flpProduct);

            flpProduct.Scroll += FlpProduct_Scroll;

            this.Disposed += (s, e) => DisposeResources();
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
            await using var db = new MiniStoreContext();

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
            if (_isDisposed) return;

            // Cancel previous operation before starting new one
            try
            {
                _loadCts?.Cancel();
            }
            catch { }

            try
            {
                _loadCts?.Dispose();
            }
            catch { }

            _loadCts = new CancellationTokenSource();
            var ct = _loadCts.Token;

            // Use semaphore to prevent concurrent execution
            try
            {
                await _filterSemaphore.WaitAsync(ct);
            }
            catch (ObjectDisposedException)
            {
                // Semaphore was disposed, return immediately
                return;
            }
            catch (OperationCanceledException)
            {
                // Operation was canceled
                return;
            }

            try
            {
                // Check again after acquiring semaphore
                if (_isDisposed || ct.IsCancellationRequested) return;

                await using var db = new MiniStoreContext();

                var selected = cboAllCate.SelectedItem as LOAISANPHAM;
                string maloai = selected?.MALOAI ?? "ALL";

                IQueryable<SANPHAM> q = db.SANPHAMs.AsNoTracking();

                if (!string.IsNullOrEmpty(maloai) && maloai != "ALL")
                    q = q.Where(x => x.MALOAI == maloai);

                _filtered = await q
                    .OrderBy(x => x.TENSP)
                    .ToListAsync(ct);

                if (ct.IsCancellationRequested || _isDisposed) return;

                _page = 0;

                flpProduct.SuspendLayout();
                flpProduct.Controls.Clear();
                flpProduct.ResumeLayout();

                await LoadNextPageAsync(ct);
            }
            catch (OperationCanceledException)
            {
                // Task was canceled, this is expected when user changes filter quickly
                // Just return silently
                return;
            }
            finally
            {
                // Only release if semaphore hasn't been disposed
                if (!_isDisposed)
                {
                    try
                    {
                        _filterSemaphore.Release();
                    }
                    catch (ObjectDisposedException)
                    {
                        // Semaphore was disposed, ignore
                    }
                }
            }
        }

        private async Task LoadNextPageAsync(CancellationToken ct = default)
        {
            if (_isLoading || _isDisposed || ct.IsCancellationRequested) return;

            var skip = _page * _pageSize;
            if (skip >= _filtered.Count) return;

            _isLoading = true;
            try
            {
                if (ct.IsCancellationRequested || _isDisposed) return;

                var chunk = _filtered.Skip(skip).Take(_pageSize).ToList();
                _page++;

                flpProduct.SuspendLayout();
                foreach (var sp in chunk)
                {
                    if (ct.IsCancellationRequested || _isDisposed) break;

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
                await LoadNextPageAsync(_loadCts.Token);
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
            //ShoppingCart sc = new ShoppingCart();
            ShoppingCartStaff sc = new ShoppingCartStaff();
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

        private void addProduct_Click(object sender, EventArgs e)
        {
            AddProductShelves ps = new AddProductShelves();
            ps.Show();
        }

        private void guna2ButtonPriceFilter_Click(object sender, EventArgs e)
        {

        }

        private void btnFillPrice_Click(object sender, EventArgs e)
        {
            
        }

        private void DisposeResources()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            
            try
            {
                _loadCts?.Cancel();
                _loadCts?.Dispose();
            }
            catch { }
            
            try
            {
                _filterSemaphore?.Dispose();
            }
            catch { }
        }
    }
}
