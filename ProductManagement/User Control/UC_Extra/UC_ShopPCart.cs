using ProductManagement.Class;
using ProductManagement.Forms.Forms_Extra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductManagement.User_Control.UC_Extra
{
    public partial class UC_ShopPCart : UserControl
    {
        private string _MaSP { get; set; }
        private decimal GiaSP { get; set; }
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
            numSoLuong.Value = item.SoLuong;
            GiaSP = item.Gia;
            var pathIMG = Path.Combine(Application.StartupPath, "ImagesProduct", item.Hinh ?? "");
            picProduct.Image = File.Exists(pathIMG) ? Image.FromFile(pathIMG) : Properties.Resources.no_image;
            UpdateGia();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            (FindForm() as ShoppingCart)?.RemoveItemAndRefresh(_MaSP);

        }
        private void UpdateGia()
        {   
            var amount = GiaSP * (int)numSoLuong.Value;
            lblGia.Text = $"{amount:N0} đ";
        }

        private void numSoLuong_ValueChanged(object sender, EventArgs e)
        {
            var found = CartService.Items.FirstOrDefault(x => x.MaSP == _MaSP);
            if(found != null)
            {
                found.SoLuong = (int)numSoLuong.Value;
            }
            UpdateGia();
            (FindForm() as ShoppingCart)?.UpdateSum();
        }
    }
}
