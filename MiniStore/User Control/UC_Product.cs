using Microsoft.EntityFrameworkCore;
using MiniStore.Models;
using MiniStore.User_Control._UC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniStore.User_Control
{
    public partial class UC_Product : UserControl
    {
        private readonly MiniStoreContext db = new MiniStoreContext();

        // Paging
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
            // Lọc theo combobox
            cboAllCate.SelectedIndexChanged += CboAllCate_SelectedIndexChanged;
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
            if (Session.Marole == "KH")
            {
                btnKho.Visible = false;
            }
            else
            {
                btnKho.Visible = true;
            }
            //load sp cho cbobox
            var loais = await db.LOAISANPHAMs.AsNoTracking().ToListAsync();
            loais.Insert(0, new LOAISANPHAM { MALOAI = "ALL", TENLOAI = "Tất Cả Loại Hàng" });
            lOAISANPHAMBindingSource.DataSource = loais;
            if (cboAllCate.Items.Count > 0) cboAllCate.SelectedIndex = 0;

            await ApplyFilterAndResetAsync();
        }

        // loc combobox
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
        // load tung trang
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
                    var card = new UC_ProductCard
                    {

                        Title = sp.TENSP,
                        Price = (decimal)(sp.GIABAN ?? 0),
                        ImageFile = sp.HINH

                    };
                    flpProduct.Controls.Add(card);
                }
                flpProduct.ResumeLayout();
            }
            finally
            {
                _isLoading = false;
            }
        }

        // den cuoi them 1 trang
        private async void FlpProduct_Scroll(object sender, ScrollEventArgs e)
        {
            // con lai pixel
            int remaining = flpProduct.DisplayRectangle.Height
                            - (-flpProduct.AutoScrollPosition.Y + flpProduct.ClientSize.Height);

            if (remaining < 400)
                await LoadNextPageAsync();
        }

        // reload all
        public async Task ReloadAllAsync()
        {
            await ApplyFilterAndResetAsync();
        }

        private void guna2CustomGradientPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnOption_Click(object sender, EventArgs e)
        {
            var pt = new Point(0, btnOption.Height);
            menuOption.Show(btnOption, pt);
        }

        private void menuOption_Opening(object sender, CancelEventArgs e)
        {

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
