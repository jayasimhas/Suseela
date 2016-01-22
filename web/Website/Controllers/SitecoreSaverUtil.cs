﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Fields;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.Areas.Account.Models;
using Jabberwocky.Glass.Models;
using Sitecore.Data;
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

		public SitecoreSaverUtil(Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil)
		{
			_sitecoreMasterService = sitecoreFactory(MasterDb);
			TempFileLocation = IsNullOrEmpty(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)) ?
				TempFolderFallover :
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\temp.";
			_articleUtil = articleUtil;

		}
		public void SaveArticleDetails(Guid articleGuid, WordPluginModel.ArticleStruct articleStruct, bool saveDocumentSpecificData = false, bool addVersion = true)
		{
			using (new Sitecore.SecurityModel.SecurityDisabler())
			{//accounting for issue restriction

				IArticle article = _sitecoreMasterService.GetItem<IArticle>(articleGuid);
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
			using (new Sitecore.SecurityModel.SecurityDisabler())
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
			Item updatedVersion;
			//IIPP-243
			if (!article.IsPublished && article.Planned_Publish_Date != articleStruct.WebPublicationDate)
			{
				MoveArticleIfNecessary(article, articleStruct);
			}
			IArticle newVersion;
			string userID = articleItem.Locking.GetOwner();
			bool loggedIn = false;
			if (!IsNullOrEmpty(userID))
			{
				loggedIn = Sitecore.Security.Authentication.AuthenticationManager.Login(userID);
			}

			var info = new WorkflowInfo(Guid.Empty.ToString(), Guid.Empty.ToString());


			try
			{
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
									// remove the old version from workflow and prevent from being published
									// Note: to remove an item from workflow requires using the fields, rather than the SetWorkflowInfo
									//  method, because the SetWorkflowInfo method does not allow empty strings 
									articleItem.Fields[Sitecore.FieldIDs.WorkflowState].Value = null;
									articleItem.Fields[Sitecore.FieldIDs.HideVersion].Value = "1";
								}
							}
						}
						//newVersion = article.InnerItem.Versions.AddVersion();
						updatedVersion = articleItem.Versions.AddVersion();
						newVersion = _sitecoreMasterService.GetItem<IArticle>(updatedVersion.ID.ToString());
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
						//new version also automatically unlocks, so we re-lock 
						//should we check to see if the previous version was locked
						//my guess is no because an author shouldn't be calling this method unless the article was locked in their favor anyways
						_sitecoreMasterService.GetItem<Item>(newVersion._Id).Locking.Lock();
					}
				}
			}

			catch (Exception ex)
			{
				var ax = new ApplicationException("Workflow: Error with saving details while saving article [" + article.Article_Number + "]!", ex);

				//_logger.Error("Workflow error:", ax);

				throw ax;
			}
			return newVersion;
		}

		private void SaveArticleFields(IArticle newArticle, IArticle originalArticle, WordPluginModel.ArticleStruct articleStruct, bool saveDocumentSpecificData)
		{
			using (new SecurityDisabler())
			{
				newArticle.Editorial_Notes = articleStruct.NotesToEditorial;
				if (articleStruct.Subtitle != null) newArticle.Sub_Title = articleStruct.Subtitle;
				if (articleStruct.Summary != null) newArticle.Summary = articleStruct.Summary;
				if (!originalArticle.IsPublished || articleStruct.WebPublicationDate != originalArticle.Planned_Publish_Date)
				{
					newArticle.Planned_Publish_Date = articleStruct.WebPublicationDate;
				}

				newArticle.Taxonomies = articleStruct.Taxonomoy.Select(x => _sitecoreMasterService.GetItem<IGlassBase>(x.ID));
				newArticle.Referenced_Articles = articleStruct.RelatedInlineArticles.Select(x => _sitecoreMasterService.GetItem<IGlassBase>(x));
				newArticle.Related_Articles = articleStruct.RelatedArticles.Select(x => _sitecoreMasterService.GetItem<IGlassBase>(x));

				if (saveDocumentSpecificData)
				{
					//the following will now always be saved and not just on a save entire document	
					newArticle.Supporting_Documents = articleStruct.SupportingDocumentPaths.Select(x => _sitecoreMasterService.GetItem<IGlassBase>(x));					
					newArticle.Referenced_Deals = Join(",", articleStruct.ReferencedDeals.ToArray());
				}
				newArticle.Authors = articleStruct.Authors.Select(x => _sitecoreMasterService.GetItem<IGlassBase>(x.ID));				
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
				_sitecoreMasterService.Save(newArticle);
			}
		}

		protected void RenameArticleItem(IArticle article, WordPluginModel.ArticleStruct articleStruct)
		{
			var articleItem = _sitecoreMasterService.GetItem<Item>(article._Id);
			string title = articleStruct.Title;
			if (title != null)
			{
				using (new SecurityDisabler())
				{
					articleItem[I___BasePageConstants.TitleFieldName] = title;
					if (!article.IsPublished)
					{
						string trim = title.Length > 100 ? title.Substring(0, 100).Trim() : title.Trim();
						articleItem["__Display name"] = trim;
						articleItem.Name = ItemUtil.ProposeValidItemName(trim);
					}
					_sitecoreMasterService.Save(articleItem);
				}
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

		public int SendDocumentToSitecore(string articleNumber, byte[] data, string extension, string username)
		{
			IArticle article = _articleUtil.GetArticleByNumber(articleNumber);

			return SendDocumentToSitecore(article, data, extension, username);
		}

		public int SendDocumentToSitecore(Guid articleGuid, byte[] data, string extension, string username)
		{
			IArticle article = _sitecoreMasterService.GetItem<IArticle>(articleGuid);
			return SendDocumentToSitecore(article, data, extension, username);
		}

		private int SendDocumentToSitecore(IArticle article, byte[] data, string extension, string username)
		{

			MediaItem doc = UploadWordDoc(article, ConvertBytesToWordDoc(data, article.Article_Number, extension), article._Id.ToString(), extension, username);
			using (new Sitecore.SecurityModel.SecurityDisabler())
			{
				using (new SecurityDisabler())
				{
					article.Word_Document.Url = doc.InnerItem.Paths.Path;
					//TODO - Set this document to be internal
					//article.Word_Document.Type = "internal";
					article.Word_Document.TargetId = new Guid(doc.ID.ToString());
					_sitecoreMasterService.Save(article);
				}
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
		/// <param name="username">Username (with domain) of uploader</param>
		/// <returns></returns>
		protected MediaItem UploadWordDoc(IArticle article, string fileName, string docName, string extension, string username)
		{
			return WordDocToMediaLibrary.SaveWordDocIntoMediaLibrary(article, fileName, docName, extension, username);
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
				//_logger.Error("Failed writing out the word document to path [" + fileName + "]!", axe);
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
	}
}
