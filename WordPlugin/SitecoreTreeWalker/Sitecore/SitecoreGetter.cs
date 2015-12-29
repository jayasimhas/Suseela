using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using SitecoreTreeWalker.SitecoreTree;
using SitecoreTreeWalker.User;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using SitecoreTreeWalker.Config;

/// <summary>
namespace SitecoreTreeWalker.Sitecore
{
	class SitecoreGetter
	{
		private static List<SitecoreTree.StaffStruct> _authors;
		private static ArticleStruct _articleDetails = new ArticleStruct();
		protected static SitecoreUser _sitecoreUser = SitecoreUser.GetUser();
		private static string webApiURL = "http://informa8.ashah.velir.com/api/";

		public static List<TaxonomyStruct> SearchTaxonomy(string term)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}SearchTaxonomy?searchTerm={term}").Result;
				var taxonomy = JsonConvert.DeserializeObject<List<TaxonomyStruct>>(response.Content.ReadAsStringAsync().Result);
				return taxonomy;
			}
		}

		public static HDirectoryStruct GetHierarchyByGuid(Guid taxonomyGuid)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}GetHierarchyByGuid?guid={taxonomyGuid}").Result;
				var directoryList = JsonConvert.DeserializeObject<HDirectoryStruct>(response.Content.ReadAsStringAsync().Result);
				return directoryList;
			}
		}

		public static MediaItemStruct GetMediaStatistics(string path)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}GetMediaStatistics?path={path}").Result;
				var mediaItem = JsonConvert.DeserializeObject<MediaItemStruct>(response.Content.ReadAsStringAsync().Result);
				return mediaItem;
			}
		}

		/// <summary>
		/// If list of authors has been set, return that. Otherwise, 
		/// set and return.
		/// </summary>
		/// <returns></returns>
		public static List<SitecoreTree.StaffStruct> LazyReadAuthors()
		{
			return _authors ?? (_authors = GetAuthors());
		}

		/// <summary>
		/// [0] - display name
		/// [1] - path
		/// </summary>
		/// <returns></returns>
		public static string[] GetSupportingDocumentsRootNode()
		{
			var sctree = new SCTree();
			//return sctree.GetSupportingDocumentsRootNodePath(_sitecoreUser.Username, _sitecoreUser.Password).ToArray();
			return sctree.GetSDRootPath(_sitecoreUser.Username, _sitecoreUser.Password).ToArray();
		}

		/// <summary>
		/// [0] - display name
		/// [1] - path
		/// </summary>
		/// <returns></returns>
		public static string[] GetGraphicsRootNode()
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}GraphicsNode").Result;
				var mediaLibraryNode = response.Content.ReadAsAsync<string[]>().Result;
				return mediaLibraryNode;
			}
		}

		/// <summary>
		/// Force the list of authors to be updated and then returned. Otherwise, 
		/// set and return.
		/// </summary>
		/// <returns></returns>
		public static List<SitecoreTree.StaffStruct> ForceReadAuthors()
		{
			return _authors = GetAuthors();
		}

		public static ArticleStruct LazyReadArticleDetails(string articleNumber)
		{
			if (_articleDetails != null && _articleDetails.ArticleNumber == articleNumber)
			{
				return _articleDetails;
			}
			return _articleDetails = GetArticleDetails(articleNumber);
		}

		public static ArticleStruct ForceReadArticleDetails(string articleNumber)
		{
			return _articleDetails = GetArticleDetails(articleNumber);
		}

		public static ArticleStruct ForceReadArticleDetails(Guid articleGuid)
		{
			return _articleDetails = GetArticleDetails(articleGuid);
		}

		public static List<ItemStruct> GetPublications()
		{
			var sctree = new SCTree();

			return sctree.GetPublications(_sitecoreUser.Username, _sitecoreUser.Password).ToList();
		}

		public static List<ItemStruct> GetArticleCategories(Guid pubGuid)
		{
			var sctree = new SCTree();

			return sctree.GetArticleCategories(pubGuid, _sitecoreUser.Username, _sitecoreUser.Password).ToList();
		}

		public static List<ItemStruct> GetWebCategories(Guid pubGuid)
		{
			using (var sctree = new SCTree())
			{
				return sctree.GetWebCategories(pubGuid, _sitecoreUser.Username, _sitecoreUser.Password).ToList();
			}
		}

		public static string GetPublicationFrequency(Guid pubGuid)
		{
			var sctree = new SCTree();

			return sctree.GetPublicationFrequency(pubGuid, _sitecoreUser.Username, _sitecoreUser.Password);
		}

		public static List<ItemStruct> GetIssues(Guid pubGuid)
		{
			var sctree = new SCTree();

			return sctree.GetIssues(pubGuid, _sitecoreUser.Username, _sitecoreUser.Password).ToList();
		}

		public static DateTime GetIssueDate(Guid issueGuid)
		{
			var sctree = new SCTree();

			return sctree.GetIssueDate(issueGuid, _sitecoreUser.Username, _sitecoreUser.Password);
		}

		public static List<ArticleSize> GetArticleSizes(Guid publicationID)
		{
			var sctree = new SCTree();

			var sizes = sctree.GetArticleSizesForPublication(publicationID, _sitecoreUser.Username, _sitecoreUser.Password);
			if (sizes == null)
			{
				return new List<ArticleSize>();
			}

			return sizes.ToList();
		}

		private static List<SitecoreTree.StaffStruct> GetAuthors()
		{
			var sctree = new SCTree();

			return sctree.GetAuthors(_sitecoreUser.Username, _sitecoreUser.Password).ToList();
		}

		private static ArticleStruct GetArticleDetails(string articleNumber)
		{
			Globals.SitecoreAddin.Log("Getting article details from Sitecore...");
			var sctree = new SCTree();
			return sctree.GetArticleDetails(articleNumber, _sitecoreUser.Username, _sitecoreUser.Password);
		}

		private static ArticleStruct GetArticleDetails(Guid articleGuid)
		{
			Globals.SitecoreAddin.Log("Getting article details from Sitecore...");
			var sctree = new SCTree();
			return sctree.GetArticleDetailsBG(articleGuid, _sitecoreUser.Username, _sitecoreUser.Password);
		}

		public static string GetDynamicUrl(string path)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}GetDynamicUrl?path={path}").Result;
				return response.Content.ReadAsStringAsync().Result;
			}
		}

		public static int GetMaxLengthShortSummary()
		{
			var sctree = new SCTree();
			return sctree.GetMaxLengthShortSummary(_sitecoreUser.Username, _sitecoreUser.Password);
		}

		public static int GetMaxLengthLongSummary()
		{
			var sctree = new SCTree();
			return sctree.GetMaxLengthLongSummary(_sitecoreUser.Username, _sitecoreUser.Password);
		}

		public static bool IsAvailable()
		{
			var sctree = new SCTree();
			try
			{
				return sctree.IsAvailable();
			}
			catch (Exception)
			{

				return false;
			}
		}

		public static List<StaffStruct> GetStaffAndGroups()
		{
			var sctree = new SCTree();
			return sctree.GetStaffAndGroups(_sitecoreUser.Username, _sitecoreUser.Password).ToList();
		}

		public static DealInfo GetDealInfo(string recordNumber)
		{
			var sctree = new SCTree();
			return sctree.GetDealInfo(recordNumber, _sitecoreUser.Username, _sitecoreUser.Password);
		}

		public static int[] GetWidthHeightOfMediaItem(string path)
		{
			var sctree = new SCTree();
			return sctree.GetWidthHeightMI(path, _sitecoreUser.Username, _sitecoreUser.Password);
		}

		public static List<CompanyWrapper> GetAllCompanies()
		{
			var sctree = new SCTree();

			return sctree.GetAllCompanies(_sitecoreUser.Username, _sitecoreUser.Password).ToList();
		}

		public static IEnumerable<CompanyWrapper> GetAllCompaniesWithRelated()
		{
			var sctree = new SCTree();

			return sctree.GetAllCompaniesWithRelated(_sitecoreUser.Username, _sitecoreUser.Password);
		}

		public static List<WordStyleStruct> GetParagraphStyles()
		{
			var sctree = new SCTree();
			return sctree.GetParagraphStyles(_sitecoreUser.Username, _sitecoreUser.Password).ToList();
		}

		public static List<WordStyleStruct> GetCharacterStyles()
		{
			var sctree = new SCTree();
			return sctree.GetCharacterStyles(_sitecoreUser.Username, _sitecoreUser.Password).ToList();
		}

		public static DirectoryStruct[] GetChildrenDirectories(string path)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}GetChildrenDirectories?path={path}").Result;
				var directoryList = JsonConvert.DeserializeObject<DirectoryStruct[]>(response.Content.ReadAsStringAsync().Result);
				return directoryList;
			}
		}

		public static MediaItemStruct GetMediaLibraryItem(string path)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}GetMediaLibraryItem?path={path}").Result;
				var mediaItem = JsonConvert.DeserializeObject<MediaItemStruct>(response.Content.ReadAsStringAsync().Result);
				return mediaItem;
			}
		}

		public static byte[] GetMediaLibraryItemData(string path)
		{
			return GetMediaLibraryItem(path).Data;
		}

		public static string MediaPreviewUrl(string path)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}MediaPreviewUrl?path={path}").Result;
				return response.Content.ReadAsStringAsync().Result;
			}
		}

		public static bool IsContinuousPublishingPublication(Guid publication)
		{
			var tree = new SCTree();
			return tree.IsContinuousPublishingPublication(publication, _sitecoreUser.Username, _sitecoreUser.Password);
		}
	}
}
