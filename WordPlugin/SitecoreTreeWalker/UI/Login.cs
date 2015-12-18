using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SitecoreTreeWalker.User;

namespace SitecoreTreeWalker.UI
{
    public partial class Login : Form
    {
        private SitecoreUser _user = SitecoreUser.GetUser();
        private Form _form = null;
        public Login()
        {
            InitializeComponent();
        }

        public Login(Form form)
        {
            InitializeComponent();
            _form = form;
        }

        private void uxLoginButton_Click(object sender, EventArgs e)
        {
            TryToLogin();
        }

        private void uxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                TryToLogin();
            }
        }

        private bool TryToLogin()
        {
            bool success = false;
            if (success = _user.Authenticate(uxUsername.Text, uxPassword.Text))
            {
                if (_form != null)
                {
                    _form.ShowDialog();
                }

                this.Close();
            }
            return success;
        }
    }
}
