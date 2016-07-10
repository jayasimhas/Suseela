using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;
using InformaSitecoreWord.Config;
using InformaSitecoreWord.Custom_Exceptions;
using InformaSitecoreWord.document;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls;
using InformaSitecoreWord.User;
using InformaSitecoreWord.Util;
using InformaSitecoreWord.Util.Document;
using InformaSitecoreWord.WebserviceHelper;
using PluginModels;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;
using ArticleStruct = PluginModels.ArticleStruct;

namespace InformaSitecoreWord.UI.ArticleDetailsForm
{
    public partial class ArticleDetail : Form
    {
        #region Class Members
        //null if no sitecore article item is associated
        public ArticleStruct ArticleDetails = new ArticleStruct();
        public bool CloseOnSuccessfulLock;

        private DocumentCustomProperties _documentCustomProperties;
        private readonly SitecoreClient _sitecoreArticle;
        private WordUtils _wordUtils;
        protected StructConverter _structConverter;

        public bool _Live;

        #endregion

        #region MinorUIManipulation

        public void HideCreationButtons()
        {
            uxCreateArticle.Visible = false;
            uxSaveMetadata.Visible = false;
            uxSaveMetadata.Visible = false;
        }

        /// <summary>
        /// Sets the enabled statuses and visibilities of the controls
        /// for an unlinked document
        /// </summary>
        public void PreLinkEnable()
        {
            uxCreateArticle.Visible = true;
            uxSaveMetadata.Visible = false;
            uxSaveArticle.Visible = false;
            articleDetailsPageSelector.PreLinkEnable();
            articleDetailsPageSelector.pageWorkflowControl.PreLinkEnable();
            if (workflowChange_UnlockOnSave)
            {
                EnablePreview();
                uxCreateArticle.Visible = false;
            }
            else
            {
                DisablePreview();
            }

            Refresh();
        }

        /// <summary>
        /// Sets the enabled statuses and visibilities of the controls
        /// for a linked document
        /// </summary>
        public void PostLinkEnable()
        {
            uxCreateArticle.Visible = false;
            uxSaveMetadata.Visible = true;
            uxSaveArticle.Visible = true;
            EnablePreview();
            articleDetailsPageSelector.PostLinkEnable();
            articleDetailsPageSelector.pageWorkflowControl.PostLinkEnable();
            Refresh();
        }

        public void EnablePreview()
        {
            uxPreview.Text = @"Preview Article";
            uxPreview.Visible = true;
            uxPreview.Enabled = true;
        }

