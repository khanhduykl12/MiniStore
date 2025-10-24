using FFImageLoading.Cache;
using Guna.UI2.WinForms;
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

namespace MiniStore.User_Control._UC
{
    public partial class UC_ProductCard : UserControl
    {
        public UC_ProductCard()
        {
            InitializeComponent();
        }
        [Category("Product"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true), DefaultValue("")]
        public string Title
        {
            get => lblTenSP.Text;
            set => lblTenSP.Text = value ?? "";
        }

        [Category("Product"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true), DefaultValue(typeof(decimal), "0")]
        public decimal Price
        {
            get => decimal.TryParse(lblGia.Tag?.ToString(), out var v) ? v : 0m;
            set { lblGia.Tag = value; lblGia.Text = $"{value:N0} ₫"; }
        }


        [Category("Product"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true), DefaultValue("")]
        public string ImageFile
        {
            get => null;
            set
            {
                if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

                var p = Path.Combine(Application.StartupPath, "ImagesProduct", value ?? "");
                picProduct.Image = File.Exists(p) ? Image.FromFile(p) : Properties.Resources.no_image;
            }
        }

        private void UC_ProductCard_Load(object sender, EventArgs e)
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }


}
