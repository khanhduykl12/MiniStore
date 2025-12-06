using MiniShop.User_Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniStore.UC
{
    public partial class UC_Dashboard : UserControl
    {
        public string userRole { get; set; }
        public UC_Dashboard(string role)
        {

            InitializeComponent();
            btnSetting.Visible= false;
            userRole=role;
            if (role == "ADMIN")
            {
                btnSetting.Visible = true;
            }
            else
            {
                btnNotification.Location = btnSetting.Location;
            }

        }

       

        private void UC_Dashboard_Load(object sender, EventArgs e)
        {
            
        }
    }
}