        public void DisablePreview()
        {
            //uxPreview.Text = @"Article Not Linked";
            uxPreview.Visible = false;
            uxPreview.Enabled = false;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ArticleDetail()
        {
            SitecoreAddin.TagActiveDocument();

            _sitecoreArticle = new SitecoreClient();
            _documentCustomProperties = new DocumentCustomProperties(SitecoreAddin.ActiveDocument);
            _structConverter = new StructConverter();
            ArticleDetails.ArticleNumber = _documentCustomProperties.ArticleNumber;

            InitializeComponent();

            articleDetailsPageSelector.LinkToParent(this);


            articleStatusBar1.LinkToParent(this);
            if (!string.IsNullOrEmpty(_documentCustomProperties.ArticleNumber))
            {
                articleStatusBar1.DisplayStatusBar(true, _documentCustomProperties.ArticleNumber);
            }
            else
            {
                articleStatusBar1.DisplayStatusBar(false, _documentCustomProperties.ArticleNumber);
            }

            if (this.articleDetailsPageSelector.pageArticleInformationControl._isCheckedOut)
            {
                articleStatusBar1.ChangeLockButtonStatus(LockStatus.Unlocked);
            }
            else
            {
                articleStatusBar1.ChangeLockButtonStatus(LockStatus.Locked);
            }

            articleDetailsPageSelector.InitializePages();
            SitecoreUser.GetUser().ResetAuthenticatedSubscription();
            SitecoreUser.GetUser().Authenticated += PopulateFieldsOnAuthentication;
            InitializeLogin();

        }

        /// <summary>
        /// Initializes the fields based on the information associated with the document,
        /// or lack thereof
        /// </summary>
        private void InitializeFields()
        {
            _documentCustomProperties = new DocumentCustomProperties(SitecoreAddin.ActiveDocument);
            SetArticleNumber(_documentCustomProperties.ArticleNumber);
            string articleNumber = GetArticleNumber();
            if (!articleNumber.IsNullOrEmpty())
            {
                SetArticleDetails(SitecoreClient.LazyReadArticleDetails(GetArticleNumber()));
                PostLinkEnable();
            }
            else
            {
                PreLinkEnable();
            }
        }

        private void InitializeLogin()
        {
            try
            {
                loginControl1.ToReveal = uxArticlePanel;
                loginControl1.ShowLogin();


                if (!SitecoreClient.IsAvailable())
                {
                    SitecoreUser.GetUser().Logout();
                }
                if (SitecoreUser.GetUser().IsLoggedIn)
                {
                    _wordUtils = new WordUtils();
                    loginControl1.HideLogin();
                    UpdateFieldsUsingSitecore();
                }
            }
            catch (UnauthorizedAccessException uax)
            {
                Globals.SitecoreAddin.LogException("ArticleDetail.InitializeLogin: Unauthorized access to API handler. Must re-login", uax);
                throw uax;
            }
            catch (Exception ex)
            {
                var ax = new ApplicationException("ArticleDetail.InitializeLogin: Error setting up the login screen!", ex);
                Globals.SitecoreAddin.LogException(ax.Message, ex);
                throw ax;
            }
        }

        private void PopulateFieldsOnAuthentication(object sender, EventArgs e)
        {
            SuspendLayout();
            _wordUtils = new WordUtils();
            InitializeFields();
            ESRibbon ribbon = Globals.Ribbons.GetRibbon<ESRibbon>();
            ribbon?.IsLoggedIn();
            UpdateFieldsUsingSitecore();
            ResetChangedStatus();
            ResumeLayout();
            CloseOnSuccessfulLock = false; //no longer close on lock; only try once
        }

        public WordUtils GetWordUtils()
        {
            return _wordUtils;
        }

        public static void Open(bool closeOnSuccessfulLock = false)
        {
            Cursor.Current = Cursors.WaitCursor;
            var ad = new ArticleDetail
            {
                CloseOnSuccessfulLock = closeOnSuccessfulLock,
                ShowInTaskbar = true,
                TopLevel = true
            };
            if (!ad.IsDisposed)
            {
                ad.ShowDialog(Globals.SitecoreAddin.Application.ActiveWindow as IWin32Window);
            }
            ad.Closed +=
                delegate
                {
                    Globals.SitecoreAddin.CloseSitecoreTreeBrowser(Globals.SitecoreAddin.Application.ActiveDocument);
                };
            Cursor.Current = Cursors.Default;
        }

        public void SetArticleNumber(string articleNumber)
        {
            Globals.SitecoreAddin.Log("Setting article number to #" + articleNumber);
            ArticleDetails.ArticleNumber = articleNumber;
            _documentCustomProperties.ArticleNumber = articleNumber;
        }

        /// <summary>
        /// Removes local document association by removing the 
        /// document custom property for the article number
        /// </summary>
        public void UnlinkWordFileFromSitecoreItem()
        {
            _documentCustomProperties.SetCustomDocumentPropertyToEmpty(Constants.ArticleNumber);
            _documentCustomProperties.SetCustomDocumentPropertyToEmpty(Constants.WordVersionNumber);
            SitecoreAddin.ActiveDocument.Saved = false;
        }

        /// <summary>
        /// Updates the UI elements based on the ArticleDetails currently stored
        /// </summary>
        public void UpdateFields()
        {
            Globals.SitecoreAddin.Log("Updating fields...");
            articleDetailsPageSelector.UpdateFields(ArticleDetails);
            articleDetailsPageSelector.pageWorkflowControl.UpdateFields(ArticleDetails.ArticleWorkflowState, ArticleDetails);
            articleStatusBar1.RefreshWorkflowDetails();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savingArticle">If true, resets all the HasChanged properties. Else,
        /// does not reset the HasChanged properties belonging to fields that reset only on
        /// saving of the article</param>
        public void ResetChangedStatus(bool savingArticle = false)
        {
            articleDetailsPageSelector.ResetChangedStatus(savingArticle);
        }

        public void ResetFields()
        {
            articleDetailsPageSelector.ResetFields();
            articleDetailsPageSelector.pageWorkflowControl.ResetNotificationList();
            articleStatusBar1.DisplayStatusBar(false, null);

        }

        /// <summary>
        /// Invalidates and repaints the UI 
        /// </summary>
        public void Repaint()
        {
            articleDetailsPageSelector.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Null if no article number has been set to the document; 
        /// otherwise, the article number set to the document</returns>
        public string GetArticleNumber()
        {
            return ArticleDetails.ArticleNumber;
        }

        /// <summary>
        /// Should be equivalent to "IsDocumentLinkedToSitecoreArticleItem"
        /// </summary>
        /// <returns></returns>
        private bool HasArticleNumber()
        {
            return (!string.IsNullOrEmpty(_documentCustomProperties.ArticleNumber));
        }

        /// <summary>
        /// Sets the member ArticleStruct ArticleDetails to the inputted
        /// ArticleStruct articleStruct
        /// </summary>
        /// <param name="articleStruct"></param>
        public void SetArticleDetails(ArticleStruct articleStruct)
        {
            ArticleDetails = articleStruct;
        }

        /// <summary>
        /// 
        /// 
        /// Be sure to catch WebException indicating server could not be contacted
        /// Saves a locked document (and unlocks the local document)
        /// </summary>
        public bool SaveArticle(ArticleDocumentMetadataParser metadataParser = null, string body = null)
        {
            var documentCustomProperties = new DocumentCustomProperties(SitecoreAddin.ActiveDocument);
            var articleNumber = GetArticleNumber();

            if (!string.IsNullOrEmpty(documentCustomProperties.ArticleNumber) && !string.IsNullOrEmpty(articleNumber))
            {
                if (articleNumber != documentCustomProperties.ArticleNumber)
                {
                    string alertMessage = "Article numbers do not match, are you sure you would like to continue? " +
                                          "This can happen accidentally if you switching between documents quickly during saving.";
                    DialogResult result = MessageBox.Show(alertMessage, "Article numbers do not match",
                                                          MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.No)
                    {
                        return false;
                    }
                }
            }

            try
            {
                if (HasArticleNumber())
                {
                    var isPublish = ArticleDetails.IsPublished;
                    var copy = ArticleDetails.ArticleGuid;
                    ArticleDetails = articleDetailsPageSelector.GetArticleDetails(metadataParser);
                    ArticleDetails.ArticleGuid = copy;
                    ArticleDetails.IsPublished = isPublish;
                    _Live = isPublish;
                    //List<string> errors = _sitecoreArticle.SaveArticle(SitecoreAddin.ActiveDocument, ArticleDetails, new Guid(), new StaffStruct[0], GetArticleNumber(), body);
                    //Uncomment this after workflow is tested properly.
                    List<string> errors = _sitecoreArticle.SaveArticle(SitecoreAddin.ActiveDocument, ArticleDetails,
                       articleDetailsPageSelector.pageWorkflowControl.GetSelectedCommand(),
                       articleDetailsPageSelector.pageWorkflowControl.GetNotifyList(),
                       GetArticleNumber(), body,
                       articleDetailsPageSelector.pageWorkflowControl.GetNotificationText());

                    if (errors != null && errors.Any())
                    {
                        foreach (string error in errors)
                        {
                            if (!String.IsNullOrEmpty(error))
                            {
                                MessageBox.Show(error, error.Contains("not secure") ? @" Non-Secure Multimedia Content" : @"Informa");
                            }
                        }
                        Cursor = Cursors.Arrow;
                        return false;
                    }

                    articleDetailsPageSelector.ResetChangedStatus(true);

                }
            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.LogException("Error when saving article!", ex);
                throw;
            }

            //TamerM - 2016-03-22: Prompt and ReExport  NLM FEED
            NLMFeedUtils.PromptAndReExportNLMFeed(ArticleDetails.ArticleNumber, ArticleDetails.IsPublished);

            return true;
        }

        protected void UpdateFieldsUsingSitecore()
        {
            if (!articleDetailsPageSelector.CheckOut())
            {
                if (!string.IsNullOrEmpty(ArticleDetails.ArticleNumber))
                {
                    Globals.SitecoreAddin.Log(String.Format("Article with # [{0}] not found!", ArticleDetails.ArticleNumber));
                    MessageBox.Show(this, @"Unable to retrieve article information from Sitecore. " +
                                          @"Please verify that this article exists on Sitecore and " +
                                          @"try again or contact support if you get this message again.", @"Informa");
                    this.Close();
                }
                else
                {
                    UpdateFields();
                }
            }
            ArticleInformationControl pageArticleInformationControl = articleDetailsPageSelector.pageArticleInformationControl;
            articleDetailsPageSelector.SwitchToPage(pageArticleInformationControl);
            pageArticleInformationControl.MenuItem.HasChanged = false;
            pageArticleInformationControl.MenuItem.UpdateBackground();

            _Live = pageArticleInformationControl._isLive;
        }

        /// <summary>
        /// Prompts the user to continue even though there are multiple sections styled as "title".
        /// </summary>
        /// <returns>True if the user wishes to continue</returns>
        protected bool AskContinueMultipleTitles()
        {
            DialogResult dialogResult = MessageBox.Show
                            (@"There are multiple paragraphs that are styled as Story Title. " +
                                @"Do you wish to proceed?",
                                @"Informa",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);
            return (dialogResult == DialogResult.Yes);
        }

        /// <summary>
        /// Prompts the user that they have exceeded a character limit for a field
        /// </summary>
        /// <param name="field"></param>
        /// <param name="characterLimit"></param>
        /// <returns>True if the user wishes to continure</returns>
        protected bool AskExceededCharacterLimit(string field, int characterLimit)
        {
            string message =
                String.Format("You have exceed the character limit of {0} characters for the {1}. " +
                              "The {1} will be truncated. Do you wish to continue?",
                              characterLimit, field);
            DialogResult dialogResult = MessageBox.Show
                            (message,
                                @"Informa",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);
            return (dialogResult == DialogResult.Yes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if user wishes to continue</returns>
        protected bool AskContinueInvalidStyle()
        {
            DialogResult dialogResult = MessageBox.Show
                (@"Unmapped styles have been identified in your document. " +
                 @"If you wish to proceed with saving, this text will be " +
                 @"transferred with the default story text style. Do you " +
                 @"wish to proceed?",
                 @"Informa",
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question);
            return (dialogResult == DialogResult.Yes);
        }

        /// <summary>
        /// Presents a prompt if title is too long
        /// </summary>
        /// <param name="titleLength"></param>
        /// <returns>True if titleLength is too long; else false</returns>
        protected bool NotifyTitleTooLong(int titleLength)
        {
            int maxLength = ApplicationConfig.GetTitleMaxCharacters();
            if (titleLength > maxLength)
            {
                MessageBox.Show(this, @"Title is too long! Title character limit is " + maxLength +
                    @". Current title is " + titleLength + @" characters long!", @"Informa");
                return true;
            }
            return false;
        }

        public DialogResult AlertConnectionFailure()
        {
            return MessageBox.Show
                (@"Sitecore server could not be contacted! Please try again in a few minutes." + Environment.NewLine +
                 Environment.NewLine + @"If the problem persists, contact your system administrator.",
                 @"Informa",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Error);
        }

        /// <summary>
        /// Creates a new Sitecore Article Item based on the selected publication,
        /// date, issue, and title. Alerts the user of invalid input
        /// 
        /// Be sure to catch WebException indicating if server cannot be contacted
        /// </summary>
        /// <returns>An ArticleStruct with relevant information if successful; otherwise null</returns>
        private ArticleStruct CreateSitecoreArticleItem()
        {
            var articleDetails = articleDetailsPageSelector.GetArticleDetails();

            string title = articleDetails.Title.TrimEnd();

            if (SitecoreClient.DoesArticleNameAlreadyExistInIssue(articleDetails))
            {
                MessageBox.Show
                    (@"This article title is already taken for this issue. Please choose another title or another issue.",
                     @"Informa",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Exclamation);
                return null;
            }

            string webPublishDate = articleDetails.WebPublicationDate.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            Guid pubGuid = articleDetails.Publication;

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show(@"Please enter an article title.", @"Informa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            if (articleDetailsPageSelector.GetPublicationGuid().Equals(Guid.Empty))
            {
                MessageBox.Show(@"Please select a publication.", @"Informa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            SuspendLayout();

            ArticleStruct astruct = _sitecoreArticle.SaveStubToSitecore(title, webPublishDate, pubGuid);

            if (string.IsNullOrEmpty(astruct.RemoteErrorMessage) == false)
            {
                Globals.SitecoreAddin.Log("SaveStubToSitecore returned astruct object with StatusCode/Error: " + astruct.RemoteErrorMessage);
                articleDetails.RemoteErrorMessage = astruct.RemoteErrorMessage;
            }
            else
            {
                articleDetails.ArticleNumber = astruct.ArticleNumber;
                articleDetails.ArticleGuid = astruct.ArticleGuid;
                SitecoreClient.SaveArticleDetailsByGuid(astruct.ArticleGuid, _structConverter.GetServerStruct(articleDetails));
            }
            ResumeLayout();

            return articleDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveArticleToSitecoreUpdateUI(ArticleDocumentMetadataParser metadataParser, string body = null)
        {
            _documentCustomProperties.ArticleNumber = GetArticleNumber();
            InvalidStylesHighlighter highlighter = InvalidStylesHighlighter.GetParser();
            bool hasInvalidStyles = highlighter.HighlightAllInvalidStyles(Globals.SitecoreAddin.Application.ActiveDocument);
            if (hasInvalidStyles && !AskContinueInvalidStyle())
            {
                return;
            }
            if (!articleDetailsPageSelector.pageWorkflowControl.uxUnlockOnSave.Checked)
            {
                var saved = SaveArticle(metadataParser, body);

                if (!saved)
                {
                    return;
                }
            }
            else
            {
                var saved = articleDetailsPageSelector.pageArticleInformationControl.CheckIn();
                if (!saved)
                {
                    return;
                }
            }

            DocumentPropertyEditor.WritePublicationAndDate(SitecoreAddin.ActiveDocument, articleDetailsPageSelector.GetPublicationName(), articleDetailsPageSelector.GetProperDate());

            articleDetailsPageSelector.pageWorkflowControl.UpdateFields(ArticleDetails.ArticleGuid != Guid.Empty
                                            ? SitecoreClient.GetWorkflowState(ArticleDetails.ArticleGuid)
                                            : SitecoreClient.GetWorkflowState(ArticleDetails.ArticleNumber), ArticleDetails);

            articleDetailsPageSelector.pageRelatedArticlesControl.PushSitecoreChanges();
            UpdateFieldsAfterSave();
            articleDetailsPageSelector.ResetChangedStatus(true);
            MessageBox.Show(@"Article successfully saved to Sitecore!", @"Informa");
            return;
        }

        private void UpdateFieldsAfterSave()
        {
            if (ArticleDetails.ArticleGuid == Guid.Empty)
            {
                articleDetailsPageSelector.UpdateFields();
                articleStatusBar1.UpdateFields();
            }
            else
            {
                articleDetailsPageSelector.UpdateFields(ArticleDetails.ArticleGuid);
                articleStatusBar1.UpdateFields(ArticleDetails.ArticleGuid);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadataParser"></param>
        /// <returns>True if user wishes to break</returns>
        private bool PreSavePrompts(ArticleDocumentMetadataParser metadataParser)
        {
            if (string.IsNullOrEmpty(metadataParser.Title))
            {
                MessageBox.Show(@"Please enter an article title.", @"Informa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }

            if (NotifyTitleTooLong(metadataParser.Title.Trim().Length))
            {
                return true;
            }

            if (metadataParser.TitleCount > 1)
            {
                if (!AskContinueMultipleTitles())
                {
                    return true;
                }
            }
            int maxLengthLongSummary = SitecoreClient.GetMaxLengthLongSummary();
            if (metadataParser.ExecutiveSummary.Length > maxLengthLongSummary)
            {
                if (!AskExceededCharacterLimit("Summary", maxLengthLongSummary))
                {
                    return true;
                }
            }

            return false;
        }

        #region ActionListeners

        private void uxCreateArticle_Click(object sender, EventArgs e)
        {
            SuspendLayout();
            Cursor = Cursors.WaitCursor;
            string body;
            try
            {
                XElement xbody = _wordUtils.GetWordDocTextWithStyles(SitecoreAddin.ActiveDocument);
                body = xbody.ToString();
            }
            catch (InsecureIFrameException insecureIframe)
            {
                string message = String.Empty;
                foreach (string iframeURL in insecureIframe.InsecureIframes)
                {
                    message += String.Format("\n{0}", iframeURL);
                }

                MessageBox.Show("The following multimedia content is not secure. Please correct and try to save again. " + message, "Non-secure Multimedia Content");
                return;
            }
            catch (InvalidHtmlException)
            {
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"The document could not be parsed to transfer to Sitecore! Details in logs.", @"Informa");
                Globals.SitecoreAddin.LogException("Error when parsing article on creation!", ex);
                return;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var metadataParser = new ArticleDocumentMetadataParser(SitecoreAddin.ActiveDocument, _wordUtils.CharacterStyleTransformer);
                if (PreSavePrompts(metadataParser))
                    return;

                ArticleDetails = CreateSitecoreArticleItem();
                if (ArticleDetails != null)
                {
                    if (string.IsNullOrEmpty(ArticleDetails.ArticleNumber))
                    {
                        if (ArticleDetails.RemoteErrorMessage == HttpStatusCode.Unauthorized.ToString())
                        {
                            MessageBox.Show(Constants.SESSIONTIMEOUTERRORMESSAGE);
                            return;
                        }
                        else
                        {
                            MessageBox.Show(@"The article number generator is busy! Please try again later.", @"Informa");
                        }
                    }
                    else
                    {
                        _documentCustomProperties.ArticleNumber = ArticleDetails.ArticleNumber;
                        articleDetailsPageSelector.CheckOut();
                        _documentCustomProperties.ArticleNumber = GetArticleNumber();
                        articleStatusBar1.DisplayStatusBar(true, _documentCustomProperties.ArticleNumber);
                        DocumentPropertyEditor.WritePublicationAndDate(
                        SitecoreAddin.ActiveDocument, articleDetailsPageSelector.GetPublicationName(), articleDetailsPageSelector.GetProperDate());
                        PostLinkEnable();
                        SaveArticleToSitecoreUpdateUI(metadataParser, body);
                    }
                }
            }
            catch (WebException wex)
            {
                AlertConnectionFailure();
                Globals.SitecoreAddin.LogException("Web connection error when creating article!", wex);
            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.LogException("Error when creating article!", ex);
                MessageBox.Show(@"Error when creating article! Error recorded in logs.", @"Informa");
            }
            finally
            {
                ResumeLayout();
                Cursor = Cursors.Default;
            }
        }

        private DialogResult WantsToSetArticleDateToNow(ArticleWorkflowCommand command)
        {
            if (command != null && command.SendsToFinal)
            {
                const string messageBoxText = "Your Scheduled Article Publish Date is in the past, update to the current date and time?";
                const MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                return MessageBox.Show(messageBoxText, "Update publish date?", buttons, MessageBoxIcon.Question);
            }
            return DialogResult.No;
        }

        private void uxSaveMetadata_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var command = articleDetailsPageSelector.pageWorkflowControl.GetSelectedCommandState();

            // Checking for Taxonomy is the workflow state is final
            if (command.SendsToFinal && articleDetailsPageSelector.GetTaxonomyCount() < 1)
            {
                MessageBox.Show(@"Select at least one taxonomy item for the article!", @"Informa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                if (articleDetailsPageSelector.pageWorkflowControl.uxUnlockOnSave.Enabled)
                {
                    workflowChange_UnlockOnSave = articleDetailsPageSelector.pageWorkflowControl.uxUnlockOnSave.Checked;
                }

                var articleDate = articleDetailsPageSelector.GetDate();

                //var timeUtc = DateTime.UtcNow;
                //TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                DateTime currentTime = DateTime.Now;// TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

                if (articleDate < currentTime)
                {

                    var result = WantsToSetArticleDateToNow(command);
                    if (result == DialogResult.Yes)
                    {
                        articleDetailsPageSelector.SetDate(DateTime.Now, true);
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        MessageBox.Show("Save cancelled.");
                        return;
                    }
                }

                SuspendLayout();

                var isPublished = ArticleDetails.IsPublished;
                Guid guidCopy = ArticleDetails.ArticleGuid;
                ArticleDetails = articleDetailsPageSelector.GetArticleDetailsWithoutDocumentParsing();
                ArticleDetails.ArticleGuid = guidCopy;
                ArticleDetails.IsPublished = isPublished;
                ArticleDetails.IsPublished = ArticleDetails.IsPublished;
                ArticleDetails.ArticleSpecificNotifications = articleDetailsPageSelector.pageWorkflowControl.GetNotifyList();

                ArticleDetails.WordCount = SitecoreAddin.ActiveDocument.ComputeStatistics(0);
                ArticleDetails.CommandID = articleDetailsPageSelector.pageWorkflowControl.GetSelectedCommand();
                ArticleDetails.NotificationText = articleDetailsPageSelector.pageWorkflowControl.GetNotificationText();

                var lockStateBeforeSaveMetaData = SitecoreClient.GetLockedStatus(ArticleDetails.ArticleGuid);
                SitecoreClient.SaveMetadataToSitecore(ArticleDetails.ArticleNumber, _structConverter.GetServerStruct(ArticleDetails));
                //I know the following checks are weird, but issue IIPP-1031 occurs sometimes only on UAT env. So had to hack around it.
                //On UAT after SaveMetadataToSitecore, the locked status becomes false for no apparent reason.
                var lockStateAfterSaveMetaData = SitecoreClient.GetLockedStatus(ArticleDetails.ArticleGuid);
                if (lockStateBeforeSaveMetaData.Locked && lockStateAfterSaveMetaData.Locked == false && workflowChange_UnlockOnSave == false)
                {
                    SitecoreClient.CheckOutArticle(ArticleDetails.ArticleNumber, SitecoreUser.GetUser().Username);
                }

                if (articleDetailsPageSelector.pageWorkflowControl.uxUnlockOnSave.Checked)
                {
                    articleDetailsPageSelector.pageArticleInformationControl.CheckIn(false);
                }
                articleDetailsPageSelector.pageWorkflowControl.UpdateFields(ArticleDetails.ArticleGuid != Guid.Empty
                                                ? SitecoreClient.GetWorkflowState(ArticleDetails.ArticleGuid)
                                                : SitecoreClient.GetWorkflowState(ArticleDetails.ArticleNumber), ArticleDetails);
                articleDetailsPageSelector.pageRelatedArticlesControl.PushSitecoreChanges();
                articleDetailsPageSelector.UpdateFields();
                articleDetailsPageSelector.ResetChangedStatus(true);

                UpdateFieldsAfterSave();
                DocumentPropertyEditor.WritePublicationAndDate(SitecoreAddin.ActiveDocument, articleDetailsPageSelector.GetPublicationName(), articleDetailsPageSelector.GetProperDate());

                //TamerM - 2016-03-22: Prompt and ReExport  NLM FEED
                NLMFeedUtils.PromptAndReExportNLMFeed(ArticleDetails.ArticleNumber, ArticleDetails.IsPublished);

                if (workflowChange_UnlockOnSave)
                {
                    EnablePreview();
                    uxCreateArticle.Visible = false;
                }
                articleStatusBar1.RefreshWorkflowDetails();

                MessageBox.Show(@"Metadata saved!", @"Informa");
            }
            catch (WebException wex)
            {
                AlertConnectionFailure();
                Globals.SitecoreAddin.LogException("Web connection error when saving metadata!", wex);
            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.LogException("Error when saving meta data!", ex);
                MessageBox.Show(@"Error when saving metadata! Error recorded in logs.", @"Informa");
            }
            finally
            {
                ResumeLayout();
                Cursor = Cursors.Default;
                workflowChange_UnlockOnSave = false;
            }

            Document activeDocument = SitecoreAddin.ActiveDocument;
            var path = activeDocument.Path;
            if (!activeDocument.ReadOnly && !string.IsNullOrWhiteSpace(path))
            {
                WordUtils.Save(activeDocument);
            }
        }

        private bool workflowChange_UnlockOnSave;
        private void uxSaveAndTransfer_Click(object sender, EventArgs e)
        {
            var command = articleDetailsPageSelector.pageWorkflowControl.GetSelectedCommandState();

            // Checking for Taxonomy is the workflow state is final
            if (command.SendsToFinal && articleDetailsPageSelector.GetTaxonomyCount() < 1)
            {
                MessageBox.Show(@"Select at least one taxonomy item for the article!", @"Informa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                Cursor = Cursors.WaitCursor;
                if (articleDetailsPageSelector.pageWorkflowControl.uxUnlockOnSave.Enabled)
                    workflowChange_UnlockOnSave = articleDetailsPageSelector.pageWorkflowControl.uxUnlockOnSave.Checked;

                var articleDate = articleDetailsPageSelector.GetDate();
                //var timeUtc = DateTime.UtcNow;
                //TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                DateTime currentTime = DateTime.Now;// TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

                if (articleDate < currentTime)
                {
                    var result = WantsToSetArticleDateToNow(command);
                    if (result == DialogResult.Yes)
                    {
                        articleDetailsPageSelector.SetDate(DateTime.Now, true);
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        MessageBox.Show("Save cancelled.");
                        return;
                    }
                }

                Globals.SitecoreAddin.Log("Save and transferring");

                SuspendLayout();

                SitecoreAddin.ActiveDocument.Saved = false;




                var metadataParser = new ArticleDocumentMetadataParser(SitecoreAddin.ActiveDocument, _wordUtils.CharacterStyleTransformer);
                if (PreSavePrompts(metadataParser)) return;
                SaveArticleToSitecoreUpdateUI(metadataParser);
                articleStatusBar1.RefreshWorkflowDetails();
            }
            catch (WebException wex)
            {
                AlertConnectionFailure();
                Globals.SitecoreAddin.LogException("Web connection error when saving and transferring article!", wex);
            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.LogException("Error when saving and transferring article!", ex);
                MessageBox.Show(@"Error when saving and transferring article! Error recorded in logs.", @"Informa");
            }
            finally
            {
                ResumeLayout();
                Cursor = Cursors.Default;
                workflowChange_UnlockOnSave = false;
            }
            Document activeDocument = SitecoreAddin.ActiveDocument;
            var path = activeDocument.Path;
            if (!activeDocument.ReadOnly && !string.IsNullOrWhiteSpace(path))
            {
                WordUtils.Save(activeDocument);
            }
        }


        private void uxPreview_Click(object sender, EventArgs e)
        {
            if (GetArticleNumber() == null)
            {
                MessageBox.Show(@"There is no article linked!", @"Informa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Process.Start(GetPreviewUrl(false));
        }


        private void uxMobilePreview_Click(object sender, EventArgs e)
        {
            if (GetArticleNumber() == null)
            {
                MessageBox.Show(@"There is no article linked!", @"Informa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Process.Start(GetPreviewUrl(true));
        }

        private string GetPreviewUrl(bool isMobile)
        {
            string guid = SitecoreClient.GetArticleGuidByArticleNumber(GetArticleNumber());
            string domain = Constants.EDITOR_ENVIRONMENT_SERVERURL;
            string mobileUrlParam = isMobile ? "&mobile=1" : String.Empty;
            string redirect = (domain + @"?sc_itemid={" + guid + @"}&sc_mode=preview&sc_lang=en" + mobileUrlParam);
            return redirect;
        }

        public System.Version AssemblyVersion => ApplicationDeployment.CurrentDeployment.CurrentVersion;

        private void ArticleDetail_Load(object sender, EventArgs e)
        {
            string version = $"{AssemblyVersion.Major}.{AssemblyVersion.Minor}.{AssemblyVersion.Build}.{AssemblyVersion.Revision}";
            uxVersionNumber.Text = version;
        }
        #endregion

        public bool CheckOutOfDate(bool prompt = true)
        {
            int localWordDocVersion = _documentCustomProperties.WordSitecoreVersionNumber;
            int sitecoreWordDocVersion = this.ArticleDetails.WordDocVersionNumber;
            if ((sitecoreWordDocVersion <= localWordDocVersion)) return true; //we're out of date!
            if (prompt)
            {
                string message =
                    "A newer version of this article  was uploaded on {0} by {1}. " +
                    "If you continue using this version, you may overwrite more recent " +
                    "changes. To get the most recent version, navigate to Sitecore to download the new document.";

                MessageBox.Show
                    (String.Format(message, this.ArticleDetails.WordDocLastUpdateDate, this.ArticleDetails.WordDocLastUpdatedBy),
                        @"Informa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
            }
            return false;
        }
    }
}
