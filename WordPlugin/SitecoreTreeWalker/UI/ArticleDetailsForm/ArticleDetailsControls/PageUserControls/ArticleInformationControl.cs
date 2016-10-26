using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using InformaSitecoreWord.document;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;
using InformaSitecoreWord.User;
using InformaSitecoreWord.Util;
using InformaSitecoreWord.Util.Document;
using ArticleStruct = PluginModels.ArticleStruct;
using StaffStruct = PluginModels.StaffStruct;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
    /// <summary>
    /// Primary control for the Article Information tab
    /// </summary>
    public partial class ArticleInformationControl : ArticleDetailsPageUserControl
    {
        private ArticleDetail _parent;
        protected DocumentCustomProperties _documentCustomProperties;

        private string ArticleNumber;
        public Guid ArticleGuid;
        public bool IsPublished;

        public bool _isLive;

        public ArticleInformationControl()
        {
            InitializeComponent();
        }

        public bool _isCheckedOut;
        public bool IsCheckedOut
        {
            get { return _isCheckedOut; }
            set
            {
                if (value == false)
                { // if we're not checked out, we can't be checked out by me.
                    IsCheckedOutByMe = false;
                }
                _isCheckedOut = value;
            }
        }

        public bool _isCheckedOutByMe;
        public bool IsCheckedOutByMe
        {
            get { return _isCheckedOutByMe; }
            set
            {
                if (value)
                {
                    IsCheckedOut = true;
                }
                _isCheckedOutByMe = value;
            }
        }

        /// <summary>
        /// transfers current material to sitecore
        /// versions it
        /// unlocks the item if current user is the one who has a lock on it
        /// 
        /// Be sure to catch potential WebException
        /// </summary>
        public bool CheckIn(bool save = true)
        {
            bool saved = false;
            try
            {
                //SitecoreAddin.WordApp.ActiveDocument.Saved = true;
                if (save)
                {
                    saved = _parent.SaveArticle();

                    if (!saved)
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.LogException("Error in article details when saving article.", ex);

                if (ex is WebException)
                {
                    throw;
                }
                else
                {
                    MessageBox.Show(@"Error in article details when checking in article.", @"Informa");
                }
            }
            finally
            {
                Guid articleGuid = _parent.ArticleDetails.ArticleGuid;
                if (articleGuid != Guid.Empty)
                {
                    SitecoreClient.CheckInArticle(articleGuid);
                }
                else
                {
                    SitecoreClient.CheckInArticle(_parent.GetArticleNumber());
                }
                IsCheckedOutByMe = false;
                IsCheckedOut = false;
            }
            return saved;
        }

        /// <summary>
        /// Checks out article associated with _parent.ArticleDetails.ArticleGuid
        /// </summary>
        /// <returns></returns>
        public bool CheckOut(bool prompt = false)
        {
            Guid articleGuid = _parent.ArticleDetails.ArticleGuid;
            if (articleGuid == Guid.Empty)
            {
                MessageBox.Show
                    (@"No article associated with file.", @"Informa",
                     MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            try
            {
                bool exists = SitecoreClient.DoesArticleExist(articleGuid);
                if (!exists)
                {
                    MessageBox.Show
                        (@"Article no longer exists on Sitecore", @"Informa",
                         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                ArticleNumber = _parent.ArticleDetails.ArticleNumber;
                //uxArticleNumberLabel.Text = ArticleNumber;


                PluginModels.CheckoutStatus checkedOut = SitecoreClient.GetLockedStatus(articleGuid);

                if (SitecoreClient.DoesArticleHaveText(articleGuid) && prompt)
                {
                    DialogResult dialogResult = MessageBox.Show
                        (@"This article already has some content uploaded. "
                            + @"If you choose to check out the article now and later upload, "
                            + @"you will overwrite that content. "
                            + @"Are you sure you wish to checkout this article?",
                            @"Informa",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
                    if (dialogResult != DialogResult.Yes)
                    {
                        return false;
                    }
                }
                if (!checkedOut.Locked)
                {
                    if (_parent.CloseOnSuccessfulLock)
                    {
                        if (DialogFactory.PromptAutoLock() == DialogResult.Yes)
                        {
                            SitecoreClient.CheckOutArticle(articleGuid, SitecoreUser.GetUser().Username);
                        }
                    }
                    else
                    {
                        SitecoreClient.CheckOutArticle(articleGuid, SitecoreUser.GetUser().Username);
                    }
                }
                SetCheckedOutStatus();
                if (_parent.CloseOnSuccessfulLock && IsCheckedOutByMe)
                {
                    return true;
                }
                //establish link, regardless of lock status
                _parent.SetArticleDetails(SitecoreClient.ForceReadArticleDetails(articleGuid));
                _parent.UpdateFields();
                return true;
            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.LogException("Error in article details when checking out article: " + articleGuid, ex);
                throw;
            }
        }

        /// <summary>
        /// Ascertain valid article number
        /// Link to article
        /// Indicate item is locked
        /// </summary>
        /// <param name="articleNumber"></param>
        /// <param name="prompt">Flag to prompt user if problem occurs</param>
        public bool CheckOut(string articleNumber, bool prompt = true)
        {
            if (articleNumber.IsNullOrEmpty())
            {
                MessageBox.Show
                    (@"Please enter an article number to link to.", @"Informa",
                     MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            try
            {
                bool exists = SitecoreClient.DoesArticleExist(articleNumber);
                if (!exists && prompt)
                {
                    MessageBox.Show
                        (@"Article number entered does not exist.", @"Informa",
                         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                ArticleNumber = articleNumber;
                //uxArticleNumberLabel.Text = articleNumber;

                PluginModels.CheckoutStatus checkedOut = SitecoreClient.GetLockedStatus(articleNumber);

                if (!checkedOut.Locked)
                { //if unlocked, then lock it by current user
                    if (SitecoreClient.DoesArticleHaveText(articleNumber) && prompt)
                    {
                        DialogResult dialogResult = MessageBox.Show
                            (@"This article already has some content uploaded. "
                             + @"If you choose to check out the article now and later upload, "
                             + @"you will overwrite that content. "
                             + @"Are you sure you wish to checkout this article?",
                             @"Informa",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);
                        if (dialogResult != DialogResult.Yes)
                        {
                            return false;
                        }
                    }
                    SitecoreClient.CheckOutArticle(articleNumber, SitecoreUser.GetUser().Username);
                }
                SetCheckedOutStatus();
                if (_parent.CloseOnSuccessfulLock && IsCheckedOutByMe) return true;
                //establish link, regardless of lock status
                _parent.SetArticleDetails(SitecoreClient.ForceReadArticleDetails(articleNumber));
                _parent.UpdateFields();
                return true;
            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.LogException("Error in article details when checking out article: [" + articleNumber + "]", ex);
                throw;
            }
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
            _documentCustomProperties = new DocumentCustomProperties(SitecoreAddin.ActiveDocument);
            string articleNum = _parent.GetArticleNumber();
            if (!articleNum.IsNullOrEmpty())
            { //document is linked to an article
                SetArticleNumber(articleNum);
                PluginModels.CheckoutStatus checkedOut;
                if (_parent.ArticleDetails.ArticleGuid != Guid.Empty)
                {
                    checkedOut = SitecoreClient.GetLockedStatus(_parent.ArticleDetails.ArticleGuid);
                }
                else
                {
                    checkedOut = SitecoreClient.GetLockedStatus(articleNum);
                }
                IsCheckedOut = checkedOut.Locked;
                if (IsCheckedOut)
                {
                    if (SitecoreUser.GetUser().Username == checkedOut.User)
                    { //locked by me

                        IndicateCheckedOutByMe(checkedOut);
                    }
                    else
                    { //locked by other
                        IndicateCheckedOutByOther(checkedOut);
                    }
                    //uxLockStatusLabel.Text = @"Locked";
                }
                else
                { //unlocked
                    IndicateUnlocked();
                }
                //uxRefreshStatus.Enabled = true;
            }
            else
            { //document is not linked to an article
                DocumentProtection.Unprotect(_documentCustomProperties);
                IsCheckedOutByMe = false;
                IsCheckedOut = false;

                _parent.PreLinkEnable();
            }
        }

        private void IndicateUnlocked()
        {
            _parent.PreLinkEnable();
            IndicatedUnfavoredLink();
            IsCheckedOutByMe = false;
            IsCheckedOut = false;
            DocumentProtection.Protect(_documentCustomProperties);
        }

        public void IndicatedUnfavoredLink()
        {
            //uxLinkToDocumentPanel.Visible = false;
            //uxLockStatus.Visible = true;
            _parent.articleStatusBar1.ChangeLockButtonStatus(LockStatus.Unlocked);
            //uxVersionStatus.Visible = true;
            uxPublication.Enabled = false;
            //_parent.EnablePreview();
            //_parent.HideCreationButtons();

            uxSelectAuthor.Enabled = false;
            //uxSelectedAuthors.Enabled = false;
            //uxSelectedAuthors.DisableEdit = true;
            uxAddAuthor.Enabled = false;
        }

        public void UpdateControlsForLockedStatus()
        {
            uxSelectAuthor.Enabled = true;
            uxAddAuthor.Enabled = true;
            uxSelectedAuthors.Enabled = true;
            uxPublication.Enabled = false;
            uxLabel.Enabled = true;
            uxWebPublishDate.Enabled = true;
            uxWebPublishTime.Enabled = true;
            uxEmbargoed.Enabled = true;
            uxMediaTypes.Enabled = true;
            uxNotes.Enabled = true;
        }

        public void UpdateControlsForUnlockedStatus()
        {
            uxSelectAuthor.Enabled = false;
            uxAddAuthor.Enabled = false;
            uxSelectedAuthors.Enabled = false;
            uxPublication.Enabled = false;
            uxLabel.Enabled = false;
            uxWebPublishDate.Enabled = false;
            uxWebPublishTime.Enabled = false;
            uxEmbargoed.Enabled = false;
            uxMediaTypes.Enabled = false;
            uxNotes.Enabled = false;
        }

        /// <summary>
        /// Enables/disables some controls since it's so similar to a PreLinkEnable state
        /// </summary>
        /// <param name="checkedOut"></param>
        public void IndicateCheckedOutByOther(PluginModels.CheckoutStatus checkedOut)
        {
            //uxLockStatus.BackColor = Color.FromArgb(255, 244, 204, 204);

            //uxLockUser.Text = FormatUserName(checkedOut.User);

            IsCheckedOutByMe = false;

            _parent.PreLinkEnable();
			IndicatedUnfavoredLink();
			_parent.EnablePreview();
			_parent.HideCreationButtons();

			//_parent.articleStatusBar1.up
			_parent.articleStatusBar1.ChangeLockButtonStatus(LockStatus.Locked);
            //IndicatedUnfavoredLink();
            DocumentProtection.Protect(_documentCustomProperties);
        }

        public void IndicateCheckedOutByMe(PluginModels.CheckoutStatus checkedOut)
        {
            DocumentProtection.Unprotect(_documentCustomProperties);
            IsCheckedOutByMe = true;
            if (_parent.CloseOnSuccessfulLock && CheckWordDocVersion(_parent.ArticleDetails, false))
            {
                _parent.Close();
                return;
            }
            _parent.CloseOnSuccessfulLock = false;
            //uxLockStatus.BackColor = Color.FromArgb(255, 217, 234, 211);

            //uxLockUser.Text = FormatUserName(checkedOut.User);
            _parent.PostLinkEnable();
            //uxUnlockButton.Visible = true;
            //uxLockButton.Visible = false;
            //uxUnlockButton.Enabled = true;
        }

        public void InitializePublications()
        {
            List<PluginModels.ItemStruct> publications = SitecoreClient.GetPublications();
            publications.Insert(0, new PluginModels.ItemStruct { ID = Guid.Empty, Name = "Select Publication" });
            uxPublication.DataSource = publications;
            uxPublication.DisplayMember = "Name";
            uxPublication.ValueMember = "ID";
        }

        public void InitializeMediaType()
        {
            List<PluginModels.ItemStruct> mediaTypes = SitecoreClient.GetMediaTypes();
            mediaTypes.Insert(0, new PluginModels.ItemStruct { ID = Guid.Empty, Name = "Select Media Type" });
            uxMediaTypes.DataSource = mediaTypes;
            uxMediaTypes.DisplayMember = "Name";
            uxMediaTypes.ValueMember = "ID";
        }

        public void InitializeContentType()
        {
            List<PluginModels.ItemStruct> contentTypes = SitecoreClient.GetContentTypes();
            contentTypes.Insert(0, new PluginModels.ItemStruct { ID = Guid.Empty, Name = "Select Content Type" });
            uxLabel.DataSource = contentTypes;
            uxLabel.DisplayMember = "Name";
            uxLabel.ValueMember = "ID";
        }

        public void LinkToParent(ArticleDetail parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Returns Guid of selected publication, or an empty Guid of none selected
        /// </summary>
        /// <returns></returns>
        public Guid GetSelectedPublicationGuid()
        {
            try
            {
                if (uxPublication.SelectedValue.GetType() == typeof(PluginModels.ItemStruct))
                {
                    return ((PluginModels.ItemStruct)uxPublication.SelectedValue).ID;
                }
                return (Guid)uxPublication.SelectedValue;
            }
            catch
            {
                return Guid.Empty;
            }
        }


        public string GetDisplayName(PluginModels.ArticleSize articleSize, int wordCount)
        {
            string displayName = articleSize.Name;
            if (articleSize.MinimumWordCount < 0 && articleSize.MaximumWordCount > 0)
            {
                displayName += " (<" + articleSize.MaximumWordCount + " words)";
                if (wordCount < articleSize.MaximumWordCount)
                {
                    displayName += " (recommended)";
                }
            }
            else if (articleSize.MinimumWordCount > 0 && articleSize.MaximumWordCount > 0)
            {
                displayName += " (" + articleSize.MinimumWordCount + "-" + articleSize.MaximumWordCount + " words)";
                if (wordCount < articleSize.MaximumWordCount && wordCount > articleSize.MinimumWordCount)
                {
                    displayName += " (recommended)";
                }
            }
            else if (articleSize.MinimumWordCount > 0 && articleSize.MaximumWordCount < 0)
            {
                displayName += " (>" + articleSize.MinimumWordCount + " words)";
                if (wordCount > articleSize.MinimumWordCount)
                {
                    displayName += " (recommended)";
                }
            }

            return displayName;
        }

        public string GetSelectedPublicationName()
        {
            return ((PluginModels.ItemStruct)uxPublication.SelectedItem).Name;
        }

        public List<StaffStruct> GetSelectedAuthors()
        {
            return uxSelectedAuthors.Selected;
        }

        public string GetNotes()
        {
            return uxNotes.Text;
        }

        public Guid GetLabelGuid()
        {
            try
            {
                if (uxLabel.SelectedValue.GetType() == typeof(PluginModels.ItemStruct))
                {
                    return ((PluginModels.ItemStruct)uxLabel.SelectedValue).ID;
                }
                return (Guid)uxLabel.SelectedValue;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public Guid GetMediaTypeGuid()
        {
            try
            {
                if (uxMediaTypes.SelectedValue.GetType() == typeof(PluginModels.ItemStruct))
                {
                    return ((PluginModels.ItemStruct)uxMediaTypes.SelectedValue).ID;
                }
                return (Guid)uxMediaTypes.SelectedValue;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Gets the web publish date (as a combination of date and time).
        /// </summary>
        /// <returns></returns>
        public DateTime GetWebPublishDate()
        {
            var localDate = uxWebPublishDate.Value.Date.Add(uxWebPublishTime.Value.TimeOfDay);
            
            Globals.SitecoreAddin.Log("GetWebPublishDate: Date before DateTime conversion: [" +
                                      localDate.Date.ToString() + "].");
            Globals.SitecoreAddin.Log("GetWebPublishDate: Time before DateTime conversion:  [" +
                                      localDate.TimeOfDay.ToString() + "].");

            DateTime convertedTime = localDate; //TimeZoneInfo.ConvertTime(localDate, TimeZoneInfo.Local, TimeZoneInfo.Utc);

            Globals.SitecoreAddin.Log("GetWebPublishDate: Date after DateTime conversion: [" +
                                      convertedTime.Date.ToString() + "].");
            Globals.SitecoreAddin.Log("GetWebPublishDate: Time after DateTime conversion:  [" +
                                      convertedTime.TimeOfDay.ToString() + "].");

            return convertedTime;
        }

        /// <summary>
        /// Gets the web publish date.
        /// </summary>
        public string GetProperDate()
        {
            return GetWebPublishDate().ToString();
        }

        public string GetArticleNumber()
        {
            return ArticleNumber;
        }

        public bool GetEmbargoedState()
        {
            return uxEmbargoed.Checked;
        }

        public void SetArticleVersionInformation(ArticleStruct articleStruct, bool outOfDate)
        {
            if (outOfDate)
            {
                Color color = (Color)new ColorConverter().ConvertFromString("#F4CCCC");
                _parent.articleStatusBar1.uxVersionStateButton.BackColor = color;
                _parent.articleStatusBar1.uxVersionStateButton.ToolTipText = "More Recent Version Available. Download it from Sitecore. Click for more information.";
                _parent.articleStatusBar1.uxVersionStateButton.Text = "Out of Date";


                //uxVersionStatus.BackColor = Color.FromArgb(255, 244, 204, 204);
                //_parent.articleStatusBar1.uxVersionStateButton.BackColor = Color.FromArgb(255, 244, 204, 204);

                //uxVersionText.Text = @"More Recent Version Available";
                //uxVersionText.Font = new Font(uxVersionText.Font, FontStyle.Italic | FontStyle.Bold);
            }
            else
            {
                Color color = (Color)new ColorConverter().ConvertFromString("#d9ead3");
                _parent.articleStatusBar1.uxVersionStateButton.BackColor = color;
                _parent.articleStatusBar1.uxVersionStateButton.ToolTipText = "Document Content is Up To Date. Click for more information.";
                _parent.articleStatusBar1.uxVersionStateButton.Text = "Up To Date";
                //uxVersionStatus.BackColor = Color.FromArgb(255, 217, 234, 211);
                //uxVersionText.Text = @"Document Content is Up To Date";
                //uxVersionText.Font = new Font(uxVersionText.Font, FontStyle.Bold);
            }

            //uxLastUpdateDate.Text = articleStruct.WordDocLastUpdateDate;
            //uxLastUpdatedBy.Text = FormatUserName(articleStruct.WordDocLastUpdatedBy);
        }

        public void UpdateFields(ArticleStruct articleDetails)
        {
            InitializePublications();
            InitializeMediaType();
            InitializeContentType();
            SetCheckedOutStatus();
            if (string.IsNullOrEmpty(articleDetails.ArticleNumber))
            {
                return;
            }
            uxPublication.SelectedValue = articleDetails.Publication;
            if (articleDetails.Authors != null)
            {
                uxSelectedAuthors.PopulateRegular(articleDetails.Authors.Select(r => new StaffStruct { ID = r.ID, Name = r.Name }).ToList());
            }

            //uxArticleNumberLabel.Text = articleDetails.ArticleNumber;
            if (articleDetails.WebPublicationDate > DateTime.MinValue)
            {
                SetPublicationTime(articleDetails.WebPublicationDate, true);
            }

            ArticleNumber = articleDetails.ArticleNumber;
            ArticleGuid = articleDetails.ArticleGuid;
            IsPublished = articleDetails.IsPublished;
            uxEmbargoed.Checked = articleDetails.Embargoed;
            uxMediaTypes.SelectedValue = articleDetails.MediaType;
            uxLabel.SelectedValue = articleDetails.Label;
            uxNotes.Text = articleDetails.NotesToEditorial;
            CheckWordDocVersion(articleDetails);

            _isLive = articleDetails.IsPublished;
            label1.Refresh();
        }

        public void SetPublicationTime(DateTime publicationDate, bool isLocal)
        {
            TimeZoneInfo.ClearCachedData();

            if (isLocal)
            {
                Globals.SitecoreAddin.Log("SetPublicationTime: Setting date field to [" + publicationDate.Date.ToString() + "].");
                uxWebPublishDate.Value = publicationDate.Date;
                Globals.SitecoreAddin.Log("SetPublicationTime: Setting time field to [" + publicationDate.TimeOfDay.ToString() + "].");
                uxWebPublishTime.Value = publicationDate;
            }
            else
            {
                Globals.SitecoreAddin.Log("SetPublicationTime: Date before DateTime conversion: [" +
                                          publicationDate.Date.ToString() + "].");
                Globals.SitecoreAddin.Log("SetPublicationTime: Time before DateTime conversion:  [" +
                                          publicationDate.TimeOfDay.ToString() + "].");

                //DateTime convertedTime = TimeZoneInfo.ConvertTime(publicationDate, TimeZoneInfo.Utc, TimeZoneInfo.Local);
                //DateTime convertedTime = TimeZoneInfo.ConvertTime(publicationDate, TimeZoneInfo.Utc).ToLocalTime();
                DateTime convertedTime = publicationDate;
                Globals.SitecoreAddin.Log("SetPublicationTime: Date after DateTime conversion: [" +
                                          convertedTime.Date.ToString() + "].");
                Globals.SitecoreAddin.Log("SetPublicationTime: Time after DateTime conversion:  [" +
                                          convertedTime.TimeOfDay.ToString() + "].");

                uxWebPublishDate.Value = convertedTime.Date;
                uxWebPublishTime.Value = convertedTime;

            }
        }

        public void UpdateArticleNumber(string articleNumber)
        {
            //uxArticleNumberLabel.Text = articleNumber;
        }

        /// <summary>
        /// Updates the list of authors according to the selected publication
        /// If no publication is selected, all authors are available.
        /// </summary>
        public void UpdateAuthorsList()
        {
            var matchingAuthors = SitecoreClient.LazyReadAuthors();
            uxSelectAuthor.DataSource = null;

            if (matchingAuthors.Count == 0 || (matchingAuthors.Count == 1 && matchingAuthors[0].ID == Guid.Empty))
            {

                if (matchingAuthors.Count == 0)
                {
                    matchingAuthors.Add(new StaffStruct
                    {
                        ID = Guid.Empty,
                        Name = "No authors found!"
                    });
                }
            }

            uxSelectAuthor.DataSource = matchingAuthors;
            uxSelectAuthor.DisplayMember = "Name";
            uxSelectAuthor.ValueMember = "ID";
        }

        public void DisableAuthorControlsIfNoAuthors()
        {
            if (uxSelectAuthor.Items.Count == 0 || (uxSelectAuthor.Items.Count == 1 && ((StaffStruct)uxSelectAuthor.Items[0]).ID == Guid.Empty))
            {
                uxSelectAuthor.Enabled = false;
                uxAddAuthor.Enabled = false;
            }
        }

        /// <summary>
        /// Enables and disables the controls necessary for editing once the document
        /// has been linked to a Sitecore article
        /// </summary>
        public void PostLinkEnable()
        {
            uxPublication.Enabled = false;
            //uxLinkDocument.Enabled = false;
            uxSelectAuthor.Enabled = true;
            uxSelectedAuthors.Enabled = true;
            DisableAuthorControlsIfNoAuthors();
            uxSelectedAuthors.DisableEdit = false;
            uxAddAuthor.Enabled = true;

            //uxLinkToDocumentPanel.Visible = false;
            //uxLockStatus.Visible = true;
            //uxVersionStatus.Visible = true;

            //uxUnlinkDocument.Visible = true;
        }

        /// <summary>
        /// Disables and disables the controls necessary for editing once the document
        /// has not been linked to a Sitecore article
        /// </summary>
        public void PreLinkEnable()
        {
            uxPublication.Enabled = true;
            //uxLinkDocument.Enabled = true;

            uxSelectAuthor.Enabled = true;
            uxAddAuthor.Enabled = true;
            DisableAuthorControlsIfNoAuthors();
            uxSelectedAuthors.Enabled = true;
            uxSelectedAuthors.DisableEdit = false;


            //uxLinkToDocumentPanel.Visible = true;
            //uxLockStatus.Visible = false;
            //uxVersionStatus.Visible = false;

            //uxUnlinkDocument.Visible = false;
        }

        public void ResetFields()
        {
            uxPublication.SelectedIndex = 0;
            SetPublicationTime(DateTime.Today, true);
            uxMediaTypes.SelectedIndex = 0;
            uxLabel.SelectedIndex = 0;
            uxSelectedAuthors.Reset();
            uxNotes.Text = string.Empty;
            MenuItem.SetIndicatorIcon(Properties.Resources.redx);
        }

        public void IndicateChanged()
        {
            if (MenuItem != null)
            {
                MenuItem.HasChanged = true;
                MenuItem.UpdateBackground();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if local document is up-to-date; else false</returns>
        public bool CheckWordDocVersion()
        {
            return CheckWordDocVersion(SitecoreClient.ForceReadArticleDetails(GetArticleNumber()));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="articleStruct"></param>
        /// <param name="prompt"></param>
        /// <returns>True if local document is up-to-date; else false</returns>
        public bool CheckWordDocVersion(ArticleStruct articleStruct, bool prompt = true)
        {
            int localWordDocVersion = _documentCustomProperties.WordSitecoreVersionNumber;

            int sitecoreWordDocVersion = articleStruct.WordDocVersionNumber;
            SetArticleVersionInformation(articleStruct, (sitecoreWordDocVersion > localWordDocVersion));
            if ((sitecoreWordDocVersion > localWordDocVersion))
            { //we're out of date!
                if (prompt)
                {
                    string message =
                                "A newer version of this article  was uploaded on {0} by {1}. " +
                                "If you continue using this version, you may overwrite more recent " +
                                "changes. To get the most recent version, navigate to Sitecore to download the new document.";

                    MessageBox.Show
                        (String.Format(message, articleStruct.WordDocLastUpdateDate, articleStruct.WordDocLastUpdatedBy),
                         @"Informa",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Exclamation);
                }
                return false;
            }
            return true;
        }

        public void SetArticleNumber(string articleNumber)
        {
            _parent.SetArticleNumber(articleNumber);
            ArticleNumber = articleNumber;
        }


        public string FormatUserName(string userNameWithDomain)
        {
            if (userNameWithDomain.IsNullOrEmpty())
            { return userNameWithDomain; }

            string formattedUserName = userNameWithDomain;

            if (formattedUserName.IndexOf("\\") > 0)
            {
                formattedUserName = formattedUserName.Substring(formattedUserName.IndexOf("\\") + 1);
            }

            return formattedUserName;
        }


        private void uxLockButton_Click(object sender, EventArgs e)
        {
            /*
			if (!SitecoreClient.DoesArticleExist(uxArticleNumberLabel.Text)) return;
			if(SitecoreClient.CheckOutArticle(uxArticleNumberLabel.Text, SitecoreUser.GetUser().Username))
			{
				CheckWordDocVersion(_parent.ArticleDetails);
			}
			SetCheckedOutStatus();
             * */
        }

        /*
		private void uxUnlockButton_Click(object sender, EventArgs e)
		{
			if (!SitecoreClient.DoesArticleExist(uxArticleNumberLabel.Text)) return;
			SitecoreClient.CheckInArticle(uxArticleNumberLabel.Text);
			SetCheckedOutStatus();
		}
         * */

        private void uxAddAuthor_Click(object sender, EventArgs e)
        {
            var selectedAuthor = (StaffStruct)uxSelectAuthor.SelectedItem;
            uxSelectedAuthors.Add(selectedAuthor);
            IndicateChanged();
        }

        private void uxPublication_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAuthorsList();
            IndicateChanged();
        }

        private void uxWebPublishDate_ValueChanged(object sender, EventArgs e)
        {
            IndicateChanged();
        }

        private void ArticleInformationControl_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                var wordApp = SitecoreAddin.ActiveDocument.Application;
                if (wordApp == null) return;
                bool currentSavedState = SitecoreAddin.ActiveDocument.Saved;
                SitecoreAddin.ActiveDocument.Saved = currentSavedState;
                _documentCustomProperties = new DocumentCustomProperties(SitecoreAddin.ActiveDocument);
            }
        }

        private void label1_Paint(object sender, PaintEventArgs e)
        {
            if (_isLive)
            {
                e.Graphics.DrawImage(Properties.Resources.live, 570, 1, 25, 25);
                e.Graphics.DrawString("Published!", new Font("SegoeUI", 17), Brushes.White, 450, 1);
            }
        }

        public ArticleInformationControl GetObject()
        {
            return this;
        }
    }
}
