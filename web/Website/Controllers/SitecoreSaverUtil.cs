using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Article.Search;
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

namespace Informa.Web.Controllers
{
	public class SitecoreSaverUtil
	{
		private readonly ISitecoreService _sitecoreMasterService;
		public const string MasterDb = "master";
		protected readonly string TempFolderFallover = Path.GetTempPath();
		protected string TempFileLocation;
		private readonly ArticleUtil _articleUtil;
		private readonly IArticleSearch _articleSearcher;
		private const string ArticleNumberLength = "000000";

		public SitecoreSaverUtil(Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil, IArticleSearch searcher)
		{
			_sitecoreMasterService = sitecoreFactory(MasterDb);
			TempFileLocation = IsNullOrEmpty(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) ? 
				TempFolderFallover : Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\temp.";
			_articleUtil = articleUtil;
			_articleSearcher = searcher;

		}

		public string GetLastArticleNumber(Guid publicationGuid)
		{
			IArticleSearchFilter filter = _articleSearcher.CreateFilter();
			var results = _articleSearcher.Search(filter);
			if (!results.Articles.Any())
			{
				return 0.ToString(ArticleNumberLength);
			}
			IEnumerable<string> articles = results.Articles.Select(a => a.Article_Number).OrderByDescending(b => b);
			string num = articles.First().Replace(SitecoreUtil.GetPublicationPrefix(publicationGuid), "");
			int n = int.Parse(num);
			return (n + 1).ToString(ArticleNumberLength);
		}

		public void SaveArticleDetails(Guid articleGuid, WordPluginModel.ArticleStruct articleStruct, bool saveDocumentSpecificData = false, bool addVersion = true)
		{
			using (new SecurityDisabler())
			{
				IArticle article = _sitecoreMasterService.GetItem<ArticleItem>(articleGuid);
				if (article == null)
				{
					throw new ApplicationException("Could not find article with Guid " + articleGuid);
				}

				SaveArticleDetails(article, articleStruct, saveDocumentSpecificData, addVersion, false);
			}
			Sitecore.Security.Authentication.AuthenticationManager.Logout();
		}

