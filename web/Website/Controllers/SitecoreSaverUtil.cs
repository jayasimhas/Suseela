using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Article.Search;
using Informa.Library.Utilities;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.Areas.Account.Models;
using Jabberwocky.Glass.Models;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using Sitecore.Workflows;
using static System.String;
using File = System.IO.File;
using PluginModels;
using Informa.Models.DCD;
using Sitecore;
using Sitecore.Mvc.Extensions;
using Constants = Informa.Library.Utilities.References.Constants;
using Informa.Library.Publication;

namespace Informa.Web.Controllers
{
    public class SitecoreSaverUtil
    {
        private readonly ISitecoreService _sitecoreMasterService;
        protected readonly string TempFolderFallover = Path.GetTempPath();
        protected string TempFileLocation;
        private readonly ArticleUtil _articleUtil;
        private readonly IArticleSearch _articleSearcher;
        private readonly EmailUtil _emailUtil;
        private ISitePublicationWorkflow _siteWorkflow;

        public SitecoreSaverUtil(Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil, IArticleSearch searcher, EmailUtil emailUtil, ISitePublicationWorkflow siteWorkflow)
        {
            _sitecoreMasterService = sitecoreFactory(Constants.MasterDb);
            TempFileLocation = IsNullOrEmpty(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) ?
                TempFolderFallover : Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\temp.";
            _articleUtil = articleUtil;
            _articleSearcher = searcher;
            _emailUtil = emailUtil;
            _siteWorkflow = siteWorkflow;
        }

        public string GetLastArticleNumber(Guid publicationGuid)
        {
            IArticleSearchFilter filter = _articleSearcher.CreateFilter();
            var results = _articleSearcher.Search(filter);
            if (!results.Articles.Any())
            {
                return 0.ToString(Constants.ArticleNumberLength);
            }
            IEnumerable<string> articles = results.Articles.Select(a => a.Article_Number).OrderByDescending(b => b);
            string num = articles.First().Replace(SitecoreUtil.GetPublicationPrefix(publicationGuid), "");
            int n = int.Parse(num);
            return (n + 1).ToString(Constants.ArticleNumberLength);
        }

        public void SaveArticleDetails(Guid articleGuid, ArticleStruct articleStruct, bool saveDocumentSpecificData = false, bool addVersion = true)
        {
            //TODO:  Add Roles
            ArticleItem article = _sitecoreMasterService.GetItem<ArticleItem>(articleGuid);
            if (article == null)
            {
                throw new ApplicationException("Could not find article with Guid " + articleGuid);
            }
            SaveArticleDetails(article, articleStruct, saveDocumentSpecificData, addVersion);
        }

        public void SaveArticleDetails(string articleNumber, ArticleStruct articleStruct, bool saveDocumentSpecificData = false, bool addVersion = true)
        {

            ArticleItem article = _articleUtil.GetArticleByNumber(articleNumber);
            if (article == null)
            {
                throw new ApplicationException("Could not find article for number [" + articleNumber + "]");
            }

            SaveArticleDetails(article, articleStruct, saveDocumentSpecificData, addVersion);
        }

