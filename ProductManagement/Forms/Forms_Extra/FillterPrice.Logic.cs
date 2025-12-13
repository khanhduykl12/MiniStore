using System;
using System.Linq;
using System.Windows.Forms;

namespace MiniShop.Forms.Forms_Extra
{
    public partial class FillterPrice
    {
        public decimal? MinPrice { get; private set; }
        public decimal? MaxPrice { get; private set; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            MinPrice = ParseDecimal(txtMin.Text);
            MaxPrice = ParseDecimal(txtMax.Text);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private static decimal? ParseDecimal(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (decimal.TryParse(s, out var d) && d >= 0) return d;
            return null;
        }
    }
}
