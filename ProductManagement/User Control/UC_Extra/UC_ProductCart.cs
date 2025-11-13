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
using MiniStore.Forms.Forms_Extra;

namespace MiniStore.User_Control._UC
{
    public partial class UC_ProductCart : UserControl
    {
        public event EventHandler ProductClicked;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string MaSP { get; set; }
        public UC_ProductCart()
        {
            InitializeComponent();
            WireClickRecursive(this);
            this.Cursor = Cursors.Hand;

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
        private void WireClickRecursive(Control parent)
        {
            parent.Click += AnyChild_Click;
            foreach (Control c in parent.Controls)
                WireClickRecursive(c);
        }

        private void AnyChild_Click(object sender, EventArgs e)
            => ProductClicked?.Invoke(this, EventArgs.Empty);

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            ProductClicked?.Invoke(this, EventArgs.Empty);
        }
        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void sANPHAMBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void sANPHAMBindingSource_CurrentChanged_1(object sender, EventArgs e)
        {

        }
        private string ShortenName(string name, int maxLength = 15)
        {
            if (string.IsNullOrEmpty(name)) return "";
            if (name.Length > maxLength)
                return name.Substring(0, maxLength - 3) + "...";
            return name;
        }

        private void lblTenSP_Click(object sender, EventArgs e)
        {

        }
    }


}
