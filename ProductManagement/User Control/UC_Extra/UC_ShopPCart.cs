using Microsoft.EntityFrameworkCore;
using MiniShop.Forms.Forms_Extra;
using MiniStore.Class;
using MiniStore.Forms.Forms_Extra;
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

namespace MiniStore.User_Control.UC_Extra
{
    public partial class UC_ShopPCart : UserControl
    {
        private string _MaSP { get; set; }
        private decimal GiaSP { get; set; }
        private int soLuongTruocDo;
        private bool giaTriThayDoi;

        public UC_ShopPCart()
        {
            InitializeComponent();
            UpdateGia();
        }
        
        public void BlindDuLieu(CartItem item)
        {
            _MaSP = item.MaSP;
            lblTen.Text = item.TenSP;
            lblDVT.Text = item.DVT;
            soLuongTruocDo = item.SoLuong;

            giaTriThayDoi = true;
            numSoLuong.Value = item.SoLuong;
            giaTriThayDoi = false;

            GiaSP = item.Gia;
            var pathIMG = Path.Combine(Application.StartupPath, "ImagesProduct", item.Hinh ?? "");
            picProduct.Image = File.Exists(pathIMG) ? Image.FromFile(pathIMG) : null;
            UpdateGia();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            (FindForm() as ShoppingCart)?.RemoveItemAndRefresh(_MaSP);
            (FindForm() as ShoppingCartStaff)?.RemoveItemAndRefresh(_MaSP);
        }

        private void UpdateGia()
        {   
            var amount = GiaSP * (int)numSoLuong.Value;
            lblGia.Text = $"{amount:N0} đ";
        }

        private int GetAvailableOnShelf()
        {
            try
            {
                using var db = new MiniStoreContext();
                var shelf = db.HANGTRUNGBAYs.AsNoTracking().FirstOrDefault(h => h.MASP == _MaSP);
                if (shelf != null) return shelf.SOLUONG_TRENKE;
                var sp = db.SANPHAMs.AsNoTracking().FirstOrDefault(s => s.MASP == _MaSP);
                return sp?.SOLUONG ?? 0;
            }
            catch
            {
                return int.MaxValue;
            }
        }

        private void numSoLuong_ValueChanged(object sender, EventArgs e)
        {
            if (giaTriThayDoi) return;

            int newQty = (int)numSoLuong.Value;
            int available = GetAvailableOnShelf();

            if (newQty > available)
            {
                MessageBox.Show($"Không đủ hàng trên kệ. Số lượng tối đa hiện có: {available}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                giaTriThayDoi = true;
                int revertTo = Math.Min(soLuongTruocDo, Math.Max(0, available));
                if (revertTo < numSoLuong.Minimum) revertTo = (int)numSoLuong.Minimum;
                if (revertTo > numSoLuong.Maximum) revertTo = (int)numSoLuong.Maximum;
                numSoLuong.Value = revertTo;
                giaTriThayDoi = false;
                return;
            }

            var found = CartService.Items.FirstOrDefault(x => x.MaSP == _MaSP);
            if(found != null)
            {
                found.SoLuong = newQty;
            }
            soLuongTruocDo = newQty;

            UpdateGia();
            (FindForm() as ShoppingCart)?.UpdateSum();
            (FindForm() as ShoppingCartStaff)?.UpdateSum();
        }
    }
}
