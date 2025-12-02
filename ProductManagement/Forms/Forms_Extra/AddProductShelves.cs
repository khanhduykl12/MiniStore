using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

namespace MiniStore.Forms.Forms_Extra
{
    public partial class AddProductShelves : Form
    {
        private readonly MiniStoreContext db = new MiniStoreContext();
        public AddProductShelves()
        {
            InitializeComponent();
        }

        private void AddProductShelves_Load(object sender, EventArgs e)
        {
            LoadLoai();
        }
        private void LoadLoai()
        {
            var loais = db.LOAISANPHAMs.AsNoTracking().ToList();
            lOAISANPHAMBindingSource.DataSource = loais;
            cboLoai.DataSource = loais;
            cboLoai.DisplayMember = "TENLOAI";
            cboLoai.ValueMember = "MALOAI";
            cboLoai.SelectedValue = -1;
            LoadMaSP();
        }
        private void LoadMaSP()
        {
            cboMaSp.DataSource = null;
            if (cboLoai.SelectedValue == null) return;
            string maloai = cboLoai.SelectedValue.ToString();
            var sanphams = db.SANPHAMs.Where(sp => sp.MALOAI == maloai).AsNoTracking().Select(sp => new { sp.MASP, sp.TENSP, sp.MANCC, sp.GIABAN }).ToList();
            cboMaSp.DataSource = sanphams;
            cboMaSp.DisplayMember = "MaSP";
            cboMaSp.ValueMember = "MaSP";
            cboTenSP.DataSource = sanphams;
            cboTenSP.DisplayMember = "TenSP";
            cboTenSP.ValueMember = "MaSP";


        }
        private void LoaiTen()
        {

        }

        private void cboLoai_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMaSP();

        }

        private void cboMaSp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaSp.SelectedValue == null)
                return;

            string masp = cboMaSp.SelectedValue.ToString();
            var sp = db.SANPHAMs
                       .Where(x => x.MASP == masp)
                       .Select(x => new { x.MANCC, x.GIABAN, x.HINH })
                       .FirstOrDefault();

            if (sp == null)
            {
                txtNcc.Text = "";
                txtGia.Text = "";
                picSanPham.Image = null;
                return;
            }
            txtGia.Text = $"{sp.GIABAN:N0} đ";

            if (!string.IsNullOrEmpty(sp.HINH))
            {
                string imagePath = Path.Combine(Application.StartupPath, "ImagesProduct", sp.HINH);
                picSanPham.Image = Image.FromFile(imagePath);
            }
            else
            {
                picSanPham.Image = Properties.Resources.no_image;
            }

            var ncc = db.NHACUNGCAPs
                        .Where(n => n.MANCC == sp.MANCC)
                        .Select(n => n.TENNCC)
                        .FirstOrDefault();

            txtNcc.Text = ncc ?? "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cboMaSp.SelectedIndex == null)
            {
                MessageBox.Show("Bạn chưa chọn sản phẩm nào", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string masp = cboMaSp.SelectedValue.ToString();
            int soluongThem = (int)numSoLuong.Value;
            if (soluongThem < 0)
            {
                MessageBox.Show("Số lượng không được nhỏ hơn 0", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (db)
            {
                var sp = db.SANPHAMs.SingleOrDefault(x => x.MASP == masp);
                if (masp == null)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm trong kho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (sp.SOLUONG < soluongThem)
                {
                    MessageBox.Show("Số lượng trong kho không đủ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var hangtrenke = db.HANGTRUNGBAYs.SingleOrDefault(x => x.MASP == masp);
                if (hangtrenke == null)
                {
                    hangtrenke = new HANGTRUNGBAY
                    {
                        MASP = masp,
                        SOLUONG_TRENKE = soluongThem
                    };
                    db.HANGTRUNGBAYs.Add(hangtrenke);
                }
                else
                {
                    hangtrenke.SOLUONG_TRENKE += soluongThem;
                }
                sp.SOLUONG -= soluongThem;
                db.SaveChanges();
            }
            MessageBox.Show("Đã thêm sản phẩm lên kệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //goi grid sp cb them len ke
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
