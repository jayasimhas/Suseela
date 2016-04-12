using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using InformaSitecoreWord.document;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.User;
using PluginModels;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
    public partial class ArticleLockInfo : Form
    {
        string _articleNumber;
        private ArticleDetail _parent;
        private ArticleInformationControl _articleInformationControl;
        private ArticleStatusBar _statusBar;
        protected DocumentCustomProperties DocumentCustomProperties;
        public ArticleLockInfo()
        {
            InitializeComponent();
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

        public void SetArticleNumber(string articleNum)
        {
            _articleNumber = articleNum;
        }

        private void uxUnlockButton_Click(object sender, System.EventArgs e)
        {
            _articleNumber = _parent.GetArticleNumber();
            if (!SitecoreClient.DoesArticleExist(_articleNumber)) return;
            SitecoreClient.CheckInArticle(_articleNumber);
            SetCheckedOutStatus();
            Close();
        }

        private void uxLockButton_Click(object sender, System.EventArgs e)
        {
            if (!SitecoreClient.DoesArticleExist(_articleNumber)) return;

            var lockPrompt = new ArticleLockConfirmation
            {
                StartPosition = FormStartPosition.CenterParent,
                YesAction = LockYesActionMethod,
                NoAction = LockNoActionMethod
            };
            lockPrompt.ShowDialog();
            Close();
        }

        /// <summary>
        /// This method is used to perform the lock action. This is called when "Yes" is clicked in the lock confirmation box.
        /// </summary>
        public void LockYesActionMethod()
        {
            if (SitecoreClient.CheckOutArticle(_articleNumber, SitecoreUser.GetUser().Username))
            {
                _articleInformationControl.CheckWordDocVersion(_parent.ArticleDetails);
            }
            SetCheckedOutStatus();
        }

        /// <summary>
        /// This method is used to perform the lock action. This is called when "No" is clicked in the lock confirmation box.
        /// </summary>
        public void LockNoActionMethod()
        {
            return;
        }

        /// <summary>
        /// Get from sitecore checked out status, then set control accordingly
        /// 
        /// Unprotects document if locked by current user
        /// </summary>
        public void SetCheckedOutStatus()
        {
            if (_parent == null || _parent.ArticleDetails == null)
            {
                return;
            }
            DocumentCustomProperties = new DocumentCustomProperties(SitecoreAddin.ActiveDocument);
            string articleNum = _parent.GetArticleNumber();
            if (!string.IsNullOrEmpty(_articleNumber))
            { //document is linked to an article
                SetArticleNumber(articleNum);
                CheckoutStatus checkedOut;
                if (_parent.ArticleDetails.ArticleGuid != Guid.Empty)
                {
                    checkedOut = SitecoreClient.GetLockedStatus(_parent.ArticleDetails.ArticleGuid);
                }
                else
                {
                    checkedOut = SitecoreClient.GetLockedStatus(articleNum);
                }
                _articleInformationControl.IsCheckedOut = checkedOut.Locked;
                if (_articleInformationControl.IsCheckedOut)
                {
                    if (SitecoreUser.GetUser().Username == checkedOut.User)
                    { //locked by me
                        IndicateCheckedOutByMe(checkedOut);
                        _parent.articleStatusBar1.ChangeLockButtonStatus(LockStatus.Locked);
                        _articleInformationControl.UpdateControlsForLockedStatus();
                    }
                    else
                    { //locked by other
                        IndicateCheckedOutByOther(checkedOut);
                        _parent.articleStatusBar1.ChangeLockButtonStatus(LockStatus.Locked);
                        _articleInformationControl.UpdateControlsForUnlockedStatus();
                    }
                    uxLockStatusLabel.Text = @"Locked";
                }
                else
                { //unlocked
                    IndicateUnlocked();
                    _articleInformationControl.UpdateControlsForUnlockedStatus();
                }
                uxRefreshStatus.Enabled = true;
            }
            else
            { //document is not linked to an article
                DocumentProtection.Unprotect(DocumentCustomProperties);
                _articleInformationControl.IsCheckedOutByMe = false;
                _articleInformationControl.IsCheckedOut = false;
                _parent.PreLinkEnable();
            }
        }

        private void IndicateUnlocked()
        {
            _parent.PreLinkEnable();
            IndicatedUnfavoredLink();

            uxLockUser.Text = @"N\A";
            uxLockStatusLabel.Text = @"Unlocked";

            _articleInformationControl.IsCheckedOutByMe = false;
            _articleInformationControl.IsCheckedOut = false;

            uxUnlockButton.Visible = false;
            uxLockButton.Visible = true;
            uxLockButton.Enabled = true;
            DocumentProtection.Protect(DocumentCustomProperties);
            _statusBar.ChangeLockButtonStatus(LockStatus.Unlocked);
        }

        private void IndicatedUnfavoredLink()
        {
            _parent.EnablePreview();
            _parent.HideCreationButtons();
            _articleInformationControl.uxSelectedAuthors.Enabled = false;
            _articleInformationControl.uxSelectedAuthors.DisableEdit = true;

            _articleInformationControl.IndicatedUnfavoredLink();  // Might needs to be remove this code later.
        }

        /// <summary>
        /// Enables/disables some controls since it's so similar to a PreLinkEnable state
        /// </summary>
        /// <param name="checkedOut"></param>
        public void IndicateCheckedOutByOther(CheckoutStatus checkedOut)
        {
            var lockUserInfo = SitecoreClient.GetFullNameAndEmail(checkedOut.User);
            if (lockUserInfo.Count == 2)
            {
                uxLockUser.Text = lockUserInfo[0];
                label1.Text = lockUserInfo[1];
            }
            _articleInformationControl.IsCheckedOutByMe = false;
            _parent.PreLinkEnable();
            IndicatedUnfavoredLink();
            uxUnlockButton.Visible = true;
            uxUnlockButton.Enabled = false;
            uxLockButton.Visible = false;
            DocumentProtection.Protect(DocumentCustomProperties);
        }

        public void IndicateCheckedOutByMe(CheckoutStatus checkedOut)
        {
            DocumentProtection.Unprotect(DocumentCustomProperties);
            _articleInformationControl.IsCheckedOutByMe = true;
            if (_parent.CloseOnSuccessfulLock && _articleInformationControl.CheckWordDocVersion(_parent.ArticleDetails, false))
            {
                _parent.Close();
                return;
            }
            _parent.CloseOnSuccessfulLock = false;
            var lockUserInfo = SitecoreClient.GetFullNameAndEmail(checkedOut.User);
            if (lockUserInfo.Count == 2)
            {
                uxLockUser.Text = lockUserInfo[0];
                label1.Text = lockUserInfo[1];
            }
            //			uxLockUser.Text = _articleInformationControl.FormatUserName(checkedOut.User);
            _parent.PostLinkEnable();
            uxUnlockButton.Visible = true;
            uxLockButton.Visible = false;
            uxUnlockButton.Enabled = true;
            _statusBar.ChangeLockButtonStatus(LockStatus.Locked);
        }

        private void ArticleLockInfo_Load(object sender, EventArgs e)
        {
            SetCheckedOutStatus();
        }

        private void uxRefreshStatus_Click(object sender, EventArgs e)
        {
            SetCheckedOutStatus();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Label ll = (Label)sender;
            Process.Start("mailto:" + ll.Text);
        }
    }
}
