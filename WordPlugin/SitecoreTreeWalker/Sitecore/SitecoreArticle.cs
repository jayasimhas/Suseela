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
using ArticlePreviewInfo = Informa.Web.Areas.Account.Models.WordPluginModel.ArticlePreviewInfo;
using ArticleStruct = Informa.Web.Areas.Account.Models.WordPluginModel.ArticleStruct;
using WorkflowState = Informa.Web.Areas.Account.Models.WordPluginModel.WorkflowState;

namespace SitecoreTreeWalker
{
	public class SitecoreArticle
	{
		//TODO - Add env variable
		//public  string webApiURL = $"{Constants.EDITOR_ENVIRONMENT_SERVERURL}"+"/api/";

		public ArticleStruct SaveStubToSitecore(string articleName, string publicationDate, Guid publicationID)
		{
			using (var client = new HttpClient())
			{
				var article = new WordPluginModel.CreateArticleRequest() { Name = articleName, PublicationID = publicationID, PublicationDate = publicationDate };
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}CreateArticle", article).Result;
				var articleItem = JsonConvert.DeserializeObject<ArticleStruct>(response.Content.ReadAsStringAsync().Result);
				return articleItem;
			}
		}

		public static bool DoesArticleNameAlreadyExistInIssue(ArticleStruct articleDetails)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}DoesArticleNameAlreadyExistInIssue").Result;
				var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
				return flag;
			}
		}

		public static void SaveMetadataToSitecore(string articleID, ArticleStruct articleData)
		{
			using (var client = new HttpClient())
			{
				var article = new WordPluginModel.SaveArticleDetails() { ArticleNumber = articleID, ArticleData = articleData };
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}SaveArticleDetails", article).Result;
			}
		}

		public static void SaveArticleDetailsByGuid(Guid articleGuid, ArticleStruct articleData)
		{
			using (var client = new HttpClient())
			{
				var article = new WordPluginModel.SaveArticleDetailsByGuid() { ArticleGuid = articleGuid, ArticleData = articleData };
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}SaveArticleDetailsByGuid", article).Result;
			}
		}

		public static string GetArticleUrl(string articleNumber)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetArticleUrl", articleNumber).Result;
				var url = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
				return url;
			}
		}

		public static string GetArticlePreviewUrl(string articleNumber)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}PreviewUrlArticle", articleNumber).Result;
				var url = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
				return url;
			}
		}

		public static string GetArticleDynamicUrl(string articleNumber)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetArticleDynamicUrl", articleNumber).Result;
				var url = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
				return url;
			}
		}

		public static bool DoesArticleHaveText(string articleNumber)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}DoesArticleHaveText",articleNumber).Result;
				var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
				return flag;
			}
		}

		public static bool DoesArticleHaveText(Guid articleGuid)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}DoesArticleGuidHaveText",articleGuid).Result;
				var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
				return flag;
			}
		}

		public static bool DoesArticleExist(string articleNumber)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}DoesArticleExist",articleNumber).Result;
				var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
				return flag;
			}
		}

		public static bool DoesArticleExist(Guid articleGuid)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}DoesArticleGuidExist",articleGuid).Result;
				var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
				return flag;
			}
		}

		public static WordPluginModel.CheckoutStatus GetLockedStatus(string articleNumber)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetLockedStatus", articleNumber).Result;
				return JsonConvert.DeserializeObject<WordPluginModel.CheckoutStatus>(response.Content.ReadAsStringAsync().Result);
			}
		}

		public static WordPluginModel.CheckoutStatus GetLockedStatus(Guid articleGuid)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetLockedStatusByGuid", articleGuid).Result;
				return JsonConvert.DeserializeObject<WordPluginModel.CheckoutStatus>(response.Content.ReadAsStringAsync().Result);
			}
		}

		public static bool CheckOutArticle(string articleNumber, string username)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}CheckOutArticle",articleNumber).Result;
				var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
				return flag;
			}
		}

		public static bool CheckOutArticle(Guid articleGuid, string username)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}CheckOutArticleByGuid",articleGuid).Result;
				var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
				return flag;
			}
		}

		public static bool CheckInArticle(string articleNumber)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}CheckInArticle", articleNumber).Result;
				var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
				return flag;
			}
		}

		public static bool CheckInArticle(Guid articleGuid)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}CheckInArticleByGuid", articleGuid).Result;
				var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
				return flag;
			}
		}

		public static string GetArticleGuidByArticleNumber(string articleNumber)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetArticleGuidByNum", articleNumber).Result;
				var articleGuid = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
				return articleGuid;
			}
		}

		public static ArticlePreviewInfo GetArticlePreviewInfo(string articleNumber)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetArticlePreviewInfo", articleNumber).Result;
				var previewInfo = JsonConvert.DeserializeObject<ArticlePreviewInfo>(response.Content.ReadAsStringAsync().Result);
				return previewInfo;
			}
		}

		public static List<ArticlePreviewInfo> GetArticlePreviewInfo(List<Guid> guids)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetArticlePreviewInfoByGuids", guids).Result;
				var articlePreviewCollection = JsonConvert.DeserializeObject<List<ArticlePreviewInfo>>(response.Content.ReadAsStringAsync().Result);
				return articlePreviewCollection;
			}
		}

		public static void SaveArticleText(string articleNumber, string text, ArticleStruct articleStruct)
		{
			using (var client = new HttpClient())
			{
				var article = new WordPluginModel.SaveArticleText() { ArticleNumber = articleNumber, ArticleData = articleStruct, WordText = text };
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}SaveArticleText", article).Result;
			}
		}

		public static void SaveArticleText(Guid articleGuid, string text, ArticleStruct articleStruct)
		{
			using (var client = new HttpClient())
			{
				var article = new WordPluginModel.SaveArticleTextByGuid() { ArticleGuid = articleGuid, ArticleData = articleStruct, WordText = text };
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}SaveArticleTextByGuid", article).Result;
			}
		}

		public static int GetWordVersionNumber(string articleNumber)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetWordVersionNum", articleNumber).Result;
				var versionNumber = JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
				return versionNumber;
			}
		}

		public static int GetWordVersionNumber(Guid articleguid)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetWordVersionNumByGuid", articleguid).Result;
				var versionNumber = JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
				return versionNumber;
			}
		}

		public int SendDocumentToSitecore(string articleNumber, byte[] data, string extension, string username)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}SendDocumentToSitecore", new WordPluginModel.SendDocumentToSitecore() {ArticleNumber = articleNumber,Data = data,Extension = extension}).Result;
				return JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);				
			}
		}

		public int SendDocumentToSitecoreByGuid(Guid articleGuid, byte[] data, string extension, string username)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}SendDocumentToSitecoreByGuid", new WordPluginModel.SendDocumentToSitecoreByGuid() { ArticlGuid = articleGuid, Data = data, Extension = extension }).Result;
				return JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
			}
		}

		public static UserStatusStruct AuthenticateUser(string username,string password)
		{
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}AuthenticateUser", new WordPluginModel.LoginModel() {Username= username,Password = password}).Result;
				var userStatus = JsonConvert.DeserializeObject<UserStatusStruct>(response.Content.ReadAsStringAsync().Result);
				return userStatus;
			}
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
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetDocumentPassword").Result;
				return response.Content.ReadAsStringAsync().Result;
			}
		}

		public List<string> SaveArticle(Document activeDocument, ArticleStruct articleDetails, Guid workflowCommand, WordPluginModel.StaffStruct[] notifications, string articleNumber, string body = null)
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
					message += $"\n{iframeURL}";
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
				articleDetails.ArticleSpecificNotifications = notifications.ToList();
				articleDetails.WordCount = activeDocument.ComputeStatistics(0);
				articleDetails.ReferencedDeals = ReferencedDealParser.GetReferencedDeals(activeDocument).ToList();
				articleDetails.CommandID = workflowCommand;
				articleDetails.SupportingDocumentPaths = _wordUtils.GetSupportingDocumentPaths().ToList();
				Globals.SitecoreAddin.Log("Local document version before check: " +
										  documentCustomProperties.WordSitecoreVersionNumber);
				var currentVersion = articleDetails.ArticleGuid != Guid.Empty
										 ? GetWordVersionNumber(articleDetails.ArticleGuid)
										 : GetWordVersionNumber(articleNumber);
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
					SaveArticleText(articleDetails.ArticleGuid, text, _structConverter.GetServerStruct(articleDetails));
				}
				else
				{
					SaveArticleText(articleNumber, text, _structConverter.GetServerStruct(articleDetails));
				}

				Globals.SitecoreAddin.Log("Local document version after check: " +
										  documentCustomProperties.WordSitecoreVersionNumber);

				SendWordDocumentToSitecore(activeDocument, documentCustomProperties, articleNumber, articleDetails);
				documentCustomProperties.WordSitecoreVersionNumber = articleDetails.ArticleGuid != Guid.Empty ? GetWordVersionNumber(articleDetails.ArticleGuid) : GetWordVersionNumber(articleNumber);

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

		protected void SendWordDocumentToSitecore(Document activeDocument, DocumentCustomProperties documentCustomProperties, string articleNumber, ArticleStruct articleDetails)
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
												? this.SendDocumentToSitecoreByGuid(articleDetails.ArticleGuid, data, extension, uploader)
												: this.SendDocumentToSitecore(articleNumber, data, extension, uploader);
			documentCustomProperties.WordSitecoreVersionNumber = wordSitecoreVersionNumber;
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
