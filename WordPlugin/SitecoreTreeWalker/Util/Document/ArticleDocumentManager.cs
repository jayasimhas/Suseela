using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using SitecoreTreeWalker.document;
using PluginModels;
using SitecoreTreeWalker.Sitecore;
using System.Windows.Forms;
using SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls;
using SitecoreTreeWalker.Config;
using System.Diagnostics;
using SitecoreTreeWalker.WebserviceHelper;
using System.Net;

namespace SitecoreTreeWalker.Util.Document
{
    /// <summary>
    /// This class is responsible for actions when creating and saving article documents
    /// </summary>
    /// <author>
    /// TamerM - 2015-02-28
    /// </author>
    public class ArticleDocumentManager
    {
        ArticleStruct _articleDetails;
        DocumentCustomProperties _documentProperties;
        WordUtils _wordUtils;

        private string articleNumber { get { return _articleDetails?.ArticleNumber; } }
        private Guid articleGuid { get { return _articleDetails == null ? Guid.Empty : _articleDetails.ArticleGuid; } }

        public ArticleDocumentManager()
        {
            _wordUtils = new WordUtils();
        }

        private void reloadDocument()
        {
            //SitecoreAddin.TagActiveDocument();
            _documentProperties = new DocumentCustomProperties(SitecoreAddin.ActiveDocument);
            if (_articleDetails == null)
                _articleDetails = new ArticleStruct();

            _articleDetails.ArticleNumber = _documentProperties.ArticleNumber;
        }

        private bool IsArticleNumberFilled()
        {
            return string.IsNullOrEmpty(_articleDetails.ArticleNumber) == false;
        }

        private bool fillArticleDetails()
        {
            reloadDocument();

            if (_articleDetails != null && _articleDetails.ArticleGuid != Guid.Empty)//If ArticleGuid is filled
                _articleDetails = SitecoreClient.ForceReadArticleDetails(_articleDetails.ArticleGuid);
            else if (string.IsNullOrEmpty(_documentProperties.ArticleNumber) == false)//if article details is completely empty and document Props is not empty and contains ArticleNumber 
                _articleDetails = SitecoreClient.ForceReadArticleDetails(_documentProperties.ArticleNumber);
            else if (_articleDetails != null && string.IsNullOrEmpty(_articleDetails.ArticleNumber) == false)//If article number is filled
                _articleDetails = SitecoreClient.ForceReadArticleDetails(_articleDetails.ArticleNumber);
            else
            {
                return false;
            }

            return true;
        }

        public void CreateDocument()
        {
            throw new NotImplementedException();
        }

        public void SaveDocument()
        {
            fillArticleDetails();

            Globals.SitecoreAddin.Log("Saving Article to Sitecore");

            //Make sure ActiveDocument is not null and that at least either ArticleGuid or ArticleNumber can be read
            if (SitecoreAddin.ActiveDocument == null || (articleGuid == Guid.Empty && string.IsNullOrEmpty(articleNumber)))
            {
                MessageBox.Show("Error reading data. Please close and try again.");
                return;
            }

            SitecoreAddin.ActiveDocument.Saved = false;

            //Prepare metadata Parser
            var metaDataParser = new ArticleDocumentMetadataParser(SitecoreAddin.ActiveDocument, _wordUtils.CharacterStyleTransformer);
            //Initialize Article Document Validation class to validate article data
            ArticleDocumentValidator validator = new ArticleDocumentValidator(SitecoreAddin.ActiveDocument, metaDataParser);

            if (validator.IsValidArticleDocument())
            {
                reloadDocument();

                if (IsArticleNumberFilled())
                {
                    var sitecoreClient = new SitecoreClient();

                    //Save to sitecore and get back a list of errors if occur
                    List<string> errors = sitecoreClient.SaveArticle(SitecoreAddin.ActiveDocument, _articleDetails, new Guid(), new StaffStruct[0], articleNumber, null);

                    if (errors != null && errors.Any())
                    {
                        foreach (string error in errors)
                        {
                            //Provide user with the list of errors that occured
                            if (String.IsNullOrEmpty(error) == false)
                            {
                                MessageBox.Show(error, error.Contains("not secure") ? @" Non-Secure Multimedia Content" : Constants.MESSAGEBOX_TITLE);
                            }
                        }
                    }

                    //Get publication name and date
                    List<ItemStruct> publications = SitecoreClient.GetPublications();
                    ItemStruct selectedPublication = publications.FirstOrDefault(w => w.ID == _articleDetails.Publication);
                    var webPublishDate = _articleDetails.WebPublicationDate;

                    //Write publication name and date to document
                    if (selectedPublication.ID != Guid.Empty && webPublishDate != DateTime.MinValue)
                        DocumentPropertyEditor.WritePublicationAndDate(SitecoreAddin.ActiveDocument, selectedPublication.Name, webPublishDate.ToString());

                    //Save word document
                    if (!SitecoreAddin.ActiveDocument.ReadOnly && !string.IsNullOrWhiteSpace(SitecoreAddin.ActiveDocument.Path))
                    {
                        WordUtils.Save(SitecoreAddin.ActiveDocument);
                    }
                }
            }
        }

