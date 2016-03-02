using PluginModels;
using SitecoreTreeWalker.Config;
using SitecoreTreeWalker.Sitecore;
using SitecoreTreeWalker.UI.ArticleDetailsForm;
using SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SitecoreTreeWalker.Util.Document
{
    /// <summary>
    /// This class is responsible for actions when validating article documents before saving or creating
    /// </summary>
    /// <author>
    /// TamerM - 2015-02-28
    /// </author>
    public class ArticleDocumentValidator
    {
        private const string MESSAGEBOX_TITLE = "Informa";
        ArticleDocumentMetadataParser _metadataParser;
        Microsoft.Office.Interop.Word.Document _document;
        public ArticleDocumentValidator(Microsoft.Office.Interop.Word.Document wordDoc, ArticleDocumentMetadataParser metadataParser)
        {
            _metadataParser = metadataParser;
            _document = wordDoc;
        }

        public bool IsValidArticleDocument()
        {
            if (string.IsNullOrEmpty(_metadataParser.Title))
            {
                MessageBox.Show(@"Please enter an article title.", ArticleDocumentValidator.MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (IsTitleTooLong_WithNotification(_metadataParser.Title.Trim().Length))
            {
                return false;
            }

            if (_metadataParser.TitleCount > 1)
            {
                if (AllowMultipleTitles() == false)
                {
                    return false;
                }
            }

            int maxLengthLongSummary = SitecoreClient.GetMaxLengthLongSummary();
            if (_metadataParser.LongSummary.Length > maxLengthLongSummary)
            {
                if (AllowTruncatingExceededCharacterLimit("Summary", maxLengthLongSummary) == false)
                {
                    return false;
                }
            }

            InvalidStylesHighlighter highlighter = InvalidStylesHighlighter.GetParser();
            //bool hasInvalidStyles = highlighter.HighlightAllInvalidStyles(Globals.SitecoreAddin.Application.ActiveDocument);
            bool hasInvalidStyles = highlighter.HighlightAllInvalidStyles(_document);
            if (hasInvalidStyles && AllowInvalidStyle() == false)
            {
                return false;
            }

            return true;
        }

        public bool IsValidArticleDate(ArticleStruct articleDetails, WorkflowCommand command)
        {
            var articleDate = articleDetails.WebPublicationDate;
            if (articleDate < DateTime.Now)
            {
                var result = WantsToSetArticleDateToNow(command);
                if (result == DialogResult.Yes)
                {
                    articleDetails.WebPublicationDate = DateTime.Now;
                    return true;
                }
                else if (result == DialogResult.Cancel)
                {
                    MessageBox.Show("Save cancelled.");
                    return false;
                }
            }

            return true;
        }

        private bool AllowInvalidStyle()
        {
            DialogResult dialogResult = MessageBox.Show
            (@"Unmapped styles have been identified in your document. " +
             @"If you wish to proceed with saving, this text will be " +
             @"transferred with the default story text style. Do you " +
             @"wish to proceed?",
             Constants.MESSAGEBOX_TITLE,
             MessageBoxButtons.YesNo,
             MessageBoxIcon.Question);

            return (dialogResult == DialogResult.Yes);
        }

        private bool AllowTruncatingExceededCharacterLimit(string field, int characterLimit)
        {
            string message =
                String.Format("You have exceeded the character limit of {0} characters for the {1}. " +
                              "The {1} will be truncated. Do you wish to continue?",
                              characterLimit, field);
            DialogResult dialogResult = MessageBox.Show(message, Constants.MESSAGEBOX_TITLE,
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);
            return (dialogResult == DialogResult.Yes);
        }

        private bool AllowMultipleTitles()
        {
            DialogResult dialogResult = MessageBox.Show
                            (@"There are multiple paragraphs that are styled as Story Title. " +
                                @"Do you wish to proceed?",
                                Constants.MESSAGEBOX_TITLE,
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);
            return (dialogResult == DialogResult.Yes);
        }

        private bool IsTitleTooLong_WithNotification(int titleLength)
        {
            int maxLength = ApplicationConfig.GetTitleMaxCharacters();
            if (titleLength > maxLength)
            {
                MessageBox.Show(string.Format("Title is too long! Title character limit is {0}. Current title is {1} characters long!", maxLength, titleLength), Constants.MESSAGEBOX_TITLE);
                return true;
            }
            return false;
        }

        private DialogResult WantsToSetArticleDateToNow(WorkflowCommand command)
        {
            if (command != null && command.SendsToFinal)
            {
                const string messageBoxText = "Your Scheduled Article Publish Date is in the past, update to the current date and time?";
                const MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                return MessageBox.Show(messageBoxText, "Update publish date?", buttons, MessageBoxIcon.Question);
            }
            return DialogResult.No;
        }
    }
}
