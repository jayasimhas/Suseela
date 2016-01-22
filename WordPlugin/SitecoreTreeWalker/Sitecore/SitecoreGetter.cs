using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using SitecoreTreeWalker.User;
using System.Web.Script.Serialization;
using Informa.Web.Areas.Account.Models;
using Newtonsoft.Json;
using SitecoreTreeWalker.Config;
using SitecoreTreeWalker.SitecoreTree;

/// <summary>
namespace SitecoreTreeWalker.Sitecore
{
	class SitecoreGetter
	{
		private static List<StaffStruct> _authors;
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
		public static List<StaffStruct> LazyReadAuthors()
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
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}SupportingDocumentsNode").Result;
				var supportingDocumentsNode = response.Content.ReadAsAsync<string[]>().Result;
				return supportingDocumentsNode;
			}
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
		public static List<StaffStruct> ForceReadAuthors()
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
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}GetPublications").Result;
				var publicationsList = JsonConvert.DeserializeObject<List<ItemStruct>>(response.Content.ReadAsStringAsync().Result);
				return publicationsList;
			}
		}

		public static List<ItemStruct> GetMediaTypes()
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}GetMediaTypes").Result;
				var mediaItem = JsonConvert.DeserializeObject<List<ItemStruct>>(response.Content.ReadAsStringAsync().Result);
				return mediaItem;
			}
		}

		public static List<WordPluginModel.ArticleSize> GetArticleSizes(Guid publicationID)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}GetArticleSizesForPublication?publicationID={publicationID}").Result;
				var articleSizes = JsonConvert.DeserializeObject<List<WordPluginModel.ArticleSize>>(response.Content.ReadAsStringAsync().Result);
				return articleSizes;
			}
		}

		private static List<StaffStruct> GetAuthors()
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}GetAuthors").Result;
				var mediaItem = JsonConvert.DeserializeObject<List<StaffStruct>>(response.Content.ReadAsStringAsync().Result);
				return mediaItem;
			}
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
			return 1000;
			using (var client = new HttpClient())
			{
				int length;
				var response = client.GetAsync($"{webApiURL}WordPlugin/GetMaxLengthShortSummary").Result;
				return JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
				Int32.TryParse(response.Content.ReadAsStringAsync().Result, out length);
				return length;
			}
		}

		public static int GetMaxLengthLongSummary()
		{
			return 1500;
			using (var client = new HttpClient())
			{
				int length;
				var response = client.GetAsync($"{webApiURL}WordPlugin/GetMaxLengthLongSummary").Result;
				return JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
				Int32.TryParse(response.Content.ReadAsStringAsync().Result, out length);
				return length;
			}
			var sctree = new SCTree();
			return sctree.GetMaxLengthLongSummary(_sitecoreUser.Username, _sitecoreUser.Password);
		}


		public static bool IsAvailable()
		{
			try
			{
				using (var client = new HttpClient())
				{
					var response = client.GetAsync($"{webApiURL}IsAvailable").Result;
					var isAvailable = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
					return isAvailable;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static List<StaffStruct> GetStaffAndGroups()
		{
			using (var client = new HttpClient())
			{
				//TODO - This might change due to change in Staff and Authors
				var response = client.GetAsync($"{webApiURL}GetAuthors").Result;
				var mediaItem = JsonConvert.DeserializeObject<List<StaffStruct>>(response.Content.ReadAsStringAsync().Result);
				return mediaItem;
			}
		}

		public static DealInfo GetDealInfo(string recordNumber)
		{
			var sctree = new SCTree();
			return sctree.GetDealInfo(recordNumber, _sitecoreUser.Username, _sitecoreUser.Password);
		}

		public static int[] GetWidthHeightOfMediaItem(string path)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}GetWidthHeightOfMediaItem/{path}").Result;
				var mediaItem = JsonConvert.DeserializeObject<List<int>>(response.Content.ReadAsStringAsync().Result);
				return mediaItem.ToArray();
			}

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
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}GetParagraphStyles").Result;
				var mediaItem = JsonConvert.DeserializeObject<List<WordStyleStruct>>(response.Content.ReadAsStringAsync().Result);
				return mediaItem;
			}
		}

		public static List<WordStyleStruct> GetCharacterStyles()
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{webApiURL}GetCharacterStyles").Result;
				var mediaItem = JsonConvert.DeserializeObject<List<WordStyleStruct>>(response.Content.ReadAsStringAsync().Result);
				return mediaItem;
			}
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
	}
}
