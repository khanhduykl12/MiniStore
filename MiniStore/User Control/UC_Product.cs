using Microsoft.EntityFrameworkCore;
using MiniStore.Data;
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
    public partial class UC_Product : UserControl
    {
        private readonly MiniStoreContext db = new MiniStoreContext();
        public UC_Product()
        {
            InitializeComponent();
        }

        private void UC_Product_Load(object sender, EventArgs e)
        {
            var list = db.LOAISANPHAMs.AsNoTracking().ToList();
            list.Insert(0, new LOAISANPHAM
            {
                MALOAI = "ALL",
                TENLOAI = "Tất Cả Loại Hàng"
            });
            lOAISANPHAMBindingSource.DataSource = list;
            cboAllCate.SelectedIndex = 0;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
