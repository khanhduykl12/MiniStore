using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
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
using System.IO;
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
        
        // Live search properties
        private CancellationTokenSource _searchCts;
        private System.Windows.Forms.Timer _searchDebounceTimer;
        private const int _searchDebounceMs = 300;
        private readonly Dictionary<string, Image> _searchImageCache = new();
        public UC_Product(string role)
        {
            InitializeComponent();
            userRole = role;
            btnKho.Visible = (role == "ADMIN" || role == "NV");
            EnableDoubleBuffer(this);
            EnableDoubleBuffer(flpProduct);

            flpProduct.Scroll += FlpProduct_Scroll;

            // Initialize search dropdown
            InitializeSearchDropdown();
            
            // Initialize debounce timer
            _searchDebounceTimer = new System.Windows.Forms.Timer();
            _searchDebounceTimer.Interval = _searchDebounceMs;
            _searchDebounceTimer.Tick += SearchDebounceTimer_Tick;

            this.Disposed += (s, e) => DisposeResources();
        }

        private void InitializeSearchDropdown()
        {
            // Setup dropdown panel
            pnlSearchResults.Visible = false;
            pnlSearchResults.BringToFront();
            
            // Setup listbox
            lstSearchResults.DrawMode = DrawMode.OwnerDrawFixed;
            lstSearchResults.ItemHeight = 80;
            lstSearchResults.DrawItem += LstSearchResults_DrawItem;
            lstSearchResults.MouseClick += LstSearchResults_MouseClick;
            lstSearchResults.KeyDown += LstSearchResults_KeyDown;
            
            // Hide dropdown when clicking outside
            this.MouseDown += (s, e) => 
            {
                if (pnlSearchResults.Visible && !pnlSearchResults.Bounds.Contains(e.Location))
                {
                    HideSearchDropdown();
                }
            };
            
            txtSearch.Leave += (s, e) => 
            {
                // Delay hiding to allow listbox click
                System.Threading.Tasks.Task.Delay(200).ContinueWith(_ =>
                {
                    if (this.InvokeRequired)
                        this.Invoke(new Action(() => 
                        {
                            var mousePos = this.PointToClient(Control.MousePosition);
                            if (!pnlSearchResults.Bounds.Contains(mousePos) && !txtSearch.Bounds.Contains(mousePos))
                                HideSearchDropdown();
                        }));
                    else
                    {
                        var mousePos = this.PointToClient(Control.MousePosition);
                        if (!pnlSearchResults.Bounds.Contains(mousePos) && !txtSearch.Bounds.Contains(mousePos))
                            HideSearchDropdown();
                    }
                });
            };
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

                if (_isDisposed || this.IsDisposed) return;
                try
                {
                    flpProduct.SuspendLayout();
                    flpProduct.Controls.Clear();
                    flpProduct.ResumeLayout();
                }
                catch (ObjectDisposedException)
                {
                    return;
                }

                await LoadNextPageAsync(ct);
            }
            catch (OperationCanceledException)
            {
                // Task was canceled, this is expected when user changes filter quickly
                // Just return silently
                return;
            }
            catch (SqlException) when (ct.IsCancellationRequested)
            {
                // SQL exception due to cancellation, ignore
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
            if (_isLoading || _isDisposed || ct.IsCancellationRequested || this.IsDisposed) return;

            var skip = _page * _pageSize;
            if (skip >= _filtered.Count) return;

            _isLoading = true;
            try
            {
                if (ct.IsCancellationRequested || _isDisposed || this.IsDisposed) return;

                var chunk = _filtered.Skip(skip).Take(_pageSize).ToList();
                _page++;

                if (_isDisposed || this.IsDisposed) return;

                try
                {
                    flpProduct.SuspendLayout();
                    foreach (var sp in chunk)
                    {
                        if (ct.IsCancellationRequested || _isDisposed || this.IsDisposed) break;

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
                catch (ObjectDisposedException)
                {
                    return;
                }
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
                _searchCts?.Cancel();
                _searchCts?.Dispose();
            }
            catch { }

            try
            {
                _searchDebounceTimer?.Stop();
                _searchDebounceTimer?.Dispose();
            }
            catch { }

            try
            {
                _filterSemaphore?.Dispose();
            }
            catch { }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text?.Trim() ?? "";
            
            if (string.IsNullOrEmpty(searchText))
            {
                HideSearchDropdown();
                return;
            }

            // Reset debounce timer
            _searchDebounceTimer.Stop();
            _searchDebounceTimer.Start();
        }

        private void SearchDebounceTimer_Tick(object sender, EventArgs e)
        {
            _searchDebounceTimer.Stop();
            _ = PerformSearchAsync(txtSearch.Text?.Trim() ?? "");
        }

        private async System.Threading.Tasks.Task PerformSearchAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                HideSearchDropdown();
                return;
            }

            // Cancel previous search
            _searchCts?.Cancel();
            _searchCts?.Dispose();
            _searchCts = new CancellationTokenSource();
            var ct = _searchCts.Token;

            try
            {
                await using var db = new MiniStoreContext();
                
                // Search in product name (case-insensitive)
                var results = await db.SANPHAMs
                    .AsNoTracking()
                    .Where(sp => sp.TENSP != null && sp.TENSP.Contains(searchText))
                    .OrderBy(sp => sp.TENSP)
                    .Take(10) // Limit to 10 results for dropdown
                    .ToListAsync(ct);

                if (ct.IsCancellationRequested || _isDisposed) return;

                // Update UI on main thread
                if (this.InvokeRequired)
                {
                    if (!this.IsDisposed && !_isDisposed)
                    {
                        try
                        {
                            this.Invoke(new Action(() => UpdateSearchResults(results, searchText)));
                        }
                        catch (ObjectDisposedException) { }
                    }
                }
                else
                {
                    if (!this.IsDisposed && !_isDisposed)
                        UpdateSearchResults(results, searchText);
                }
            }
            catch (OperationCanceledException)
            {
                // Search was cancelled, ignore
            }
            catch (SqlException) when (ct.IsCancellationRequested)
            {
                // SQL exception due to cancellation, ignore
            }
            catch (Exception ex)
            {
                // Log error if needed
                System.Diagnostics.Debug.WriteLine($"Search error: {ex.Message}");
            }
        }

        private void UpdateSearchResults(List<SANPHAM> results, string searchText)
        {
            if (_isDisposed || this.IsDisposed) return;

            if (results == null || results.Count == 0)
            {
                HideSearchDropdown();
                return;
            }

            try
            {
                lstSearchResults.Items.Clear();
                foreach (var product in results)
                {
                    lstSearchResults.Items.Add(product);
                }

                // Position dropdown below search box
                var searchLocation = txtSearch.PointToScreen(Point.Empty);
                var parentLocation = this.PointToClient(searchLocation);
                
                pnlSearchResults.Location = new Point(
                    parentLocation.X,
                    parentLocation.Y + txtSearch.Height + 2
                );
                pnlSearchResults.Width = txtSearch.Width;
                pnlSearchResults.Height = Math.Min(results.Count * 60 + 10, 300); // Max height 300px
                
                pnlSearchResults.Visible = true;
                pnlSearchResults.BringToFront();
            }
            catch (ObjectDisposedException)
            {
                // Control disposed while updating
            }
        }

        private void HideSearchDropdown()
        {
            if (_isDisposed || this.IsDisposed) return;

            try
            {
                pnlSearchResults.Visible = false;
            }
            catch (ObjectDisposedException)
            {
                // Control disposed, ignore
            }
        }

        private void LstSearchResults_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= lstSearchResults.Items.Count)
                return;

            e.DrawBackground();

            if (lstSearchResults.Items[e.Index] is SANPHAM product)
            {
                // Highlight selected item
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(230, 230, 250)), e.Bounds);
                }

                // Image area
                var imgRect = new Rectangle(e.Bounds.X + 8, e.Bounds.Y + 8, 56, 56);
                var img = GetProductThumb(product.HINH);
                if (img != null)
                {
                    e.Graphics.DrawImage(img, imgRect);
                }
                else
                {
                    // Placeholder
                    using var pen = new Pen(Color.Silver);
                    e.Graphics.DrawRectangle(pen, imgRect);
                }

                // Text area
                var textRect = new Rectangle(imgRect.Right + 10, e.Bounds.Y + 8, e.Bounds.Width - imgRect.Width - 24, e.Bounds.Height - 16);
                var name = product.TENSP ?? "";
                const int maxChars = 40;
                if (name.Length > maxChars)
                    name = name.Substring(0, maxChars - 3) + "...";

                using var nameFont = new Font("Segoe UI", 10F, FontStyle.Bold);
                using var infoFont = new Font("Segoe UI", 9F, FontStyle.Regular);

                var nameRect = new Rectangle(textRect.X, textRect.Y, textRect.Width, 24);
                e.Graphics.DrawString(name, nameFont, Brushes.Black, nameRect);

                string priceText = product.GIABAN.HasValue ? $"{product.GIABAN.Value:N0} ₫" : "";
                string dvt = !string.IsNullOrWhiteSpace(product.DVT) ? $" / {product.DVT}" : "";
                var infoText = $"{priceText}{dvt}";

                var infoRect = new Rectangle(textRect.X, textRect.Y + 28, textRect.Width, 20);
                e.Graphics.DrawString(infoText, infoFont, Brushes.DimGray, infoRect);
            }

            e.DrawFocusRectangle();
        }

        private void LstSearchResults_MouseClick(object sender, MouseEventArgs e)
        {
            if (lstSearchResults.SelectedItem is SANPHAM selectedProduct)
            {
                SelectProduct(selectedProduct);
            }
        }

        private void LstSearchResults_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lstSearchResults.SelectedItem is SANPHAM selectedProduct)
            {
                SelectProduct(selectedProduct);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideSearchDropdown();
                txtSearch.Focus();
                e.Handled = true;
            }
        }

        private void SelectProduct(SANPHAM product)
        {
            HideSearchDropdown();
            txtSearch.Text = product.TENSP ?? "";
            
            // Open product details
            using var frm = new ProductDetails(product.MASP);
            frm.ShowDialog();
        }

        private Image GetProductThumb(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;

            if (_searchImageCache.TryGetValue(fileName, out var cached))
                return cached;

            try
            {
                var path = Path.Combine(Application.StartupPath, "ImagesProduct", fileName);
                if (!File.Exists(path)) return null;

                // Load a small copy to avoid locking the file
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                var bmp = new Bitmap(fs);
                _searchImageCache[fileName] = bmp;
                return bmp;
            }
            catch
            {
                return null;
            }
        }
    }
}
