using MiniShop.Forms.Forms_Extra;
using MiniStore.Class;
using MiniStore.Forms.Forms_Extra;
using MiniStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniStore.User_Control.UC_Extra
{
    public partial class UC_ShippingCOD : UserControl
    {
        private readonly Dictionary<string, string[]> _wards = new()
        {
            ["Quận 1"] = new[] { "Bến Nghé", "Bến Thành", "Cô Giang", "Cầu Kho" },
            ["Quận 3"] = new[] { "Phường 1", "Phường 2", "Phường 3", "Phường 4" },
            ["Quận 4"] = new[] { "Phường 1", "Phường 2", "Phường 3", "Phường 4" },
            ["Quận 5"] = new[] { "Phường 1", "Phường 2", "Phường 3", "Phường 4" },
            ["Quận 6"] = new[] { "Phường 1", "Phường 2", "Phường 3", "Phường 4" },
            ["Quận 7"] = new[] { "Tân Phong", "Tân Phú", "Bình Thuận", "Tân Hưng" },
            ["Quận 8"] = new[] { "Phường 1", "Phường 2", "Phường 3", "Phường 4" },
            ["Quận 10"] = new[] { "Phường 1", "Phường 2", "Phường 3", "Phường 4" },
            ["Quận 11"] = new[] { "Phường 1", "Phường 2", "Phường 3", "Phường 4" },
            ["Quận 12"] = new[] { "Tân Chánh Hiệp", "Tân Hưng Thuận", "Đông Hưng Thuận", "Tân Thới Hiệp" },
            ["Quận Phú Nhuận"] = new[] { "Phường 1", "Phường 2", "Phường 3", "Phường 4" },
            ["Quận Bình Thạnh"] = new[] { "Phường 1", "Phường 2", "Phường 3", "Phường 5" },
            ["Quận Gò Vấp"] = new[] { "Phường 1", "Phường 3", "Phường 4", "Phường 5" },
            ["Quận Tân Bình"] = new[] { "Phường 1", "Phường 2", "Phường 4", "Phường 6" },
            ["Quận Bình Tân"] = new[] { "An Lạc", "An Lạc A", "Bình Trị Đông", "Bình Trị Đông A" },
            ["Quận Tân Phú"] = new[] { "Tây Thạnh", "Sơn Kỳ", "Tân Quý", "Tân Thới Hòa" },
        };
        public UC_ShippingCOD()
        {
            InitializeComponent();
            cboQuan.SelectedIndexChanged += (_, __) => AddPhuong();
            cboQuan.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPhuong.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            UC_ShopPCart uc = new UC_ShopPCart();
            uc.Show();
        }


        private void AddPhuong()
        {

            cboPhuong.BeginUpdate();
            cboPhuong.Items.Clear();
            string quan = cboQuan.Text.Trim();

            if (_wards.TryGetValue(quan, out var wards))
            {
                cboPhuong.Items.AddRange(wards);
                if (cboPhuong.Items.Count > 0)
                    cboPhuong.SelectedIndex = 0;
            }
            else
            {
                cboPhuong.Items.Add("— Chưa có dữ liệu phường —");
                cboPhuong.SelectedIndex = 0;
            }

            cboPhuong.EndUpdate();
        }

        private void cboQuan_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddPhuong();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {

        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đơn hàng của bạn sẽ được giao trong vài giờ tới,vui lòng chú ý điện thoại!!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            using (var db = new MiniStoreContext())
            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in CartService.Items)
                    {
                        // item.MaSP, item.SoLuong lấy từ CartItem
                        var hangTrenKe = db.HANGTRUNGBAYs
                                           .SingleOrDefault(x => x.MASP == item.MaSP);

                        if (hangTrenKe == null)
                        {
                            MessageBox.Show(
                                $"Sản phẩm {item.TenSP} chưa được đưa lên kệ.",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            tran.Rollback();
                            return;
                        }

                        if (hangTrenKe.SOLUONG_TRENKE < item.SoLuong)
                        {
                            MessageBox.Show(
                                $"Số lượng trên kệ của {item.TenSP} không đủ.\n" +
                                $"Còn: {hangTrenKe.SOLUONG_TRENKE}, khách mua: {item.SoLuong}.",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            tran.Rollback();
                            return;
                        }

                        // Trừ số lượng trên kệ
                        hangTrenKe.SOLUONG_TRENKE -= item.SoLuong;
                    }

                    db.SaveChanges();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("Có lỗi khi cập nhật số lượng: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            CartService.Clear();
            var parentForm = this.FindForm();
            if (parentForm is ShoppingCartStaff staffForm)
            {
                
                staffForm.Close();
            }
            else if (parentForm is ShoppingCart cartForm)
            {
               
                cartForm.Close();
            }
        }
    }
}
