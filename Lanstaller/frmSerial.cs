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
using System.Threading;

namespace Lanstaller
{
    
    public partial class frmSerial : Form
    {

        public frmSerial()
        {
            InitializeComponent();
        }

       
        private void txtSerial_TextChanged(object sender, EventArgs e)
        {
            txtSerial.Text = txtSerial.Text.Replace("-", "").Replace(" ","");

        }

        
       

        

     
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            txtSerial.Text = GetClipboard();
        }


        private string GetClipboard()
        {
            try
            {
                string clipboardData = null;
                Exception threadEx = null;
                Thread staThread = new Thread(
                    delegate ()
                    {
                        try
                        {
                            clipboardData = Clipboard.GetText(TextDataFormat.Text);
                        }

                        catch (Exception ex)
                        {
                            threadEx = ex;
                        }
                    });
                staThread.SetApartmentState(ApartmentState.STA);
                staThread.Start();
                staThread.Join();
                return clipboardData;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return string.Empty;
            }
        }

        
    }
}
