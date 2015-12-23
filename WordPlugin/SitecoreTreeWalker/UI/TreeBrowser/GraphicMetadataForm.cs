using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SitecoreTreeWalker.Sitecore;

namespace SitecoreTreeWalker.UI.TreeBrowser
{
    public partial class GraphicsMetadataForm : Form
    {
        public GraphicsMetadataForm()
        {
            InitializeComponent();
        }

        private void uxCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GraphicsMetadataForm_Load(object sender, EventArgs e)
        {

        }

        public GraphicsMetadataForm(string imagePath)
        {
            InitializeComponent();
            pictureBox1.Image = Properties.Resources.loading;
            if (imagePath == null)
            {
                pictureBox1.Visible = false;
                return;
            }

            pictureBox1.ImageLocation = imagePath;
        }

        private void header_TextChanged(object sender, EventArgs e)
        {
            uxHeaderLabel.Text = this.uxHeader.Text;
        }

        private void title_TextChanged(object sender, EventArgs e)
        {
            uxTitleLabel.Text = this.uxTitle.Text;
        }

        private void caption_TextChanged(object sender, EventArgs e)
        {
            uxCaptionLabel.Text = this.uxCaption.Text;
        }

        private void source_TextChanged(object sender, EventArgs e)
        {
            uxSourceLabel.Text = this.uxSource.Text;
        }
    }
}
