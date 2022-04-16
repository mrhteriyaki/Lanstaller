using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace window_control_box
{
    public partial class FormControlBox : UserControl
    {
        [Category("Appearance")]
        [Description("Gets or sets maximize button visibility")]
        public bool Maximize
        {
            set
            {
                lblMaximize.Visible = value;   
            }
            get
            {
                return lblMaximize.Visible; 
            }
        }
        [Category("Appearance")]
        [Description("Gets or sets minimize button visibility")]
        public bool Minimize
        {
            set
            {
                lblMinimize.Visible = value;
            }
            get
            {
                return lblMinimize.Visible;
            }
        }

        [Category("Appearance")]
        [Description("Gets or sets close button visibility")]
        public bool Close
        {
            set
            {
                lblClose.Visible = value;
            }
            get
            {
                return lblClose.Visible;
            }
        }

        public FormControlBox()
        {
            InitializeComponent();
        }

        private void lblClose_MouseMove(object sender, MouseEventArgs e)
        {
            lblClose.Image = global::window_control_box.Properties.Resources.close_sele;     
        }

        private void lblClose_MouseLeave(object sender, EventArgs e)
        {
            lblClose.Image = global::window_control_box.Properties.Resources.close;      
        }

        private void lblMaximize_MouseLeave(object sender, EventArgs e)
        {
            lblMaximize.Image = global::window_control_box.Properties.Resources.maximize;     
        }

        private void lblMaximize_MouseMove(object sender, MouseEventArgs e)
        {
            lblMaximize.Image = global::window_control_box.Properties.Resources.maximize_sele;     
        }

        private void lblMinimize_MouseMove(object sender, MouseEventArgs e)
        {
            lblMinimize.Image = global::window_control_box.Properties.Resources.minimize_sele;     
        }

        private void lblMinimize_MouseLeave(object sender, EventArgs e)
        {
            lblMinimize.Image = global::window_control_box.Properties.Resources.minimize;     
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.ParentForm.Close();   
        }

        private void lblMaximize_Click(object sender, EventArgs e)
        {
            if (this.ParentForm.WindowState == FormWindowState.Maximized)
            {
                this.ParentForm.WindowState = FormWindowState.Normal;
            }
            else if(this.ParentForm.WindowState == FormWindowState.Normal)
            {
                this.ParentForm.WindowState = FormWindowState.Maximized;
            }
            this.ParentForm.Show();   
        }

        private void lblMinimize_Click(object sender, EventArgs e)
        {
            this.ParentForm.WindowState = FormWindowState.Minimized;
            this.ParentForm.Show(); 
        }

        private void FormControlBox_Load(object sender, EventArgs e)
        { 
        }
    }
}