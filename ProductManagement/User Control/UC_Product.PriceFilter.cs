using MiniShop.Forms.Forms_Extra;
using MiniStore.Models;
using MiniStore.User_Control._UC;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MiniStore.User_Control
{
    public partial class UC_Product
    {
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (btnFillPrice != null)
            {
                btnFillPrice.Click -= BtnFillPrice_OpenDialog;
                btnFillPrice.Click += BtnFillPrice_OpenDialog;
            }
        }

        private void BtnFillPrice_OpenDialog(object sender, System.EventArgs e)
        {
            ShowPriceFilter();
        }

        private void ApplyPriceFilterAndRender(decimal? min, decimal? max)
        {
            var list = _filtered.AsEnumerable();
            if (min.HasValue)
                list = list.Where(sp => (decimal)(sp.GIABAN ?? 0) >= min.Value);
            if (max.HasValue)
                list = list.Where(sp => (decimal)(sp.GIABAN ?? 0) <= max.Value);

            var final = list.OrderBy(x => x.TENSP).ToList();

            flpProduct.SuspendLayout();
            flpProduct.Controls.Clear();
            foreach (var sp in final)
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

        public void ShowPriceFilter()
        {
            using var dlg = new FillterPrice();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                ApplyPriceFilterAndRender(dlg.MinPrice, dlg.MaxPrice);
            }
        }
    }
}
