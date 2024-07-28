using Lanstaller_Shared;
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
    public partial class frmRunSelection : Form
    {
        public frmRunSelection()
        {
            InitializeComponent();
        }

        private void frmRunSelection_Load(object sender, EventArgs e)
        {
            this.Deactivate += new System.EventHandler(this.frmRunSelection_Deactivate);
        }


        public void SetOptions(List<ShortcutOperation> LaunchOptions)
        {
            int locationX = 10;
            int locationY = 20;
            foreach(ShortcutOperation op in LaunchOptions)
            {
                Button newButton = new Button();
                newButton.ForeColor = Color.White;
                newButton.FlatStyle = FlatStyle.Flat;
                newButton.Tag = op;
                newButton.Text = op.name;
                newButton.Font = new Font(Font.FontFamily, 15);
                newButton.Click += new System.EventHandler(this.newButton_Click);
                newButton.Location = new Point(locationX, locationY);
                newButton.Width = 480;
                newButton.Height = 50;
                gbxOptions.Controls.Add(newButton);
                
                locationY += 60;
            }

            this.Height = locationY + 10;
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            Button sending_button = (Button)sender;
            ShortcutOperation shortcutOperation = (ShortcutOperation)sending_button.Tag;
            frmLanstaller.runShortcut(shortcutOperation);
            this.Close();
        }

        private void frmRunSelection_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }
        

    }
}