        /// <summary>
        /// Saves all the data about a changed article
        /// </summary>
        /// <param name="article">The existing article to update</param>
        /// <param name="articleStruct">The new data to be saved to the article</param>
        /// <param name="saveDocumentSpecificData"></param>
        /// <param name="addVersion">Should a new version be added</param>
        /// <param name="shouldNotify">Should notifications be sent for this update</param>
        /// <returns>The updated Sitecore item representing the article</returns>
        /// <remarks>This method could stand to be refactored into smaller chunks.</remarks>
        private ArticleItem SaveArticleDetails(ArticleItem article, ArticleStruct articleStruct, bool saveDocumentSpecificData, bool addVersion, bool shouldNotify = true)
        {
            var articleItem = _sitecoreMasterService.GetItem<Item>(article._Id);
            var publication = ArticleExtension.GetAncestorItemBasedOnTemplateID(articleItem);
            articleStruct.Publication = publication.ID.Guid;
            //IIPP-243 - Moving the location of the article if needed
            if (!article.IsPublished && article.Planned_Publish_Date != articleStruct.WebPublicationDate)
            {
                MoveArticleIfNecessary(article, articleStruct);
            }
            string userID = articleItem.Locking.GetOwner();
            bool loggedIn = false;
            if (!IsNullOrEmpty(userID))
            {
                loggedIn = Sitecore.Context.User.IsAuthenticated;
            }

            var newVersion = article;
            var info = new WorkflowInfo(Guid.Empty.ToString(), Guid.Empty.ToString());

            try
            {
                Item updatedVersion;

                if (addVersion)
                {
                    using (new EditContext(articleItem))
                    {
                        ItemState itemState = articleItem.State;
                        if (itemState != null)
                        {
                            WorkflowState workflowState = itemState.GetWorkflowState();
                            if (workflowState != null)
                            {
                                IWorkflow workflow = itemState.GetWorkflow();

                                string state = workflowState.StateID;
                                if (workflow != null && state != null)
                                {
                                    info = new WorkflowInfo(workflow.WorkflowID, state);
                                    //					// remove the old version from workflow and prevent from being published
                                    //					// Note: to remove an item from workflow requires using the fields, rather than the SetWorkflowInfo
                                    //					//  method, because the SetWorkflowInfo method does not allow empty strings 
                                    articleItem.Fields[Sitecore.FieldIDs.WorkflowState].Value = null;
                                    articleItem.Fields[Sitecore.FieldIDs.HideVersion].Value = "1";
                                }
                            }
                        }
                        //newVersion = article.InnerItem.Versions.AddVersion();
                        updatedVersion = articleItem.Versions.AddVersion();
                        newVersion = _sitecoreMasterService.GetItem<ArticleItem>(updatedVersion.ID.ToString());
                    }
                }
                else
                {
                    newVersion = article;
                }

            }
            catch (Exception ex)
            {
                var ax = new ApplicationException("Workflow: Error with versioning/workflow while saving article [" + article.Article_Number + "]!", ex);
                throw ax;
            }

            try
            {
                var newVersionItem = _sitecoreMasterService.GetItem<Item>(newVersion._Id);
                using (new EditContext(newVersionItem))
                {

                    SaveArticleFields(newVersion, article, articleStruct, saveDocumentSpecificData);
                    if (saveDocumentSpecificData)
                    {
                        RenameArticleItem(newVersion, articleStruct);
                    }


                    if (info.StateID != Guid.Empty.ToString() && info.WorkflowID != Guid.Empty.ToString())
                    {
                        // Doing this twice is intentional: when we do it once, the workflow field gets set to the empty string.
                        //  I don't know why, but it does. Doing this twice sets it properly. Doing it not at all causes the 
                        //  workflow field to be set to the empty string when leaving the edit context.

                        newVersionItem.Database.DataManager.SetWorkflowInfo(newVersionItem, info);
                        newVersionItem.Database.DataManager.SetWorkflowInfo(newVersionItem, info);

                        if (articleStruct.CommandID != Guid.Empty)
                        {
                            //newVersion.NotificationTransientField.ShouldSend.Checked = true;
                            _articleUtil.ExecuteCommandAndGetWorkflowState(newVersionItem, articleStruct.CommandID.ToString());

                            if (shouldNotify)
                            {
                                _emailUtil.SendNotification(articleStruct, info);

                            }
                        }
                    }
                }

                if (loggedIn)
                {
                    _sitecoreMasterService.GetItem<Item>(newVersion._Id).Locking.Lock();
                }
            }

            catch (Exception ex)
            {
                var ax = new ApplicationException("Workflow: Error with saving details while saving article [" + article.Article_Number + "]!", ex);
                throw ax;
            }

            //  Notifying the Editors when stories are edited after pushlished 
            if (articleStruct.IsPublished)
            {
                // Setting the workflow to "Edit After Publish".
                try
                {
                    var newVersionItem = _sitecoreMasterService.GetItem<Item>(newVersion._Id);
                    newVersionItem.Locking.Unlock();

                    using (new EditContext(newVersionItem))
                    {
                        newVersionItem[FieldIDs.WorkflowState] = _siteWorkflow.GetEditAfterPublishState(newVersionItem)._Id.ToString();// Constants.EditAfterPublishWorkflowCommand;
                    }

                    if (loggedIn)
                    {
                        _sitecoreMasterService.GetItem<Item>(newVersion._Id).Locking.Lock();
                    }
                }
                catch (Exception ex)
                {
                    var ax =
                        new ApplicationException(
                            "Workflow: Error with changing the workflow to Edit After Publish [" + article.Article_Number + "]!", ex);
                    throw ax;
                }

				try
				{ 
					_emailUtil.EditAfterPublishSendNotification(articleStruct);
				}
				catch (Exception ex)
				{
					Sitecore.Diagnostics.Log.Error("SitecoreSaverUtil.SaveArticleDetails EditAfterPublishSendNotification(): " + ex.ToString(), this);
				}
			}
            return newVersion;
        }

