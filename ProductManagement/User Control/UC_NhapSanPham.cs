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

namespace MiniStore.User_Control
{
    public partial class UC_NhapSanPham : UserControl
    {
        public UC_NhapSanPham()
        {
            InitializeComponent();
        }




        private void pbox_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn hình ảnh sản phẩm";
                ofd.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pbox.Image = Image.FromFile(ofd.FileName);
                    pbox.Tag = ofd.FileName;
                }
            }
        }


        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            ValidateNSX();
            if (pbox.Tag == null)
            {
                MessageBox.Show("Vui lòng chọn ảnh sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string sourcePath = pbox.Tag.ToString();
            string folderPath = Path.Combine(Application.StartupPath, "ImagesProduct");

            string fileName = Path.GetFileNameWithoutExtension(sourcePath)
                  + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss")
                  + Path.GetExtension(sourcePath);
            string destPath = Path.Combine(folderPath, fileName);
            File.Copy(sourcePath, destPath, true);

            using (var db = new MiniStoreContext())
            {
                var sp = new SANPHAM
                {
                    MALOAI = txtMaLoai.Text,
                    MASP = txtMaSP.Text,
                    TENSP = txtTenSanPham.Text,
                    GHICHU = txtGhiChu.Text,
                    DVT = txtDVT.Text,
                    MANCC = txtMaNhaCC.Text,
                    GIABAN = decimal.Parse(txtGiaBan.Text),
                    SOLUONG = int.Parse(txtSoluong.Text),
                    NSX = DateOnly.FromDateTime(dtpNSX.Value),
                    HINH = fileName,
                };

                db.SANPHAMs.Add(sp);
                db.SaveChanges();
                SanPhamDaThem?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.Dispose();
            }
        }
        bool isDateSelected = false;

        private void dtpNSX_ValueChanged_1(object sender, EventArgs e)
        {
            isDateSelected = true;
            ValidateNSX();
        }
        private void ValidateNSX()
        {
            if (!isDateSelected)
            {
                MessageBox.Show("Bạn quên chọn ngày sản xuất hả?", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public event EventHandler SanPhamDaThem;

    }
}

