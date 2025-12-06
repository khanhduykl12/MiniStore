using System;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace MiniShop.Forms_Extra
{
    public partial class FormLockAccount : Form
    {
        public int? LockDays { get; private set; }
        public bool IsPermanentLock { get; private set; }

        public FormLockAccount()
        {
            InitializeComponent();
        }

        private void FormLockAccount_Load(object sender, EventArgs e)
        {
            rdbTemporary.Checked = true;
            numDays.Value = 1;
            numDays.Enabled = true;
        }

        private void rdbTemporary_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbTemporary.Checked)
            {
                numDays.Enabled = true;
                IsPermanentLock = false;
            }
        }

        private void rdbPermanent_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbPermanent.Checked)
            {
                numDays.Enabled = false;
                IsPermanentLock = true;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (rdbTemporary.Checked)
            {
                LockDays = (int)numDays.Value;
                IsPermanentLock = false;
            }
            else
            {
                LockDays = null;
                IsPermanentLock = true;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

