using InformaSitecoreWord.document;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
    public partial class ArticleVersionStatePopup : Form
    {
        ArticleDetail _parentForm;
        public ArticleVersionStatePopup(ArticleDetail parent)
        {
            InitializeComponent();
            _parentForm = parent;
            //d9ead3 green
            //F4CCCC
        }

        private void ArticleVersionStatePopup_Load(object sender, EventArgs e)
        {
            refreshState();
        }

        private void refreshState()
        {
            var documentCustomProperties = new DocumentCustomProperties(SitecoreAddin.ActiveDocument);
            using (ArticleDetailFieldsUpdateDisabler disabler = new ArticleDetailFieldsUpdateDisabler())
            {
                var updatedArticleDetail = new ArticleDetail();

                var localDocVersion = documentCustomProperties.WordSitecoreVersionNumber;
                var sitecoreDocVersion = SitecoreClient.GetWordVersionNumber(_parentForm.ArticleDetails.ArticleGuid);

                if (sitecoreDocVersion > localDocVersion)
                {
                    Color color = (Color)new ColorConverter().ConvertFromString("#F4CCCC");
                    this.BackColor = color;
                    lblTitle.Text = "This document is Outdated";
                }
                else
                {
                    Color color = (Color)new ColorConverter().ConvertFromString("#d9ead3");
                    this.BackColor = color;
                    lblTitle.Text = "This document is Up to Date";
                }

                lblBy.Text = updatedArticleDetail.ArticleDetails.WordDocLastUpdatedBy;
                lblUpdatedOn.Text = SitecoreWordUtil.FormatUserName(Convert.ToDateTime(updatedArticleDetail.ArticleDetails.WordDocLastUpdateDate).ToLocalTime().ToString(CultureInfo.InvariantCulture));
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refreshState();
        }
    }
}