        private void SaveArticleFields(ArticleItem newArticle, ArticleItem originalArticle, ArticleStruct articleStruct, bool saveDocumentSpecificData)
        {
            using (new SecurityDisabler())
            {
                if (articleStruct.Title != null) newArticle.Title = articleStruct.Title;
                if (articleStruct.Subtitle != null) newArticle.Sub_Title = articleStruct.Subtitle;
                if (articleStruct.Summary != null) newArticle.Summary = articleStruct.Summary;
                if (!originalArticle.IsPublished || articleStruct.WebPublicationDate != originalArticle.Planned_Publish_Date)
                {
                    newArticle.Planned_Publish_Date = articleStruct.WebPublicationDate;
                }

                newArticle.Content_Type = _sitecoreMasterService.GetItem<ITaxonomy_Item>(articleStruct.Label);
                DateTime webDate = articleStruct.WebPublicationDate;
                if (webDate != DateTime.MinValue || webDate != DateTime.MaxValue)
                {
                    newArticle.Planned_Publish_Date = webDate;
                }
                else
                {
                    newArticle.Planned_Publish_Date = new DateTime();
                }
                //newArticle.Actual_Publish_Date = DateTime.;
                newArticle.Embargoed = articleStruct.Embargoed;
                newArticle.Media_Type = _sitecoreMasterService.GetItem<ITaxonomy_Item>(articleStruct.MediaType);
                newArticle.Authors = articleStruct.Authors.Select(x => _sitecoreMasterService.GetItem<IStaff_Item>(x.ID));
                newArticle.Editorial_Notes = articleStruct.NotesToEditorial;
                if (articleStruct.RelatedInlineArticles != null && articleStruct.RelatedInlineArticles.Any())
                {
                    newArticle.Referenced_Articles =
                        articleStruct.RelatedInlineArticles.Select(x => _sitecoreMasterService.GetItem<IArticle>(x));
                }
                if (articleStruct.RelatedArticles != null && articleStruct.RelatedArticles.Any())
                {
                    newArticle.Related_Articles = articleStruct.RelatedArticles.Select(x => _sitecoreMasterService.GetItem<IArticle>(x));
                }

                newArticle.Featured_Image_16_9 = new Image { MediaId = articleStruct.FeaturedImage };
                newArticle.Featured_Image_Caption = articleStruct.FeaturedImageCaption;
                newArticle.Featured_Image_Source = articleStruct.FeaturedImageSource;
                newArticle.Notification_Text = articleStruct.NotificationText;

                var taxonomyItems = new List<ITaxonomy_Item>();
                if (articleStruct.Taxonomoy.Any())
                {
                    taxonomyItems.AddRange(articleStruct.Taxonomoy
                        .Select(eachTaxonomy => _sitecoreMasterService.GetItem<ITaxonomy_Item>(eachTaxonomy.ID))
                        .Where(taxItem => taxItem != null));
                    newArticle.Taxonomies = taxonomyItems;
                }

                if (saveDocumentSpecificData)
                {
                    //the following will now always be saved and not just on a save entire document	
                    newArticle.Supporting_Documents = articleStruct.SupportingDocumentPaths.Select(x => _sitecoreMasterService.GetItem<IGlassBase>(x));
                    newArticle.Referenced_Deals = Join(",", articleStruct.ReferencedDeals.ToArray());
                }

                newArticle.Word_Count = articleStruct.WordCount.ToString();

                _sitecoreMasterService.Save(newArticle);
            }
        }

        protected void RenameArticleItem(ArticleItem article, ArticleStruct articleStruct)
        {
            string title = articleStruct.Title;
            if (title == null) return;
            using (new SecurityDisabler())
            {
                var articleItem = _sitecoreMasterService.GetItem<Item>(article._Id);
                articleItem.Editing.BeginEdit();
                articleItem[I___BasePageConstants.TitleFieldName] = title;
                if (!article.IsPublished)
                {
                    string trim = title.Length > 100 ? title.Substring(0, 100).Trim() : title.Trim();
                    articleItem["__Display name"] = trim;
                    articleItem.Name = ItemUtil.ProposeValidItemName(trim);
                }
                articleItem.Editing.EndEdit();
            }
        }

