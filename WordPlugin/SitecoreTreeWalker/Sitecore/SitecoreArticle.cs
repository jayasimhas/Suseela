using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Informa.Web.Areas.Account.Models;
using Informa.Web.Controllers;
using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using SitecoreTreeWalker.Custom_Exceptions;
using SitecoreTreeWalker.document;
using SitecoreTreeWalker.SitecoreServer;
using SitecoreTreeWalker.SitecoreTree;
using SitecoreTreeWalker.User;
using SitecoreTreeWalker.Util;
using SitecoreTreeWalker.Util.Document;
using SitecoreTreeWalker.WebserviceHelper;
using ArticleStruct = SitecoreTreeWalker.SitecoreServer.ArticleStruct;
using WorkflowState = SitecoreTreeWalker.SitecoreTree.WorkflowState;

namespace SitecoreTreeWalker
{
	public class SitecoreArticle
	{
		//TODO - Add env variable
		private static string webApiURL = "http://informa8.ashah.velir.com/api/";

		public ArticleStruct SaveStubToSitecore(string articleName, string publicationDate, Guid publicationID)
		{
			using (var client = new HttpClient())
			{
				var article = new WordPluginModel.CreateArticleRequest() { Name = articleName, PublicationID = publicationID, PublicationDate = publicationDate };
				var response = client.PostAsJsonAsync($"{webApiURL}SitecoreSaver/CreateArticle", article).Result;
				var articleItem = JsonConvert.DeserializeObject<ArticleStruct>(response.Content.ReadAsStringAsync().Result);
				return articleItem;
			}
		}
		public ArticleStruct SaveStubToSitecore(string articleName, string publicationDate, Guid publicationID, Guid publicationIssue)
		{
			var server = new SitecoreServer.SCServer();
			server.Url = Constants.EDITOR_ENVIRONMENT_SERVERURL;
			return server.CreatePubArtGet(articleName, publicationDate, publicationID, publicationIssue, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
		}

		public static bool DoesArticleNameAlreadyExistInIssue(SitecoreTree.ArticleStruct articleDetails)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}DoesArticleNameAlreadyExistInIssue").Result;				
				var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
				return flag;
			}
		}

		public static void SaveMetadataToSitecore(string articleID, ArticleStruct articleData)
		{
			var server = new SitecoreServer.SCServer();
			server.Url = Constants.EDITOR_ENVIRONMENT_SERVERURL;
			server.SaveArticleDetails(articleID, articleData, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
		}

		public static void SaveArticleDetailsByGuid(Guid articleGuid, WordPluginModel.ArticleStruct articleData)
		{
			using (var client = new HttpClient())
			{
				var article = new WordPluginModel.SaveArticleDetailsByGuid() { ArticleGuid = articleGuid, ArticleData = articleData};
				var response = client.PostAsJsonAsync($"{webApiURL}SaveArticleDetailsByGuid", article).Result;
			}
			/*
			var server = new SitecoreServer.SCServer();
			server.Url = Constants.EDITOR_ENVIRONMENT_SERVERURL;
			server.SaveArticleDetailsByGuid(articleGuid, articleData, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);*/
		}

		public static string GetArticleUrl(string articleNumber)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{webApiURL}/SitecoreSaver/GetArticleUrl", articleNumber).Result;
				var url = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
				return url;
			}
			/*
			var sctree = new SitecoreTree.SCTree();
            sctree.Url = Constants.EDITOR_ENVIRONMENT_LOGINURL;
			return sctree.GetArticleUrl(articleNumber, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
			*/
		}

		public static string GetArticlePreviewUrl(string articleNumber)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{webApiURL}/SitecoreSaver/PreviewUrlArticle", articleNumber).Result;
				var url = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
				return url;
			}
			/*
			var sctree = new SitecoreTree.SCTree();
			sctree.Url = Constants.EDITOR_ENVIRONMENT_LOGINURL;
			return sctree.PreviewUrlArticle(articleNumber, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
			*/
		}

		public static string GetArticleDynamicUrl(string articleNumber)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{webApiURL}/SitecoreSaver/GetArticleDynamicUrl", articleNumber).Result;
				var url = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
				return url;
			}
			/*
			var sctree = new SitecoreTree.SCTree();
			sctree.Url = Constants.EDITOR_ENVIRONMENT_LOGINURL;
			return sctree.GetArticleDynamicUrl(articleNumber, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
			*/
		}

		public static bool DoesArticleHaveText(string articleNumber)
		{
			var sctree = new SCTree();
			sctree.Url = Constants.EDITOR_ENVIRONMENT_LOGINURL;
			return sctree.DoesArticleHaveText(articleNumber, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
		}

		public static bool DoesArticleHaveText(Guid articleGuid)
		{
			var sctree = new SCTree();
			sctree.Url = Constants.EDITOR_ENVIRONMENT_LOGINURL;
			return sctree.DoesArticleGuidHaveText(articleGuid, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
		}

		public static bool DoesArticleExist(string articleNumber)
		{
			var sctree = new SCTree();
			sctree.Url = Constants.EDITOR_ENVIRONMENT_LOGINURL;
			return sctree.DoesArticleExist(articleNumber, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
		}

		public static bool DoesArticleExist(Guid articleGuid)
		{
			var sctree = new SCTree();
			sctree.Url = Constants.EDITOR_ENVIRONMENT_LOGINURL;
			return sctree.DoesArticleGuidExist(articleGuid, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
		}

		public static CheckoutStatus GetLockedStatus(string articleNumber)
		{
			var server = new SitecoreServer.SCServer();
			server.Url = Constants.EDITOR_ENVIRONMENT_SERVERURL;
			return server.GetLockedStatus(articleNumber, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
		}

		public static CheckoutStatus GetLockedStatus(Guid articleGuid)
		{
			var server = new SitecoreServer.SCServer();
			server.Url = Constants.EDITOR_ENVIRONMENT_SERVERURL;
			return server.GetLockedStatusByGuid(articleGuid, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
		}

		public static bool CheckOutArticle(string articleNumber, string username)
		{
			var server = new SitecoreServer.SCServer();
			return server.CheckOutArticle(articleNumber, username, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
		}

		public static bool CheckOutArticle(Guid articleGuid, string username)
		{
			var server = new SitecoreServer.SCServer();
			server.Url = Constants.EDITOR_ENVIRONMENT_SERVERURL;
			return server.CheckOutArticleGuid(articleGuid, username, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
		}

		public static bool CheckInArticle(string articleNumber)
		{
			var server = new SitecoreServer.SCServer();
			server.Url = Constants.EDITOR_ENVIRONMENT_SERVERURL;
			return server.CheckInArticle(articleNumber, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
		}

		public static bool CheckInArticle(Guid articleGuid)
		{
			var server = new SitecoreServer.SCServer();
			server.Url = Constants.EDITOR_ENVIRONMENT_SERVERURL;
			return server.CheckInArticleGuid(articleGuid, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
		}

		public static string GetArticleGuidByArticleNumber(string articleNumber)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{webApiURL}/SitecoreSaver/GetArticleGuidByNum", articleNumber).Result;
				var articleGuid = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
				return articleGuid;
			}
		}

		public static SitecoreTree.ArticlePreviewInfo GetArticlePreviewInfo(string articleNumber)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{webApiURL}/SitecoreSaver/GetArtPreInfo", articleNumber).Result;
				var versionNumber = JsonConvert.DeserializeObject<SitecoreTree.ArticlePreviewInfo>(response.Content.ReadAsStringAsync().Result);
				return versionNumber;
			}
		}

		public static List<SitecoreTree.ArticlePreviewInfo> GetArticlePreviewInfo(List<Guid> guids)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{webApiURL}/GetArticlePreviewInfo", guids).Result;
				var articlePreviewCollection = JsonConvert.DeserializeObject<List<SitecoreTree.ArticlePreviewInfo>>(response.Content.ReadAsStringAsync().Result);
				return articlePreviewCollection;
			}
		}

		public static void SaveArticleText(string articleNumber, string text, ArticleStruct articleStruct)
		{
			var server = new SitecoreServer.SCServer();
			server.Url = Constants.EDITOR_ENVIRONMENT_SERVERURL;
			server.SaveArticleText(articleNumber, text, articleStruct, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
		}

		public static void SaveArticleText(Guid articleGuid, string text, ArticleStruct articleStruct)
		{
			var server = new SitecoreServer.SCServer();
			server.Url = Constants.EDITOR_ENVIRONMENT_SERVERURL;
			server.SaveArticleTextByGuid(articleGuid, text, articleStruct, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
		}

		public static int GetWordVersionNumber(string articleNumber)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{webApiURL}/SitecoreSaver/GetWordVersionNum", articleNumber).Result;
				var versionNumber = JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
				return versionNumber;
			}
		}

		public static int GetWordVersionNumber(Guid articleguid)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{webApiURL}/SitecoreSaver/GetWordVersionNumByGuid", articleguid).Result;
				var versionNumber = JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
				return versionNumber;
			}
		}

		public int SendDocumentToSitecore(string articleNumber, byte[] data, string extension, string username)
		{
			return _server.SendDocumentToSitecore(articleNumber, data, extension, username, SitecoreUser.GetUser().Password);
		}

		public int SendDocumentToSitecore(Guid articleGuid, byte[] data, string extension, string username)
		{
			return _server.SendDocumentToSitecoreByGuid(articleGuid, data, extension, username, SitecoreUser.GetUser().Password);
		}

		public SitecoreArticle()
			: this(new SCServer(), new SCTree(), new WordUtils())
		{ }

		public SitecoreArticle(SCServer server, SCTree tree, WordUtils wordUtils)
		{
			_server = server;
			_server.Url = Constants.EDITOR_ENVIRONMENT_SERVERURL;
			_tree = tree;
			_tree.Url = Constants.EDITOR_ENVIRONMENT_LOGINURL;
			_wordUtils = wordUtils;
		}

		public static WorkflowState GetWorkflowState(string articleNumber)
		{
			var sctree = new SitecoreTree.SCTree();
			sctree.Url = Constants.EDITOR_ENVIRONMENT_LOGINURL;
			return sctree.GetWorkflowState(articleNumber, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
		}

		public static WorkflowState GetWorkflowState(Guid articleGuid)
		{
			var sctree = new SitecoreTree.SCTree();
			sctree.Url = Constants.EDITOR_ENVIRONMENT_LOGINURL;
			return sctree.GetWorkflowStateByGuid(articleGuid, SitecoreUser.GetUser().Username, SitecoreUser.GetUser().Password);
		}

		public static string GetDocumentPassword()
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}GetDocumentPassword").Result;
				return response.Content.ReadAsStringAsync().Result;
			}
		}

		public List<string> SaveArticle(Document activeDocument, SitecoreTree.ArticleStruct articleDetails,
			Guid workflowCommand, SitecoreTree.StaffStruct[] notifications, string articleNumber, string body = null)
		{

			string text;
			try
			{
				text = body ?? _wordUtils.GetWordDocTextWithStyles(activeDocument).ToString();
			}
			catch (InsecureIFrameException insecureIframe)
			{
				string message = String.Empty;
				foreach (string iframeURL in insecureIframe.InsecureIframes)
				{
					message += String.Format("\n{0}", iframeURL);
				}

				return new List<string> { "The following multimedia content is not secure. Please correct and try to save again. " + message };
			}
			catch (InvalidHtmlException)
			{
				return new List<string> { String.Empty };
			}
			catch (Exception ex)
			{
				Globals.SitecoreAddin.LogException("Error when parsing article!", ex);
				return new List<string> { "The document could not be parsed to transfer to Sitecore! Details in logs." };
			}
			try
			{
				var documentCustomProperties = new DocumentCustomProperties(activeDocument);
				articleDetails.ArticleSpecificNotifications = notifications;
				articleDetails.WordCount = activeDocument.ComputeStatistics(0);
				//articleDetails.ChildArticles = _wordUtils.SidebarArticleParser.SidebarArticleGuids.ToArray();
				articleDetails.ReferencedDeals = ReferencedDealParser.GetReferencedDeals(activeDocument).ToArray();
				articleDetails.CommandID = workflowCommand;
				articleDetails.SupportingDocumentPaths = _wordUtils.GetSupportingDocumentPaths().ToArray();
				Globals.SitecoreAddin.Log("Local document version before check: " +
										  documentCustomProperties.WordSitecoreVersionNumber);
				var currentVersion = articleDetails.ArticleGuid != Guid.Empty
										 ? SitecoreArticle.GetWordVersionNumber(articleDetails.ArticleGuid)
										 : SitecoreArticle.GetWordVersionNumber(articleNumber);
				Globals.SitecoreAddin.Log("Sitecore document version before save: " + currentVersion);
				if (currentVersion != -1)
				{
					documentCustomProperties.WordSitecoreVersionNumber = currentVersion + 1;
				}
				else
				{
					documentCustomProperties.WordSitecoreVersionNumber = 1;
				}
				if (articleDetails.ArticleGuid != Guid.Empty)
				{
					SitecoreArticle.SaveArticleText(articleDetails.ArticleGuid, text,_structConverter.GetServerStruct(articleDetails));
				}
				else
				{
					SitecoreArticle.SaveArticleText(articleNumber, text,_structConverter.GetServerStruct(articleDetails));
				}

				Globals.SitecoreAddin.Log("Local document version after check: " +
										  documentCustomProperties.WordSitecoreVersionNumber);

				SendWordDocumentToSitecore(activeDocument, documentCustomProperties, articleNumber, articleDetails);
				documentCustomProperties.WordSitecoreVersionNumber = articleDetails.ArticleGuid != Guid.Empty
																		 ? SitecoreArticle.GetWordVersionNumber(
																			 articleDetails.ArticleGuid)
																		 : SitecoreArticle.GetWordVersionNumber(
																			 articleNumber);

				Globals.SitecoreAddin.Log("Local document version after cautionary update: " +
										  documentCustomProperties.WordSitecoreVersionNumber);

				var path = activeDocument.Path;
				if (!string.IsNullOrWhiteSpace(path) && WordUtils.FileExistsAndNotReadOnly(path, activeDocument.Name))
				{
					WordUtils.Save(activeDocument);
				}

				return _wordUtils.Errors;

			}
			catch (Exception ex)
			{
				Globals.SitecoreAddin.LogException("Error when saving article!", ex);
				throw;
			}
		}

		protected void SendWordDocumentToSitecore(Document activeDocument, DocumentCustomProperties documentCustomProperties, string articleNumber, SitecoreTree.ArticleStruct articleDetails)
		{
			string extension = GetExtension(activeDocument);
			DocumentProtection.Protect(documentCustomProperties);

			byte[] data = _wordUtils.GetWordBytes(activeDocument);
			if (data == null)
			{
				throw new Exception("Error saving file to disk.");
			}

			DocumentProtection.Unprotect(documentCustomProperties);
			string uploader = SitecoreUser.GetUser().Username;
			int wordSitecoreVersionNumber = articleDetails.ArticleGuid != Guid.Empty
												? this.SendDocumentToSitecore(articleDetails.ArticleGuid, data, extension, uploader)
												: this.SendDocumentToSitecore(articleNumber, data, extension, uploader);
			documentCustomProperties.WordSitecoreVersionNumber = wordSitecoreVersionNumber;
			//MessageBox.Show(@"recv w ver: " + _documentCustomProperties.WordSitecoreVersionNumber);
		}

		protected string GetExtension(Document activeDocument)
		{
			string extension;
			int indexOfDot = activeDocument.Name.LastIndexOf(".");
			if (indexOfDot < 0 || indexOfDot >= activeDocument.Name.Length)
			{
				// If, for some reason, we can't get the extension normally, just default to .docx

				extension = ".docx";
			}
			else
			{
				extension = activeDocument.Name.Substring(indexOfDot);
				if (!(extension == ".docx" || extension == ".doc"))
				{
					// if we get a weird extension, this is probably a problem. 
					//TODO: what do we do if we get a weird extension?
					extension = ".docx";
				}
			}
			return extension;
		}

		protected SCServer _server;
		protected SCTree _tree;
		protected WordUtils _wordUtils;
		protected static StructConverter _structConverter = new StructConverter();
	}
}
