﻿using System;
using System.Windows.Forms;
using SitecoreTreeWalker.document;
using SitecoreTreeWalker.SitecoreServer;
using SitecoreTreeWalker.User;
using SitecoreTreeWalker.Util;

namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
    public partial class ArticleVersionStateInfo : Form
    {
        private ArticleDetail _parent;
        public ArticleVersionStateInfo()
        {            
            InitializeComponent();
        }

        public ArticleVersionStateInfo(ArticleDetail parent)
        {            
            InitializeComponent();
            _parent = parent;
            uxVersionUpdated.Text = parent.ArticleDetails.WordDocLastUpdatedBy;
            uxVersionUpdateDate.Text = SitecoreWordUtil.FormatUserName(parent.ArticleDetails.WordDocLastUpdateDate);
        }

        private void uxRefresh_Click(object sender, EventArgs e)
        {
            if (_parent == null || _parent.ArticleDetails == null)
            {
                return;
            }
            var updatedArticleDetail = new ArticleDetail();
            if (Convert.ToDateTime(_parent.ArticleDetails.WordDocLastUpdateDate) >=
                Convert.ToDateTime(updatedArticleDetail.ArticleDetails.WordDocLastUpdateDate)) return;
            uxVersionUpdated.Text = updatedArticleDetail.ArticleDetails.WordDocLastUpdatedBy;
            uxVersionUpdateDate.Text = SitecoreWordUtil.FormatUserName(updatedArticleDetail.ArticleDetails.WordDocLastUpdateDate);
        }

    }
}
