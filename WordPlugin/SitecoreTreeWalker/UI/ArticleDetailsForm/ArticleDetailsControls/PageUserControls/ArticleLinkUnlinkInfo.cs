using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using InformaSitecoreWord.document;
using InformaSitecoreWord.Util.Document;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	public partial class ArticleLinkUnlinkInfo : Form
	{
        protected DocumentCustomProperties _documentCustomProperties;
        private bool _isArtcleLinked=false;
        private ArticleDetail _parent;
        private ArticleInformationControl _articleInformationControl;
        private ArticleStatusBar _statusBar;

		public ArticleLinkUnlinkInfo()
		{
			InitializeComponent();
            uxLinkedTo.Text = Constants.ARTICLE_NUMBER;
		}
        public void SetArticleNumber(string articleNumber)
        {
            if (articleNumber!= Constants.DOCUMENT_NOT_LINKED && !string.IsNullOrEmpty(articleNumber))
            {
                uxLinkedTo.Text = articleNumber;
                _isArtcleLinked = true;
            }
            else
            {
                _isArtcleLinked = false;
            }

        }

        public void LinkToParent(ArticleDetail parent)
        {
            _parent = parent;
            _articleInformationControl = _parent.articleDetailsPageSelector.pageArticleInformationControl;

        }
        public void LinkToStatusBar(ArticleStatusBar parent)
        {
            _statusBar = parent;
        }
        private void uxUnlinkButton_Click(object sender, EventArgs e)
        {
            if (_articleInformationControl._isCheckedOutByMe)
            {
                _articleInformationControl.CheckIn(false);
            }
            uxLinkedTo.Text = Constants.DOCUMENT_NOT_LINKED;
            _isArtcleLinked = false;
            DocumentPropertyEditor.Clear(SitecoreAddin.ActiveDocument);
            _parent.PreLinkEnable();
            _parent.SetArticleNumber(null);
            _parent.UnlinkWordFileFromSitecoreItem();
            _parent.ResetFields();
            _parent.ResetChangedStatus(true);
			_parent.articleDetailsPageSelector.pageArticleInformationControl.IsPublished = false;
			 this.Close();
        }

        private void uxLinkDocument_Click(object sender, EventArgs e)
        {
            if (_articleInformationControl.CheckOut(uxArticleNumberToLink.Text, true))
            {
                _parent.ResetChangedStatus();
                _statusBar.DisplayStatusBar(true, uxArticleNumberToLink.Text);
            }
            else
            {
                _statusBar.DisplayStatusBar(false, null);
            }
            this.Close();
        }

        private void ArticleLinkUnlinkInfo_Load(object sender, EventArgs e)
        {
            if(_isArtcleLinked)
            {
                uxPanelUnlink.Visible = true;
                uxPanelLink.Visible = false;
            }
            else
            {
                uxPanelUnlink.Visible = false;
                uxPanelLink.Visible = true;
            }
        }

   
    }
}
