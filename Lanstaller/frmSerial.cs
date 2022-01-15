using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace Lanstaller
{
    
    public partial class frmSerial : Form
    {

        public frmSerial()
        {
            InitializeComponent();
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            txtSerial.Text = GetClipboard();
        }

        private void txtSerial_TextChanged(object sender, EventArgs e)
        {
            txtSerial.Text = txtSerial.Text.Replace("-", "").Replace(" ","");

        }

        
        string GetClipboard()
        {
            if (Clipboard.ContainsText())
            {
                return Clipboard.GetText();
            }
            return "";
        }

        private void frmSerial_Load(object sender, EventArgs e)
        {

        }

     
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
