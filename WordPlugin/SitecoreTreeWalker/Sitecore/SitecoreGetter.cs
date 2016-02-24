using System;
using System.Collections.Generic;
using System.Net.Http;
using Informa.Web.Areas.Account.Models;
using Newtonsoft.Json;

namespace SitecoreTreeWalker.Sitecore
{
	class SitecoreGetter
	{
		private static List<WordPluginModel.StaffStruct> _authors;
		private static WordPluginModel.ArticleStruct _articleDetails = new WordPluginModel.ArticleStruct();		
	

		/// <summary>
		/// This method returns all the Taxonomy Items found based on the search term which is passed.
		/// </summary>
		/// <param name="term"></param>
		/// <returns></returns>
		public static List<WordPluginModel.TaxonomyStruct> SearchTaxonomy(string term)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}SearchTaxonomy?searchTerm={term}").Result;
				var taxonomy = JsonConvert.DeserializeObject<List<WordPluginModel.TaxonomyStruct>>(response.Content.ReadAsStringAsync().Result);
				return taxonomy;
			}
		}

		public static WordPluginModel.HDirectoryStruct GetHierarchyByGuid(Guid taxonomyGuid)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetHierarchyByGuid?guid={taxonomyGuid}").Result;
				var directoryList = JsonConvert.DeserializeObject<WordPluginModel.HDirectoryStruct>(response.Content.ReadAsStringAsync().Result);
				return directoryList;
			}
		}


		public static WordPluginModel.MediaItemStruct GetMediaStatistics(string path)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetMediaStatistics?path={path}").Result;
				var mediaItem = JsonConvert.DeserializeObject<WordPluginModel.MediaItemStruct>(response.Content.ReadAsStringAsync().Result);
				return mediaItem;
			}
		}

		/// <summary>
		/// If list of authors has been set, return that. Otherwise, 
		/// set and return.
		/// </summary>
		/// <returns></returns>
		public static List<WordPluginModel.StaffStruct> LazyReadAuthors()
		{
			return _authors ?? (_authors = GetAuthors());
		}

		/// <summary>
		/// This method returns the Supporting Document Base node from Media Library.
		/// </summary>
		/// <returns></returns>
		public static string[] GetSupportingDocumentsRootNode()
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}SupportingDocumentsNode").Result;
				var supportingDocumentsNode = response.Content.ReadAsAsync<string[]>().Result;
				return supportingDocumentsNode;
			}
		}

		/// <summary>
		/// The method returns the Media Library Base node items.
		/// </summary>
		/// <returns></returns>
		public static string[] GetGraphicsRootNode()
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GraphicsNode").Result;
				var mediaLibraryNode = response.Content.ReadAsAsync<string[]>().Result;
				return mediaLibraryNode;
			}
		}

		/// <summary>
		/// Force the list of authors to be updated and then returned. Otherwise, 
		/// set and return.
		/// </summary>
		/// <returns></returns>
		public static List<WordPluginModel.StaffStruct> ForceReadAuthors()
		{
			return _authors = GetAuthors();
		}

		public static WordPluginModel.ArticleStruct LazyReadArticleDetails(string articleNumber)
		{
			if (_articleDetails != null && _articleDetails.ArticleNumber == articleNumber)
			{
				return _articleDetails;
			}
			return _articleDetails = GetArticleDetails(articleNumber);
		}

		public static WordPluginModel.ArticleStruct ForceReadArticleDetails(string articleNumber)
		{
			return _articleDetails = GetArticleDetails(articleNumber);
		}

		public static WordPluginModel.ArticleStruct ForceReadArticleDetails(Guid articleGuid)
		{
			return _articleDetails = GetArticleDetails(articleGuid);
		}

		/// <summary>
		/// This method gets the list of all the publications.
		/// </summary>
		/// <returns></returns>
		public static List<WordPluginModel.ItemStruct> GetPublications()
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetPublications").Result;
				var publicationsList = JsonConvert.DeserializeObject<List<WordPluginModel.ItemStruct>>(response.Content.ReadAsStringAsync().Result);
				return publicationsList;
			}
		}

		/// <summary>
		/// This method gets all the media types.
		/// </summary>
		/// <returns></returns>
		public static List<WordPluginModel.ItemStruct> GetMediaTypes()
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetMediaTypes").Result;
				var mediaItem = JsonConvert.DeserializeObject<List<WordPluginModel.ItemStruct>>(response.Content.ReadAsStringAsync().Result);
				return mediaItem;
			}
		}

		/// <summary>
		/// This methd gets al the Content Types.
		/// </summary>
		/// <returns></returns>
		public static List<WordPluginModel.ItemStruct> GetContentTypes()
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetContentTypes").Result;
				var mediaItem = JsonConvert.DeserializeObject<List<WordPluginModel.ItemStruct>>(response.Content.ReadAsStringAsync().Result);
				return mediaItem;
			}
		}

		/// <summary>
		/// Gets all the authors. TODO - This mayneed tweak when we have authors from multiple publications.
		/// </summary>
		/// <returns></returns>
		private static List<WordPluginModel.StaffStruct> GetAuthors()
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetAuthors").Result;
				var mediaItem = JsonConvert.DeserializeObject<List<WordPluginModel.StaffStruct>>(response.Content.ReadAsStringAsync().Result);
				return mediaItem;
			}
		}

		/// <summary>
		/// This is one of the key methods, where you get all the details about the article on passing the Article Number.
		/// </summary>
		/// <param name="articleNumber"></param>
		/// <returns></returns>
		private static WordPluginModel.ArticleStruct GetArticleDetails(string articleNumber)
		{
			Globals.SitecoreAddin.Log("Getting article details from Sitecore...");
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetArticleDetails?articleNumber={articleNumber}").Result;
				var articleStruct = JsonConvert.DeserializeObject<WordPluginModel.ArticleStruct>(response.Content.ReadAsStringAsync().Result);
				return articleStruct;
			}
		}

		/// <summary>
		/// This is one of the key methods, where you get all the details about the article on passing the Article Guid.
		/// </summary>
		/// <param name="articleGuid"></param>
		/// <returns></returns>
		private static WordPluginModel.ArticleStruct GetArticleDetails(Guid articleGuid)
		{
			Globals.SitecoreAddin.Log("Getting article details from Sitecore...");
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetArticleDetailsBg?articleGuid={articleGuid}").Result;
				var articleStruct = JsonConvert.DeserializeObject<WordPluginModel.ArticleStruct>(response.Content.ReadAsStringAsync().Result);
				return articleStruct;
			}
		}

		public static string GetDynamicUrl(string path)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetDynamicUrl?path={path}").Result;
				return response.Content.ReadAsStringAsync().Result;
			}
		}
		
		/// <summary>
		///  Get's the Max lenght of the Summary
		/// </summary>
		/// <returns></returns>
		public static int GetMaxLengthShortSummary()
		{			
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetMaxLengthShortSummary").Result;
				var length = JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
				return length;
			}
		}

		/// <summary>
		///  Get's the Max lenght of the Summary
		/// </summary>
		/// <returns></returns>
		public static int GetMaxLengthLongSummary()
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetMaxLengthLongSummary").Result;
				var length =  JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
				return length;
			}
		}

		/// <summary>
		/// The method checks if the API service is avialable or not.
		/// </summary>
		/// <returns></returns>
		public static bool IsAvailable()
		{
			try
			{
				using (var client = new HttpClient())
				{
					var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}IsAvailable").Result;
					var isAvailable = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
					return isAvailable;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// This method returns a list of all the Staff.
		/// </summary>
		/// <returns></returns>
		public static List<WordPluginModel.StaffStruct> GetStaffAndGroups()
		{
			using (var client = new HttpClient())
			{
				//TODO - This might change due to change in Staff and Authors
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetAuthors").Result;
				var mediaItem = JsonConvert.DeserializeObject<List<WordPluginModel.StaffStruct>>(response.Content.ReadAsStringAsync().Result);
				return mediaItem;
			}
		}

		//TODO - GetDealInfo
		public static WordPluginModel.DealInfo GetDealInfo(string recordNumber)
		{
			//return sctree.GetDealInfo(recordNumber, _sitecoreUser.Username, _sitecoreUser.Password);
			return new WordPluginModel.DealInfo();
		}

		public static int[] GetWidthHeightOfMediaItem(string path)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetWidthHeightOfMediaItem?path={path}").Result;
				var mediaItem = JsonConvert.DeserializeObject<List<int>>(response.Content.ReadAsStringAsync().Result);
				return mediaItem.ToArray();
			}

		}

		//TODO - GetAllCompanies
		public static List<WordPluginModel.CompanyWrapper> GetAllCompanies()
		{
			//return sctree.GetAllCompanies(_sitecoreUser.Username, _sitecoreUser.Password).ToList();
			return new List<WordPluginModel.CompanyWrapper>();
		}
		
		//TODO - GetAllRelatedCompanies
		public static IEnumerable<WordPluginModel.CompanyWrapper> GetAllCompaniesWithRelated()
		{
			//return sctree.GetAllCompaniesWithRelated(_sitecoreUser.Username, _sitecoreUser.Password);
			return new List<WordPluginModel.CompanyWrapper>();
		}

		/// <summary>
		/// The method gets all the Paragraph Styles from Sitecore
		/// </summary>
		/// <returns></returns>
		public static List<WordPluginModel.WordStyleStruct> GetParagraphStyles()
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetParagraphStyles").Result;
				var mediaItem = JsonConvert.DeserializeObject<List<WordPluginModel.WordStyleStruct>>(response.Content.ReadAsStringAsync().Result);
				return mediaItem;
			}
		}

		/// <summary>
		/// The method gets all the Character Styles from Sitecore
		/// </summary>
		/// <returns></returns>
		public static List<WordPluginModel.WordStyleStruct> GetCharacterStyles()
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetCharacterStyles").Result;
				var mediaItem = JsonConvert.DeserializeObject<List<WordPluginModel.WordStyleStruct>>(response.Content.ReadAsStringAsync().Result);
				return mediaItem;
			}
		}

		/// <summary>
		/// This method returns the Directory Structure of the Path that is passed to Sitecore.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static WordPluginModel.DirectoryStruct[] GetChildrenDirectories(string path)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetChildrenDirectories?path={path}").Result;
				var directoryList = JsonConvert.DeserializeObject<WordPluginModel.DirectoryStruct[]>(response.Content.ReadAsStringAsync().Result);
				return directoryList;
			}
		}

		/// <summary>
		/// This method returns the MediaItem Information based on the GUID or Path passed to the method.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static WordPluginModel.MediaItemStruct GetMediaLibraryItem(string path)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetMediaLibraryItem?path={path}").Result;
				var mediaItem = JsonConvert.DeserializeObject<WordPluginModel.MediaItemStruct>(response.Content.ReadAsStringAsync().Result);
				return mediaItem;
			}
		}
		
		/// <summary>
		/// The method returns the GUID of the item when passed in the path.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static Guid GetItemGuidByPath(string path)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetItemGuidByPath?path={path}").Result;
				var itemGuid = JsonConvert.DeserializeObject<Guid>(response.Content.ReadAsStringAsync().Result);
				return itemGuid;
			}
		}

		/// <summary>
		/// This Method get the Media Item in form of Bytes when you give in a GUID / Path of the item.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static byte[] GetMediaLibraryItemData(string path)
		{
			return GetMediaLibraryItem(path).Data;
		}

		/// <summary>
		/// This Method returns the Preview URL of the MediaItem.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string MediaPreviewUrl(string path)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}MediaPreviewUrl?path={path}").Result;
				return response.Content.ReadAsStringAsync().Result;
			}
		}
	}
}
