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
using Microsoft.VisualBasic;

namespace Lanstaller_Management_Console
{

    public partial class frmSecurity : Form
    {
        List<Security.Token> TokenList;
        List<Security.Registration> RegList;

        public frmSecurity()
        {
            InitializeComponent();
        }

        private void lbxAuth_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lbxAuth.SelectedIndex == -1)
            {
                return;
            }

            int tokenid = TokenList[lbxAuth.SelectedIndex].id;
            ShowToken(tokenid);
        }

        void ShowToken(int id)
        {
            Security.Token Tkn = Security.GetToken(id);
            txtToken.Text = Tkn.token;
            txtName.Text = Tkn.Name;
        }

        private void frmSecurity_Load(object sender, EventArgs e)
        {
            LoadKeys();
        }

        private void LoadKeys()
        {
            //Get auth keys.
            TokenList = Security.GetTokens();
            lbxAuth.Items.Clear();
            foreach (Security.Token ST in TokenList)
            {
                lbxAuth.Items.Add(ST.Name);
            }

            //Reg reg keys.
            RegList = Security.GetRegTokens();
            lbxRegCodes.Items.Clear();
            foreach (Security.Registration RT in RegList)
            {
                lbxRegCodes.Items.Add(RT.name);
            }

        }

       

        private void btnAuthDelete_Click(object sender, EventArgs e)
        {
            if (lbxAuth.SelectedIndex == -1)
            {
                return;
            }
            int tokenid = TokenList[lbxAuth.SelectedIndex].id;
            Security.DeleteToken(tokenid);

            LoadKeys();

        }

        private void btnRegAdd_Click(object sender, EventArgs e)
        {
            string name = Interaction.InputBox("Description / Name");
            string code = Interaction.InputBox("Registration Code");
            DateTime expiry = DateTime.Today.AddDays(365); //1 year expiry.

            if (name == "" || code == "")
            {
                return;
            }

            Security.NewRegistrationCode(name, code, expiry);

            //Refresh
            LoadKeys();
        }

        private void btnRegDel_Click(object sender, EventArgs e)
        {
            if (lbxRegCodes.SelectedIndex == -1)
            {
                return;
            }
            //delete reg key.
            Security.DeleteRegistrationCode(RegList[lbxRegCodes.SelectedIndex].id);

            //Refresh
            LoadKeys();
        }

        private void lbxRegCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxRegCodes.SelectedIndex == -1)
            {
                return;
            }
            txtRegName.Text = RegList[lbxRegCodes.SelectedIndex].name;
            txtRegCode.Text = RegList[lbxRegCodes.SelectedIndex].regcode;
            if (RegList[lbxRegCodes.SelectedIndex].expiry.ToString("yyyy-MM-dd") == "0001-01-01")
            {
                txtExpiry.Text = "Never";
            }
            else
            {
                txtExpiry.Text = RegList[lbxRegCodes.SelectedIndex].expiry.ToString("yyyy-MM-dd");
            }
        }
    }
}