		public void SaveArticleDetails(string articleNumber, WordPluginModel.ArticleStruct articleStruct, bool saveDocumentSpecificData = false, bool addVersion = true)
		{
			using (new SecurityDisabler())
			{
				IArticle article = _articleUtil.GetArticleByNumber(articleNumber);
				if (article == null)
				{
					throw new ApplicationException("Could not find article for number [" + articleNumber + "]");
				}

				SaveArticleDetails(article, articleStruct, saveDocumentSpecificData, addVersion);
			}
			Sitecore.Security.Authentication.AuthenticationManager.Logout();
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
		private IArticle SaveArticleDetails(IArticle article, WordPluginModel.ArticleStruct articleStruct, bool saveDocumentSpecificData, bool addVersion, bool shouldNotify = true)
		{
			var articleItem = _sitecoreMasterService.GetItem<Item>(article._Id);

			//IIPP-243 - Moving the location of the article if needed
			if (!article.IsPublished && article.Planned_Publish_Date != articleStruct.WebPublicationDate)
			{
				MoveArticleIfNecessary(article, articleStruct);
			}
			string userID = articleItem.Locking.GetOwner();
			bool loggedIn = false;
			if (!IsNullOrEmpty(userID))
			{
				loggedIn = Sitecore.Security.Authentication.AuthenticationManager.Login(userID);
			}

			var newVersion = article;

			//TODO - Add version adn workflow informatiomn
			//try
			//{							Item updatedVersion;
			// var info = new WorkflowInfo(Guid.Empty.ToString(), Guid.Empty.ToString());
			//if (addVersion)
			//{
			//	using (new EditContext(articleItem))
			//	{
			//		ItemState itemState = articleItem.State;
			//		if (itemState != null)
			//		{
			//			WorkflowState workflowState = itemState.GetWorkflowState();
			//			if (workflowState != null)
			//			{
			//				IWorkflow workflow = itemState.GetWorkflow();

			//				string state = workflowState.StateID;
			//				if (workflow != null && state != null)
			//				{
			//					info = new WorkflowInfo(workflow.WorkflowID, state);
			//					// remove the old version from workflow and prevent from being published
			//					// Note: to remove an item from workflow requires using the fields, rather than the SetWorkflowInfo
			//					//  method, because the SetWorkflowInfo method does not allow empty strings 
			//					articleItem.Fields[Sitecore.FieldIDs.WorkflowState].Value = null;
			//					articleItem.Fields[Sitecore.FieldIDs.HideVersion].Value = "1";
			//				}
			//			}
			//		}
			//		//newVersion = article.InnerItem.Versions.AddVersion();
			//		updatedVersion = articleItem.Versions.AddVersion();
			//		newVersion = _sitecoreMasterService.GetItem<IArticle>(updatedVersion.ID.ToString());
			//	}
			//}
			//else
			//{
			//	newVersion = article;
			//}
			//newVersion = article;
			//}
			//catch (Exception ex)
			//{
			//	var ax = new ApplicationException("Workflow: Error with versioning/workflow while saving article [" + article.Article_Number + "]!", ex);
			//	throw ax;
			//}

			try
			{
				SaveArticleFields(newVersion, article, articleStruct, saveDocumentSpecificData);
				if (saveDocumentSpecificData)
				{
					RenameArticleItem(newVersion, articleStruct);
				}
				//TODO - Workflow Updates
				/*
				if (info.StateID != Guid.Empty.ToString() && info.WorkflowID != Guid.Empty.ToString())
				{
					// Doing this twice is intentional: when we do it once, the workflow field gets set to the empty string.
					//  I don't know why, but it does. Doing this twice sets it properly. Doing it not at all causes the 
					//  workflow field to be set to the empty string when leaving the edit context.
					newVersion.Database.DataManager.SetWorkflowInfo(newVersion, info);
					newVersion.Database.DataManager.SetWorkflowInfo(newVersion, info);
				}
				
				
				//an editcontext just for workflow
				using (new SecurityDisabler())
				{
					using (new EditContext(newVersion))
					{
						if (articleStruct.CommandID != Guid.Empty)
						{
							newVersion.NotificationTransientField.ShouldSend.Checked = true;
							_workflowController.ExecuteCommandAndGetWorkflowState(newVersion.InnerItem, articleStruct.CommandID.ToString());

							//TODO: explain thyself, heathen
							// I'm guessing we only should send notifications if the flag is set? Not sure why the need to explain this.
							if (shouldNotify)
							{
								_notificationsManager.SendArticleSpecificNotifications(newVersion, articleStruct);

							}
						}
					}
				}
				*/
				using (new SecurityDisabler())
				{
					if (loggedIn)
					{
						_sitecoreMasterService.GetItem<Item>(newVersion._Id).Locking.Lock();
					}
				}
			}

			catch (Exception ex)
			{
				var ax = new ApplicationException("Workflow: Error with saving details while saving article [" + article.Article_Number + "]!", ex);
				throw ax;
			}
			return newVersion;
		}

		private void SaveArticleFields(IArticle newArticle, IArticle originalArticle, WordPluginModel.ArticleStruct articleStruct, bool saveDocumentSpecificData)
		{
			using (new SecurityDisabler())
			{
				
				if (articleStruct.Subtitle != null) newArticle.Sub_Title = articleStruct.Subtitle;
				if (articleStruct.Summary != null) newArticle.Summary = articleStruct.Summary;
				if (!originalArticle.IsPublished || articleStruct.WebPublicationDate != originalArticle.Planned_Publish_Date)
				{
					newArticle.Planned_Publish_Date = articleStruct.WebPublicationDate;
				}

				//TODO - Add Taxonomy items
				//newArticle.Taxonomies = articleStruct.Taxonomoy.Select(x => _sitecoreMasterService.GetItem<ITaxonomy_Item>(x.ID));
				newArticle.Referenced_Articles = articleStruct.RelatedInlineArticles.Select(x => _sitecoreMasterService.GetItem<IGlassBase>(x));
				newArticle.Related_Articles = articleStruct.RelatedArticles.Select(x => _sitecoreMasterService.GetItem<ArticleItem>(x));

				if (saveDocumentSpecificData)
				{
					//the following will now always be saved and not just on a save entire document	
					newArticle.Supporting_Documents = articleStruct.SupportingDocumentPaths.Select(x => _sitecoreMasterService.GetItem<IGlassBase>(x));
					newArticle.Referenced_Deals = Join(",", articleStruct.ReferencedDeals.ToArray());
				}
				newArticle.Authors = articleStruct.Authors.Select(x => _sitecoreMasterService.GetItem<IAuthor>(x.ID));
				newArticle.Word_Count = articleStruct.WordCount.ToString();

				newArticle.Embargoed = articleStruct.Embargoed;
				DateTime webDate = articleStruct.WebPublicationDate;
				if (webDate != DateTime.MinValue || webDate != DateTime.MaxValue)
				{
					newArticle.Planned_Publish_Date = webDate;
				}
				else
				{
					newArticle.Planned_Publish_Date = new DateTime();
				}

				//TODO - Convert Label to dropdown or Single line text
				newArticle.Content_Type = _sitecoreMasterService.GetItem<ITaxonomy_Item>(articleStruct.Label);
				newArticle.Media_Type = _sitecoreMasterService.GetItem<ITaxonomy_Item>(articleStruct.MediaType);
				newArticle.Editorial_Notes = articleStruct.NotesToEditorial;
				newArticle.Featured_Image_16_9 = new Image {MediaId = articleStruct.FeaturedImage};
				newArticle.Featured_Image_Caption = articleStruct.FeaturedImageCaption;
				newArticle.Featured_Image_Source = articleStruct.FeaturedImageSource;

				_sitecoreMasterService.Save(newArticle);
			}
		}

		protected void RenameArticleItem(IArticle article, WordPluginModel.ArticleStruct articleStruct)
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

		protected void MoveArticleIfNecessary(IArticle article, WordPluginModel.ArticleStruct articleStruct)
		{
			using (new SecurityDisabler())
			{
				var publication = article.Publication;
				var newParent = _articleUtil.GenerateDailyFolder(publication, articleStruct.WebPublicationDate);
				if (newParent != null)
				{
					_sitecoreMasterService.Move(article, newParent);
				}
				//TODO - Verify if this feautre needs to be there or not.
				//_wordDocToMediaLibrary.MoveWordDocIfNecessary(article, articleStruct, oldIssueID);	

			}
		}

		/// <summary>
		/// Saves article text and fields that are dependent on the article text
		/// </summary>
		/// <param name="article"></param>
		/// <param name="articleText"></param>
		public void SaveArticleDetailsAndText(IArticle article, string articleText, WordPluginModel.ArticleStruct articleStruct)
		{
			using (new SecurityDisabler())
			{
				article = SaveArticleDetails(article, articleStruct, true, true);
				string parsedText = ParseXmltoHtml(articleText);
				article.Body = articleText;
				if (articleText != parsedText)
				{
					article.Body = parsedText;
				}
				//TODO - Replace the body text company names with company look ups and company references.
				/*
				string companyIdsCsv;
					article.Body = _companyFinder.ReplaceStrongCompanyNamesWithToken(articleText, out companyIdsCsv);
					article.Referenced_Companies = companyIdsCsv;					
				*/
				_sitecoreMasterService.Save(article);
			}
		}


		public int SendDocumentToSitecore(IArticle article, byte[] data, string extension)
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

				//TODO - Set this document to be internal
				//article.Word_Document.Type = "internal";
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
		protected MediaItem UploadWordDoc(IArticle article, string fileName, string docName, string extension)
		{
			var _wordDocLibrary = new WordDocToMediaLibrary(_sitecoreMasterService);
			return _wordDocLibrary.SaveWordDocIntoMediaLibrary(article, fileName, docName, extension);
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
