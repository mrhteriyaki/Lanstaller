using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lanstaller
{
    public partial class frmComplete : Form
    {
        public frmComplete()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void frmComplete_Load(object sender, EventArgs e)
        {

        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
            
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

    }
}
