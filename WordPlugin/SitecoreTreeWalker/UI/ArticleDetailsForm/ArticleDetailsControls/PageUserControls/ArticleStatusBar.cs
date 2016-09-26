using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Shapes;
using InformaSitecoreWord.Properties;
using InformaSitecoreWord.document;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.User;
using InformaSitecoreWord.Util;
using InformaSitecoreWord.WebserviceHelper;
using PluginModels;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
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

        private void uxLockStateButton_Click(object sender, EventArgs e)
        {
            var articleLockInfo = new ArticleLockInfo { StartPosition = FormStartPosition.CenterParent };
            articleLockInfo.LinkToParent(_parent);
            articleLockInfo.SetArticleNumber(_articleNumber);
            articleLockInfo.LinkToStatusBar(this);
            articleLockInfo.ShowDialog();
        }

        public void UpdateFields()
        {
            if (_parent != null)
                UpdateFields(SitecoreClient.ForceReadArticleDetails(_parent.GetArticleNumber()));
        }

        public void UpdateFields(Guid articleGuid)
        {
            UpdateFields(SitecoreClient.ForceReadArticleDetails(articleGuid));
        }

        public void UpdateFields(ArticleStruct articleDetails)
        {
            _parent.ArticleDetails = articleDetails;
        }

        private void uxVersionStateButton_Click(object sender, EventArgs e)
        {
            //var articleVersionStateInfo = new ArticleVersionStateInfo(_parent) { StartPosition = FormStartPosition.CenterParent };
            var articleVersionState = new ArticleVersionStatePopup(_parent) { StartPosition = FormStartPosition.CenterParent };
            articleVersionState.ShowDialog();
            //articleVersionStateInfo.ShowDialog();
        }

        private void uxWorkflowButton_Click(object sender, EventArgs e)
        {
            var history = SitecoreClient.GetItemWorkflowHistory(_parent.ArticleDetails.ArticleGuid);
            if (_parent.ArticleDetails.IsPublished)
            {
                DateTime actualPublishDate = SitecoreClient.GetArticleActualPublishedDate(_parent.ArticleDetails.ArticleGuid);

                if (actualPublishDate != DateTime.MinValue)
                    MessageBox.Show(string.Format("Published on '{0}'", actualPublishDate.ToLocalTime().ToString("MM/dd/yyyy hh:mm:00 tt")));
            }
            else
            {
                MessageBox.Show(string.Format("Workflow state: '{0}' since {1}", _parent.ArticleDetails.ArticleWorkflowState?.DisplayName, history.LastOrDefault().Item1.ToLocalTime().ToString("MM/dd/yyyy hh:mm:00 tt")));
            }
            return;
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
            RefreshWorkflowDetails();
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
                uxLinkUnlinkButton.Image = new Bitmap(Resources.broken_link);
            }
            else
            {
                uxLinkUnlinkButton.Text = "Link";
                uxLinkUnlinkButton.Image = new Bitmap(Resources.link_32);
            }
            var articleLockInfo = new ArticleLockInfo();
			articleLockInfo.LinkToParent(_parent);
			articleLockInfo.SetCheckedOutStatus();
        }

        public void RefreshWorkflowDetails()
        {
            if (_parent.ArticleDetails.IsPublished)
            {
                uxWorkflowButton.Image = Properties.Resources.live;
                uxWorkflowButton.Text = "Published";
            }
            else
            {
                if (_parent.ArticleDetails.ArticleWorkflowState != null)
                    uxWorkflowButton.Text = string.Format("In '{0}'", _parent.ArticleDetails.ArticleWorkflowState.DisplayName);
            }
        }

        public void ChangeLockButtonStatus(LockStatus lockStatus)
        {
            switch (lockStatus)
            {
                case LockStatus.Locked:
                    {
                        uxLockStateButton.Text = "Locked";
                        uxLockStateButton.ToolTipText = "Currently locked, click for more information and to unlock";
                        uxLockStateButton.Image = new Bitmap(Resources.lockedIcon);
                    }
                    break;
                //case LockStatus.Unlocked:
                //    {
                //        uxLockStateButton.Text = "Unlocked";
                //        uxLockStateButton.ToolTipText = "";
                //        uxLockStateButton.Image = new Bitmap(Resources.unlockIcon);
                //    }
                //    break;
                default:
                    {
                        uxLockStateButton.Text = "Unlocked";
                        uxLockStateButton.ToolTipText = "Currently unlocked, click for more information and to Lock";
                        uxLockStateButton.Image = new Bitmap(Resources.unlockIcon);
                    }
                    break;
            }
        }

		private void ArticleStatusBar_Load(object sender, EventArgs e)
		{
			try {
				var articleLockInfo = new ArticleLockInfo();
				articleLockInfo.LinkToParent(_parent);
				articleLockInfo.SetArticleNumber(_articleNumber);
				articleLockInfo.LinkToStatusBar(this);
				articleLockInfo.SetCheckedOutStatus();
			}
			catch { }
		}
	}

	public enum LockStatus
    {
        Locked,
        Unlocked
    }
}
