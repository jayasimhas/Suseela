using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;
using Informa.Web.Areas.Account.Models;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using SitecoreTreeWalker.Config;
using SitecoreTreeWalker.Custom_Exceptions;
using SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls;
using SitecoreTreeWalker.document;
using SitecoreTreeWalker.Sitecore;
using SitecoreTreeWalker.User;
using SitecoreTreeWalker.Util;
using SitecoreTreeWalker.Util.Document;
using SitecoreTreeWalker.WebserviceHelper;
using Application = Microsoft.Office.Interop.Word.Application;
using ArticleStruct = Informa.Web.Areas.Account.Models.WordPluginModel.ArticleStruct;

namespace SitecoreTreeWalker.UI.ArticleDetailsForm
{
    public partial class ArticleDetail : Form
    {
        #region Class Members
        //null if no sitecore article item is associated
        public ArticleStruct ArticleDetails = new ArticleStruct();
        public bool CloseOnSuccessfulLock;

        private DocumentCustomProperties _documentCustomProperties;
        private readonly SitecoreArticle _sitecoreArticle;
        private WordUtils _wordUtils;
        protected StructConverter _structConverter;

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
            DisablePreview();
            articleDetailsPageSelector.PreLinkEnable();
            articleDetailsPageSelector.pageWorkflowControl.PreLinkEnable();
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
            uxPreview.Enabled = true;

            uxMobilePreview.Text = @"Preview Mobile Article";
            uxMobilePreview.Enabled = true;
        }