        public void SaveMetaData()
        {
            fillArticleDetails();

            //Guid tempArticleGuid = _articleDetails.ArticleGuid;
            //string tempArticleNumber = _articleDetails.ArticleNumber;

            //_articleDetails = articleDetailsPageSelector.GetArticleDetailsWithoutDocumentParsing();
            //_articleDetails.ArticleGuid = tempArticleGuid;
            //_articleDetails.ArticleNumber = tempArticleNumber;
            //_articleDetails.ArticleSpecificNotifications = articleDetailsPageSelector.pageWorkflowControl.GetNotifyList().ToList();
            _articleDetails.WordCount = SitecoreAddin.ActiveDocument.ComputeStatistics(0);

            SitecoreClient.SaveMetadataToSitecore(_articleDetails.ArticleNumber, new StructConverter().GetServerStruct(_articleDetails));
            //if (articleDetailsPageSelector.pageWorkflowControl.uxUnlockOnSave.Checked)
            //{
                //articleDetailsPageSelector.pageArticleInformationControl.CheckIn(false);
            //}

            //articleDetailsPageSelector.pageRelatedArticlesControl.PushSitecoreChanges();
            //articleDetailsPageSelector.UpdateFields();
            //articleDetailsPageSelector.ResetChangedStatus();

            List<ItemStruct> publications = SitecoreClient.GetPublications();
            ItemStruct selectedPublication = publications.FirstOrDefault(w => w.ID == _articleDetails.Publication);
            var webPublishDate = _articleDetails.WebPublicationDate;

            if (selectedPublication.ID != Guid.Empty && webPublishDate != DateTime.MinValue)
                DocumentPropertyEditor.WritePublicationAndDate(SitecoreAddin.ActiveDocument, selectedPublication.Name, webPublishDate.ToString());

            Microsoft.Office.Interop.Word.Document activeDocument = SitecoreAddin.ActiveDocument;

            if (activeDocument.ReadOnly == false && string.IsNullOrWhiteSpace(activeDocument.Path) == false)
            {
                WordUtils.Save(activeDocument);
            }
        }

        public void PreviewArticle()
        {
            string url = getPreviewUrl(false);

            ProcessStartInfo sInfo = new ProcessStartInfo(url);
            Process.Start(sInfo);
        }

        private string getPreviewUrl(bool isMobile)
        {
            fillArticleDetails();

            string guid = _articleDetails.ArticleGuid.ToString();// SitecoreArticle.GetArticleGuidByArticleNumber(GetArticleNumber());
            string domain = ApplicationConfig.GetPropertyValue("DomainName");

            if (domain.StartsWith("http") == false)
                domain = "http://" + domain;

            return domain + @"?sc_itemid={" + guid + @"}&sc_mode=preview&sc_lang=en" + (isMobile ? "&mobile=1" : String.Empty);

            string mobileUrlParam = isMobile ? "&mobile=1" : String.Empty;
            string redirect = Uri.EscapeDataString(domain + @"?sc_itemid={" + guid + @"}&sc_mode=preview&sc_lang=en" + mobileUrlParam);
            return domain + @"Util/LoginRedirectToPreview.aspx?redirect=" + redirect;


            //string guid = SitecoreClient.GetArticleGuidByArticleNumber(GetArticleNumber());
            //string domain = ApplicationConfig.GetPropertyValue("DomainName");
            //string mobileUrlParam = isMobile ? "&mobile=1" : String.Empty;
            //string redirect = (domain + @"?sc_itemid={" + guid + @"}&sc_mode=preview&sc_lang=en" + mobileUrlParam);
            //return redirect;
        }
    }
}
