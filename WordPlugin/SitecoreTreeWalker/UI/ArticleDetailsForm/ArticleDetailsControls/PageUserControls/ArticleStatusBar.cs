using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Shapes;
using SitecoreTreeWalker.document;
using SitecoreTreeWalker.User;
using SitecoreTreeWalker.WebserviceHelper;

namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
    public partial class ArticleStatusBar : UserControl
    {
        private ArticleDetail _parent;
        private ArticleInformationControl _parentArticleInformation;
        private string _articleNumber;
        public ArticleStatusBar()
        {
            InitializeComponent();
        }

        private void uxLinkUnlinkButton_Click(object sender, EventArgs e)
        {
            var articleLinkUnlinkInfo = new ArticleLinkUnlinkInfo();
            articleLinkUnlinkInfo.StartPosition = FormStartPosition.CenterParent;
            articleLinkUnlinkInfo.SetArticleNumber(_articleNumber);
            articleLinkUnlinkInfo.LinkToParent(_parent);
            articleLinkUnlinkInfo.LinkToStatusBar(this);
            articleLinkUnlinkInfo.ShowDialog();
        }

        private void uxRefreshButton_Click(object sender, EventArgs e)
        {
            //TODO: refresh button yet to be done
            var updatedArticleDetail = new ArticleDetail();
            _parent = updatedArticleDetail;
            _parent.Repaint();

        }

        private void uxLockStateButton_Click(object sender, EventArgs e)
        {
            var articleLockInfo = new ArticleLockInfo { StartPosition = FormStartPosition.CenterParent };
            articleLockInfo.LinkToParent(_parent);
            articleLockInfo.SetArticleNumber(_articleNumber);
            articleLockInfo.LinkToStatusBar(this);
            articleLockInfo.ShowDialog();
        }

        private void uxVersionStateButton_Click(object sender, EventArgs e)
        {
            var articleVersionStateInfo = new ArticleVersionStateInfo(_parent) { StartPosition = FormStartPosition.CenterParent };
            articleVersionStateInfo.ShowDialog();
        }

        private void uxWorkflowButton_Click(object sender, EventArgs e)
        {
            var articleWorkFlowInfo = new ArticleWorkflowInfo(_parent) { StartPosition = FormStartPosition.CenterParent };
            articleWorkFlowInfo.ShowDialog();
        }

        public void LinkToParent(ArticleDetail parent)
        {
            _parent = parent;
        }

        public void LinkToParentArticleInformation(ArticleInformationControl parent)
        {
            _parentArticleInformation = parent;
        }

        public void SetArticleNumber(string articleNumber)
        {
            if (!string.IsNullOrEmpty(articleNumber))
            {
                uxArticleNumber.Text = articleNumber;
                _articleNumber = articleNumber;
            }
        }

        public void DisplayStatusBar(bool displayStatus, string articleNumber)
        {
            uxLockStateButton.Visible = displayStatus;            
            uxVersionStateButton.Visible = displayStatus;
            uxWorkflowButton.Visible = displayStatus;
            if (!string.IsNullOrEmpty(articleNumber))
            {
                uxArticleNumber.Text = articleNumber;
                _articleNumber = articleNumber;
            }
            else
            {
                uxArticleNumber.Text = Constants.DOCUMENT_NOT_LINKED;
                _articleNumber = null;
            }
            if (displayStatus)
            {
                uxLinkUnlinkButton.Text = "UnLink";
                uxLinkUnlinkButton.Image = new Bitmap(SitecoreTreeWalker.Properties.Resources.broken_link);
            }
            else
            {
                uxLinkUnlinkButton.Text = "Link";
                uxLinkUnlinkButton.Image = new Bitmap(SitecoreTreeWalker.Properties.Resources.link_32);
            }
            var articleLockInfo = new ArticleLockInfo();
            articleLockInfo.SetCheckedOutStatus();
        }

        public void ChangeLockButtonStatus(bool displayStatus)
        {
            if (displayStatus)
            {
                uxLockStateButton.Text = "Unlock";
                uxLockStateButton.Image = new Bitmap(SitecoreTreeWalker.Properties.Resources._53_Lock_unlocked);
            }
            else
            {
                uxLockStateButton.Text = "Lock";
                uxLockStateButton.Image = new Bitmap(SitecoreTreeWalker.Properties.Resources._1445989402_lock_24);
            }
        }

    }
}
