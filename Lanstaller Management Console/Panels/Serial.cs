using LanstallerShared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lanstaller_Management_Console.Panels
{
    public partial class Serial : UserControl
    {
        public Serial()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Serial_Load(object sender, EventArgs e)
        {
            lvUserSerials.Columns.Add("Serial", 200);
            lvUserSerials.Columns.Add("Used", 130);
        }

        private void txtUserSerial_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddUserSerial_Click(object sender, EventArgs e)
        {

        }

        private void cmbxSerials_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAddSerial_Click(object sender, EventArgs e)
        {

        }

        private void btnDelUserSerial_Click(object sender, EventArgs e)
        {

        }
    }
}
