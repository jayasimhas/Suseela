using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
    public partial class ArticleLockConfirmation : Form
    {
        public Action YesAction { get; set; }
        public Action NoAction { get; set; }
        public ArticleLockConfirmation()
        {
            InitializeComponent();
        }

        private void ArticleLockConfirmation_Load(object sender, EventArgs e)
        {

        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            YesAction();
            Close();
        }

        private void BtnNo_Click(object sender, EventArgs e)
        {
            NoAction();
            Close();
        }
    }
}
