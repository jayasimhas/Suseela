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
            try
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

                    var user = SitecoreClient.GetFullNameAndEmail(updatedArticleDetail.ArticleDetails.WordDocLastUpdatedBy);

                    lblBy.Text = user[0];

                    DateTime dt;
                    if (updatedArticleDetail.ArticleDetails != null && DateTime.TryParse(updatedArticleDetail.ArticleDetails.WordDocLastUpdateDate, out dt))
                        lblUpdatedOn.Text = dt.ToLocalTime().ToString(CultureInfo.InvariantCulture);
                    else
                        lblUpdatedOn.Text = _parentForm.ArticleDetails.WordDocLastUpdateDate.ToString(CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.LogException("refreshState ex: " + ex.ToString());
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refreshState();
        }
    }
}
