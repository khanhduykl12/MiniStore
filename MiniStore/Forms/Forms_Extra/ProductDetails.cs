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
    public partial class ProductDetails : Form
    {
        private string _MaSP;
        public ProductDetails(string MaSP)
        {
            InitializeComponent();
            _MaSP = MaSP;
        }
    }
}
