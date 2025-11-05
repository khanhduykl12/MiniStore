using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductManagement
{
    public partial class FormLuongChucVu : Form
    {
        public decimal Luong_formLuongChucVu { get; private set; }
        public string ChucVu_formLuongChucVu { get; private set; }
        public FormLuongChucVu()
        {
            InitializeComponent();
        }

        private void FormLuongChucVu_Load(object sender, EventArgs e)
        {
            cboChucVu.Items.Add("Quản trị viên");
            cboChucVu.Items.Add("Nhân viên");
            lblErrorChucVu.Text = string.Empty;
            txtLuong.Enabled = false;

        }
        private void btnRegisterForm_Click(object sender, EventArgs e)
        {
            if (cboChucVu.SelectedIndex == -1)
            {
                lblErrorChucVu.Text = "Bạn chọn chức vụ";
                lblErrorChucVu.ForeColor = Color.Red;
                return; 
            }
            else
            {
                lblErrorChucVu.Text = string.Empty;
            }
            
      
            Luong_formLuongChucVu = decimal.Parse(txtLuong.Text.Replace(".", "").Replace(",", ""));
            ChucVu_formLuongChucVu = cboChucVu.SelectedItem.ToString();
            this.Close();
        }
        private void cboChucVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboChucVu.SelectedIndex != -1)
            {
                if (cboChucVu.SelectedItem.ToString() == "Quản trị viên")
                {
                    txtLuong.Text = "20.000.000";
                }
                else if (cboChucVu.SelectedItem.ToString() == "Nhân viên")
                {
                    txtLuong.Text = "12.000.000";
                }
                
                lblErrorChucVu.Text = string.Empty;
            }
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