        public void DisablePreview()
        {
            uxPreview.Text = @"Article Not Linked";
            uxPreview.Enabled = false;

            uxMobilePreview.Text = @"Mobile Article Not Linked";
            uxMobilePreview.Enabled = false;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ArticleDetail()
        {
            SitecoreAddin.TagActiveDocument();

            _sitecoreArticle = new SitecoreArticle();
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
                articleStatusBar1.ChangeLockButtonStatus(false);
            }
            else
            {
                articleStatusBar1.ChangeLockButtonStatus(true);
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
                SetArticleDetails(SitecoreGetter.LazyReadArticleDetails(GetArticleNumber()));
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


                if (!SitecoreGetter.IsAvailable())
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
            articleDetailsPageSelector.pageWorkflowControl.UpdateFields(ArticleDetails.WorkflowState);
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
                    var copy = ArticleDetails.ArticleGuid;
                    ArticleDetails = articleDetailsPageSelector.GetArticleDetails(metadataParser);
                    ArticleDetails.ArticleGuid = copy;
					List<string> errors = _sitecoreArticle.SaveArticle(SitecoreAddin.ActiveDocument, ArticleDetails, new Guid(), new WordPluginModel.StaffStruct[0], GetArticleNumber(), body);
					//TODO - Add workflow commands and Notification List
					//List<string> errors = _sitecoreArticle.SaveArticle(SitecoreAddin.ActiveDocument,ArticleDetails,articleDetailsPageSelector.pageWorkflowControl.GetSelectedCommand(),articleDetailsPageSelector.pageWorkflowControl.GetNotifyList().ToArray(),GetArticleNumber(), body);

                    if (errors != null && errors.Any())
                    {
                        foreach (string error in errors)
                        {
                            if (!String.IsNullOrEmpty(error))
                            {
                                MessageBox.Show(error, error.Contains("not secure") ? @" Non-Secure Multimedia Content" : @"Elsevier");
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

            return true;
        }

        protected void UpdateFieldsUsingSitecore()
        {
            if (!articleDetailsPageSelector.CheckOut(false))
            {
                if (!string.IsNullOrEmpty(ArticleDetails.ArticleNumber))
                {
                    Globals.SitecoreAddin.Log(String.Format("Article with # [{0}] not found!", ArticleDetails.ArticleNumber));
                    MessageBox.Show(this, @"Unable to retrieve article information from Sitecore. " +
                                          @"Please verify that this article exists on Sitecore and " +
                                          @"try again or contact support if you get this message again.", @"Elsevier");
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
                                @"Elsevier",
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
                                @"Elsevier",
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
                 @"Elsevier",
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
                    @". Current title is " + titleLength + @" characters long!", @"Elsevier");
                return true;
            }
            return false;
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if the user is trying to nominate without any primary industries associated</returns>
        protected bool PromptAddIndustryToNominate()
        {
            if (articleDetailsPageSelector.TryingToNominateWithNoIndustries())
            {
                MessageBox.Show(this, @"Please add at least 1 industry in order to nominate this article for the homepage.",
                                @"Elsevier");
                return true;
            }
            if (articleDetailsPageSelector.TryingToNominateWithNoPrimaryIndustries())
            {
                MessageBox.Show(this, @"The industries you have added do not have an associated primary industry. You must have an " +
                                      @"industry taxonomy that has a primary industry.",
                                @"Elsevier");
                return true;
            }
            return false;
        }*/

        public DialogResult AlertConnectionFailure()
        {
            return MessageBox.Show
                (@"Sitecore server could not be contacted! Please try again in a few minutes." + Environment.NewLine +
                 Environment.NewLine + @"If the problem persists, contact your system administrator.",
                 @"Elsevier",
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

            if (SitecoreArticle.DoesArticleNameAlreadyExistInIssue(articleDetails))
            {
                MessageBox.Show
                    (@"This article title is already taken for this issue. Please choose another title or another issue.",
                     @"Elsevier",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Exclamation);
                return null;
            }

            string webPublishDate = articleDetails.WebPublicationDate.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            Guid pubGuid = articleDetails.Publication;

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show(@"Please enter an article title.", @"Elsevier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            if (pubGuid.Equals(Guid.Empty))
            {
                MessageBox.Show(@"Please select a publication.", @"Elsevier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            
            SuspendLayout();

			WordPluginModel.ArticleStruct astruct = _sitecoreArticle.SaveStubToSitecore(title, webPublishDate, pubGuid);

            //articleDetailsPageSelector.UpdateArticleNumber(astruct.ArticleNumber);
            articleDetails.ArticleNumber = astruct.ArticleNumber;
            articleDetails.ArticleGuid = astruct.ArticleGuid;
            SitecoreArticle.SaveArticleDetailsByGuid(astruct.ArticleGuid, _structConverter.GetServerStruct(articleDetails));
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


            DocumentPropertyEditor.WritePublicationAndDate(
                SitecoreAddin.ActiveDocument, articleDetailsPageSelector.GetPublicationName(), articleDetailsPageSelector.GetProperDate());

            articleDetailsPageSelector.pageWorkflowControl.UpdateFields(ArticleDetails.ArticleGuid != Guid.Empty
                                            ? SitecoreArticle.GetWorkflowState(ArticleDetails.ArticleGuid)
                                            : SitecoreArticle.GetWorkflowState(ArticleDetails.ArticleNumber));
            articleDetailsPageSelector.pageRelatedArticlesControl.PushSitecoreChanges();
            UpdateFieldsAfterSave();
            articleDetailsPageSelector.ResetChangedStatus(true);
            MessageBox.Show(@"Article successfully saved to Sitecore!", @"Elsevier");
            return;
        }

        private void UpdateFieldsAfterSave()
        {
            if (ArticleDetails.ArticleGuid == Guid.Empty)
            {
                articleDetailsPageSelector.UpdateFields();
            }
            else
            {
                articleDetailsPageSelector.UpdateFields(ArticleDetails.ArticleGuid);
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
                MessageBox.Show(@"Please enter an article title.", @"Elsevier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            int maxLengthLongSummary = SitecoreGetter.GetMaxLengthLongSummary();
            if (metadataParser.LongSummary.Length > maxLengthLongSummary)
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
                Cursor = Cursors.Arrow;
                return;
            }
            catch (InvalidHtmlException)
            {
                Cursor = Cursors.Arrow;
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"The document could not be parsed to transfer to Sitecore! Details in logs.", @"Elsevier");
                Globals.SitecoreAddin.LogException("Error when parsing article on creation!", ex);
                return;
            }
            try
            {
                var metadataParser = new ArticleDocumentMetadataParser(SitecoreAddin.ActiveDocument,
                                                                       _wordUtils.CharacterStyleTransformer);
                if (PreSavePrompts(metadataParser)) return;
                ArticleDetails = CreateSitecoreArticleItem();
                if (ArticleDetails != null)
                {
                    if (string.IsNullOrEmpty(ArticleDetails.ArticleNumber))
                    {
                        MessageBox.Show(@"The article number generator is busy! Please try again later.", @"Elsevier");
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
                MessageBox.Show(@"Error when creating article! Error recorded in logs.", @"Elsevier");
            }
            finally
            {
                ResumeLayout();
                Cursor = Cursors.Default;
            }
        }

        private DialogResult WantsToSetArticleDateToNow(WordPluginModel.WorkflowCommand command)
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
            var articleDate = articleDetailsPageSelector.GetDate();
            if (articleDate < DateTime.Now)
            {
                var command = articleDetailsPageSelector.pageWorkflowControl.GetSelectedCommandState();
                var result = WantsToSetArticleDateToNow(command);
                if (result == DialogResult.Yes)
                {
                    articleDetailsPageSelector.SetDate(DateTime.Now);
                }
                else if (result == DialogResult.Cancel)
                {
                    MessageBox.Show("Save cancelled.");
                    return;
                }
            }

            SuspendLayout();
            Cursor = Cursors.WaitCursor;
            try
            {
                /*
                if (PromptAddIndustryToNominate())
                {
                    return;
                }*/
                Guid guidCopy = ArticleDetails.ArticleGuid;
                ArticleDetails = articleDetailsPageSelector.GetArticleDetailsWithoutDocumentParsing();
                ArticleDetails.ArticleGuid = guidCopy;
                ArticleDetails.ArticleSpecificNotifications = articleDetailsPageSelector.pageWorkflowControl.GetNotifyList().ToList();

                ArticleDetails.WordCount = SitecoreAddin.ActiveDocument.ComputeStatistics(0);
                ArticleDetails.CommandID = articleDetailsPageSelector.pageWorkflowControl.GetSelectedCommand();
                SitecoreArticle.SaveMetadataToSitecore(ArticleDetails.ArticleNumber, _structConverter.GetServerStruct(ArticleDetails));
                if (articleDetailsPageSelector.pageWorkflowControl.uxUnlockOnSave.Checked)
                {
                    articleDetailsPageSelector.pageArticleInformationControl.CheckIn(false);
                }
                articleDetailsPageSelector.pageWorkflowControl.UpdateFields(ArticleDetails.ArticleGuid != Guid.Empty
                                                ? SitecoreArticle.GetWorkflowState(ArticleDetails.ArticleGuid)
                                                : SitecoreArticle.GetWorkflowState(ArticleDetails.ArticleNumber));
                articleDetailsPageSelector.pageRelatedArticlesControl.PushSitecoreChanges();
                articleDetailsPageSelector.UpdateFields();
                articleDetailsPageSelector.ResetChangedStatus();
                DocumentPropertyEditor.WritePublicationAndDate(
                    SitecoreAddin.ActiveDocument, articleDetailsPageSelector.GetPublicationName(), articleDetailsPageSelector.GetProperDate());
                MessageBox.Show(@"Metadata saved!", @"Elsevier");
            }
            catch (WebException wex)
            {
                AlertConnectionFailure();
                Globals.SitecoreAddin.LogException("Web connection error when saving metadata!", wex);
            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.LogException("Error when saving meta data!", ex);
                MessageBox.Show(@"Error when saving metadata! Error recorded in logs.", @"Elsevier");
            }
            finally
            {
                ResumeLayout();
                Cursor = Cursors.Default;
            }

            Document activeDocument = SitecoreAddin.ActiveDocument;
            var path = activeDocument.Path;
            if (!activeDocument.ReadOnly && !string.IsNullOrWhiteSpace(path))
            {
                WordUtils.Save(activeDocument);
            }
        }

        private void uxSaveAndTransfer_Click(object sender, EventArgs e)
        {
            var articleDate = articleDetailsPageSelector.GetDate();
            if (articleDate < DateTime.Now)
            {
                var command = articleDetailsPageSelector.pageWorkflowControl.GetSelectedCommandState();
                var result = WantsToSetArticleDateToNow(command);
                if (result == DialogResult.Yes)
                {
                    articleDetailsPageSelector.SetDate(DateTime.Now);
                }
                else if (result == DialogResult.Cancel)
                {
                    MessageBox.Show("Save cancelled.");
                    return;
                }
            }

            Globals.SitecoreAddin.Log("Save and transferring");
            Cursor = Cursors.WaitCursor;
            SuspendLayout();

            SitecoreAddin.ActiveDocument.Saved = false;

            try
            {
                var metadataParser = new ArticleDocumentMetadataParser(SitecoreAddin.ActiveDocument,
                                                                       _wordUtils.CharacterStyleTransformer);
                if (PreSavePrompts(metadataParser)) return;
                SaveArticleToSitecoreUpdateUI(metadataParser);
            }
            catch (WebException wex)
            {
                AlertConnectionFailure();
                Globals.SitecoreAddin.LogException("Web connection error when saving and transferring article!", wex);
            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.LogException("Error when saving and transferring article!", ex);
                MessageBox.Show(@"Error when saving and transferring article! Error recorded in logs.", @"Elsevier");
            }
            finally
            {
                ResumeLayout();
                Cursor = Cursors.Default;
            }
            Document activeDocument = SitecoreAddin.ActiveDocument;
            var path = activeDocument.Path;
            if (!activeDocument.ReadOnly && !string.IsNullOrWhiteSpace(path))
            {
                WordUtils.Save(activeDocument);
            }
        }

        //TODO:Remove these once logout fully implemented
        /*
        private void uxLogout_Click(object sender, EventArgs e)
        {
            try
            {
                loginControl1.Logout();
                Globals.SitecoreAddin.CloseSitecoreTreeBrowser(Globals.SitecoreAddin.Application.ActiveDocument);
            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.LogException("Error while logging out!", ex);
                throw;
            }
        }
         * */

        private void uxPreview_Click(object sender, EventArgs e)
        {
            if (GetArticleNumber() == null)
            {
                MessageBox.Show(@"There is no article linked!", @"Elsevier", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //string guid = SitecoreArticle.GetArticleGuidByArticleNumber(GetArticleNumber());
            //string domain = ApplicationConfig.GetPropertyValue("DomainName");
            //string redirect = Uri.EscapeDataString(domain + @"?sc_itemid={" + guid + @"}&sc_mode=preview&sc_lang=en");
            //string url = domain + @"Util/LoginRedirectToPreview.aspx?redirect=" + redirect;

            //var p = new Process {StartInfo = {FileName = url}};
            Process.Start(GetPreviewUrl(false));
        }


        private void uxMobilePreview_Click(object sender, EventArgs e)
        {
            if (GetArticleNumber() == null)
            {
                MessageBox.Show(@"There is no article linked!", @"Elsevier", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //var p = new Process {StartInfo = {FileName = url}};
            Process.Start(GetPreviewUrl(true));
        }

        private string GetPreviewUrl(bool isMobile)
        {
            string guid = SitecoreArticle.GetArticleGuidByArticleNumber(GetArticleNumber());
            string domain = ApplicationConfig.GetPropertyValue("DomainName");
            string mobileUrlParam = isMobile ? "&mobile=1" : String.Empty;
            string redirect = Uri.EscapeDataString(domain + @"?sc_itemid={" + guid + @"}&sc_mode=preview&sc_lang=en" + mobileUrlParam);
            return domain + @"Util/LoginRedirectToPreview.aspx?redirect=" + redirect;

        }

        private void ArticleDetail_Load(object sender, EventArgs e)
        {
			
            //if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                uxVersionNumber.Text = System.Windows.Forms.Application.ProductVersion;
            }			
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
                        @"Elsevier",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
            }                
            return false;
        }
    }
}
