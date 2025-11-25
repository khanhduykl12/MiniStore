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
            LoadMaSP();
        }
        private void LoadMaSP()
        {
            cboMaSp.DataSource = null;
            if (cboLoai.SelectedValue == null) return;
            string maloai = cboLoai.SelectedValue.ToString();
            var sanphams = db.SANPHAMs.Where(sp => sp.MALOAI == maloai).AsNoTracking().Select(sp => new { sp.MASP, sp.TENSP, sp.MANCC }).ToList();
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
                       .Select(x => new { x.MANCC })
                       .FirstOrDefault();

            if (sp == null)
            {
                txtNcc.Text = "";
                return;
            }

            var ncc = db.NHACUNGCAPs
                        .Where(n => n.MANCC == sp.MANCC)
                        .Select(n => n.TENNCC)
                        .FirstOrDefault();

            txtNcc.Text = ncc ?? "";
        }
    }
}