        protected void MoveArticleIfNecessary(ArticleItem article, ArticleStruct articleStruct)
        {
            var item = _sitecoreMasterService.GetItem<Item>(article._Id);
            using (new SecurityDisabler())
            {
                var publicationItem = ArticleExtension.GetAncestorItemBasedOnTemplateID(item);
                if (publicationItem != null)
                {
                    var publication = publicationItem.ID.Guid;
                    var newParent = _articleUtil.GenerateDailyFolder(publication, articleStruct.WebPublicationDate);
                    if (newParent != null)
                    {
                        _sitecoreMasterService.Move(article, newParent);
                    }
                    //TODO - Verify if this feautre needs to be there or not.
                    //_wordDocToMediaLibrary.MoveWordDocIfNecessary(article, articleStruct, oldIssueID);	
                }
            }
        }

        /// <summary>
        /// Saves article text and fields that are dependent on the article text
        /// </summary>
        /// <param name="article"></param>
        /// <param name="articleText"></param>
        /// <param name="articleStruct"></param>
        public void SaveArticleDetailsAndText(ArticleItem article, string articleText, ArticleStruct articleStruct)
        {
			try { 
            using (new SecurityDisabler())
            {
                article = SaveArticleDetails(article, articleStruct, true, true);
                string parsedText = ParseXmltoHtml(articleText);
                article.Body = articleText;
                if (articleText != parsedText)
                {
                    article.Body = parsedText;
                }

                string companyIdsCsv;
                article.Body = CompanyTokenizer.ReplaceStrongCompanyNamesWithToken(articleText, out companyIdsCsv);
                article.Referenced_Companies = companyIdsCsv;

                _sitecoreMasterService.Save(article);
            }
			}
			catch (Exception ex)
			{
				Sitecore.Diagnostics.Log.Error("SaveArticleDetailsAndText: " + ex.ToString(), this);
			}
		}

        public int SendDocumentToSitecore(ArticleItem article, byte[] data, string extension)
        {
            MediaItem doc = UploadWordDoc(article, ConvertBytesToWordDoc(data, article.Article_Number, extension), article._Id.ToString(), extension);
            using (new SecurityDisabler())
            {
                article.Word_Document = new Link
                {
                    Url = doc.InnerItem.Paths.Path,
                    TargetId = new Guid(doc.ID.ToString()),
                    Type = LinkType.Internal
                };
                _sitecoreMasterService.Save(article);
            }
            return doc.InnerItem.Version.Number;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="article"></param>
        /// <param name="fileName"></param>
        /// <param name="docName"></param>
        /// <param name="extension"></param>		
        /// <returns></returns>
        protected MediaItem UploadWordDoc(ArticleItem article, string fileName, string docName, string extension)
        {
            var wordDocLibrary = new WordDocToMediaLibrary(_sitecoreMasterService);
            return wordDocLibrary.SaveWordDocIntoMediaLibrary(article, fileName, docName, extension);
        }

        protected string ConvertBytesToWordDoc(byte[] data, string articleID, string extension)
        {
            var fileName = TempFileLocation + articleID + extension;

            if (IsFileUsedbyAnotherProcess(fileName))
            {
                fileName = TempFileLocation + articleID + "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + extension;
            }

            FileStream fs = null;
            MemoryStream ms = null;

            try
            {
                ms = new MemoryStream(data);
                fs = new FileStream(fileName, FileMode.Create);
                ms.WriteTo(fs);
            }
            catch (Exception ex)
            {
                var axe = new ApplicationException("Failed writing out the word document to path [" + fileName + "]!", ex);
                throw axe;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                }

            }

            return fileName;
        }

        protected bool IsFileUsedbyAnotherProcess(string filename)
        {
            var info = new FileInfo(filename);
            if (!info.Exists)
            { return false; }

            FileStream fs = null;
            try
            {
                fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            return false;

        }

        protected string ParseXmltoHtml(string articleText)
        {
            try
            {
                var x = new XmlDocument();
                x.LoadXml(articleText);

                return x.InnerXml;
            }
            catch (Exception ex)
            {
                var ax = new ApplicationException("Workflow: Error parsing article text!", ex);
                throw ax;
            }
        }
    }
}
