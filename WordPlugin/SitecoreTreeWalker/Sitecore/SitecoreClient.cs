using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Script.Serialization;
using InformaSitecoreWord.Custom_Exceptions;
using InformaSitecoreWord.document;
using InformaSitecoreWord.User;
using InformaSitecoreWord.Util;
using InformaSitecoreWord.Util.Document;
using InformaSitecoreWord.WebserviceHelper;
using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using PluginModels;
using InformaSitecoreWord.Config;
using Newtonsoft.Json.Converters;

/// <summary>
namespace InformaSitecoreWord.Sitecore
{
    class SitecoreClient
    {
        private static List<StaffStruct> _authors;
        private static ArticleStruct _articleDetails = new ArticleStruct();
        protected static SitecoreUser _sitecoreUser = SitecoreUser.GetUser();
        private static WebRequestHandler _handler = new WebRequestHandler { CookieContainer = new CookieContainer(), UseCookies = true };
        //private static WebRequestHandler _handler = new WebRequestHandler { CookieContainer = new CookieContainer(), UseCookies = true, Credentials = new NetworkCredential(@"dev\tanasuk", "DjnznZZ21!") };
        private static JsonSerializer _serializer
        {
            get
            {
                var ser = new JsonSerializer();
                ser.Converters.Add(new IsoDateTimeConverter());
                return ser;
            }
        }

        private static readonly UserCredentialReader _reader = UserCredentialReader.GetReader();

        public SitecoreClient()
        {
            if (_sitecoreUser.Username != null && _handler.CookieContainer.GetCookies(new Uri(Constants.EDITOR_ENVIRONMENT_SERVERURL)) == null)
            {
                _sitecoreUser.Authenticate(_sitecoreUser.Username, _sitecoreUser.Password);
                //var cookie = UserCredentialReader.GetReader().GetCookie(_sitecoreUser.Username);
                //_handler.CookieContainer.Add(cookie);
            }

        }

        public bool IsUserAuthorized()
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}CreateArticle").Result;

                return response.IsSuccessStatusCode;
            }
        }

        public static List<TaxonomyStruct> SearchTaxonomy(string term,Guid verticalTaxonomyGuid = default(Guid))
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}SearchTaxonomy?searchTerm={term}&verticalTaxonomyGuid={verticalTaxonomyGuid}").Result;
                var taxonomy = JsonConvert.DeserializeObject<List<TaxonomyStruct>>(response.Content.ReadAsStringAsync().Result);
                return taxonomy??new List<TaxonomyStruct>();
            }
        }

        public static HDirectoryStruct GetHierarchyByGuid(Guid taxonomyGuid= default(Guid))
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetHierarchyByGuid?guid={taxonomyGuid}").Result;
                var directoryList = JsonConvert.DeserializeObject<HDirectoryStruct>(response.Content.ReadAsStringAsync().Result);
                return directoryList;
            }
        }

        public static MediaItemStruct GetMediaStatistics(string path)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetMediaStatistics?path={path}").Result;
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
            //return _authors ?? (_authors = GetAuthors());
            return GetAuthors();
        }

        /// <summary>
        /// [0] - display name
        /// [1] - path
        /// </summary>
        /// <returns></returns>
        public static string[] GetSupportingDocumentsRootNode()
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}SupportingDocumentsNode").Result;
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
            using (var client = new HttpClient(_handler, false))
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
        
        public static List<VerticalStruct> GetVerticals()
        {
            Globals.SitecoreAddin.Log("Getting vertical details from Sitecore...");
            using (var client = new HttpClient(_handler, false))
            {

                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetVerticals").Result;
                var verticalList = JsonConvert.DeserializeObject<List<VerticalStruct>>(response.Content.ReadAsStringAsync().Result);

                return verticalList;
            }
        }

        public static List<ItemStruct> GetPublications()
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetPublications").Result;
                var publicationsList = JsonConvert.DeserializeObject<List<ItemStruct>>(response.Content.ReadAsStringAsync().Result);

                return publicationsList;
            }
        }

        public static List<ItemStruct> GetMediaTypes()
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetMediaTypes").Result;
                var mediaItem = JsonConvert.DeserializeObject<List<ItemStruct>>(response.Content.ReadAsStringAsync().Result);
                return mediaItem;
            }
        }

        public static List<ItemStruct> GetContentTypes()
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetContentTypes").Result;
                var mediaItem = JsonConvert.DeserializeObject<List<ItemStruct>>(response.Content.ReadAsStringAsync().Result);
                return mediaItem;
            }
        }

        public static List<ArticleSize> GetArticleSizes(Guid publicationID)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetArticleSizesForPublication?publicationID={publicationID}").Result;
                var articleSizes = JsonConvert.DeserializeObject<List<ArticleSize>>(response.Content.ReadAsStringAsync().Result);
                return articleSizes;
            }
        }

        private static List<StaffStruct> GetAuthors()
        {
            Guid veticalGuid = default(Guid);
            veticalGuid = PluginSingletonVerticalRoot.Instance.CurrentVertical.ID;

            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetAuthors?veticalGuid={veticalGuid}").Result;
                var mediaItem = JsonConvert.DeserializeObject<List<StaffStruct>>(response.Content.ReadAsStringAsync().Result);
                return mediaItem;
            }
        }

        private static ArticleStruct GetArticleDetails(string articleNumber)
        {
            Globals.SitecoreAddin.Log("Getting article details from Sitecore...");
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetArticleDetails?articleNumber={articleNumber}").Result;
                var articleStruct = JsonConvert.DeserializeObject<ArticleStruct>(response.Content.ReadAsStringAsync().Result);

                TimezoneUtil.ConvertArticleDatesToLocalTimezone(articleStruct);
                return articleStruct;
            }
            /*
			var sctree = new SCTree();
			return sctree.GetArticleDetails(articleNumber, _sitecoreUser.Username, _sitecoreUser.Password);
			*/
        }

        private static ArticleStruct GetArticleDetails(Guid articleGuid)
        {
            Globals.SitecoreAddin.Log("Getting article details from Sitecore...");
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetArticleDetailsBg?articleGuid={articleGuid}").Result;
                var articleStruct = JsonConvert.DeserializeObject<ArticleStruct>(response.Content.ReadAsStringAsync().Result);
                TimezoneUtil.ConvertArticleDatesToLocalTimezone(articleStruct);
                return articleStruct;
            }
            /*
			var sctree = new SCTree();
			return sctree.GetArticleDetailsBG(articleGuid, _sitecoreUser.Username, _sitecoreUser.Password);
			*/
        }

        public static string GetDynamicUrl(string path)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetDynamicUrl?path={path}").Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public static int GetMaxLengthShortSummary()
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetMaxLengthShortSummary").Result;
                var length = JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
                return length;
                /*
				Int32.TryParse(response.Content.ReadAsStringAsync().Result, out length);
				return length;
				*/
            }
        }

        public static int GetMaxLengthLongSummary()
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetMaxLengthLongSummary").Result;
                var length = JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
                return length;
            }
        }

        public static string GetContactEmail()
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetContactEmail").Result;
                var email = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                return email;
            }
        }

        public static List<string> GetFullNameAndEmail(string userName)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetUserInfo?username={userName}").Result;
                var userInfo = JsonConvert.DeserializeObject<List<string>>(response.Content.ReadAsStringAsync().Result);
                return userInfo;
            }
        }

        public static bool IsAvailable()
        {
            try
            {
                using (var client = new HttpClient(_handler, false))
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

        public static List<StaffStruct> GetStaffAndGroups()
        {
            Guid veticalGuid = default(Guid);
            veticalGuid = PluginSingletonVerticalRoot.Instance.CurrentVertical.ID;

            using (var client = new HttpClient(_handler, false))
            {
                //TODO - This might change due to change in Staff and Authors
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetAuthors?veticalGuid={veticalGuid}").Result;
                var mediaItem = JsonConvert.DeserializeObject<List<StaffStruct>>(response.Content.ReadAsStringAsync().Result);
                return mediaItem;
            }
        }

        public static DealInfo GetDealInfo(string recordNumber)
        {
            using (var client = new HttpClient(_handler, false))
            {
                //var response = client.GetAsync($"http://dev.ibi.velir.com/api/GetDealInfo?recordNumber={recordNumber}").Result;
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetDealInfo?recordNumber={recordNumber}").Result;
                var dealInfo = JsonConvert.DeserializeObject<DealInfo>(response.Content.ReadAsStringAsync().Result);
                return dealInfo;
            }
        }

        public static int[] GetWidthHeightOfMediaItem(string path)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetWidthHeightOfMediaItem?path={path}").Result;
                var mediaItem = JsonConvert.DeserializeObject<List<int>>(response.Content.ReadAsStringAsync().Result);
                return mediaItem.ToArray();
            }

        }
        public static List<CompanyWrapper> GetAllCompanies()
        {
            using (var client = new HttpClient(_handler, false))
            {
                //var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetAllCompanies").Result;
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetAllCompanies").Result;
                var lstComp = JsonConvert.DeserializeObject<List<CompanyWrapper>>(response.Content.ReadAsStringAsync().Result);
                return lstComp;
            }
        }

        public static IEnumerable<CompanyWrapper> GetAllCompaniesWithRelated()
        {
            using (var client = new HttpClient(_handler, false))
            {
                //var response = client.GetAsync("http://dev.ibi.velir.com/api/GetAllCompaniesWithRelated").Result;
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetAllCompaniesWithRelated").Result;
                var lstComp = JsonConvert.DeserializeObject<List<CompanyWrapper>>(response.Content.ReadAsStringAsync().Result);
                return lstComp;
            }
        }

        public static List<WordStyleStruct> GetParagraphStyles()
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetParagraphStyles").Result;
                var mediaItem = JsonConvert.DeserializeObject<List<WordStyleStruct>>(response.Content.ReadAsStringAsync().Result);
                return mediaItem;
            }
        }

        public static List<WordStyleStruct> GetCharacterStyles()
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetCharacterStyles").Result;
                var mediaItem = JsonConvert.DeserializeObject<List<WordStyleStruct>>(response.Content.ReadAsStringAsync().Result);
                return mediaItem;
            }
        }

        public static DirectoryStruct[] GetChildrenDirectories(string path)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetChildrenDirectories?path={path}").Result;
                var directoryList = JsonConvert.DeserializeObject<DirectoryStruct[]>(response.Content.ReadAsStringAsync().Result);
                return directoryList;
            }
        }

        public static MediaItemStruct GetMediaLibraryItem(string path)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetMediaLibraryItem?path={path}").Result;
                var mediaItem = JsonConvert.DeserializeObject<MediaItemStruct>(response.Content.ReadAsStringAsync().Result);
                return mediaItem;
            }
        }

        public static Guid GetItemGuidByPath(string path)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetItemGuidByPath?path={path}").Result;
                var itemGuid = JsonConvert.DeserializeObject<Guid>(response.Content.ReadAsStringAsync().Result);
                return itemGuid;
            }
        }

        public static byte[] GetMediaLibraryItemData(string path)
        {
            return GetMediaLibraryItem(path).Data;
        }

        public static string MediaPreviewUrl(string path)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}MediaPreviewUrl?path={path}").Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public ArticleStruct SaveStubToSitecore(string articleName, string publicationDate, Guid publicationID)
        {
            using (var client = new HttpClient(_handler, false))
            {
                publicationDate = TimezoneUtil.ConvertDateToServerTimezone(publicationDate);
                var article = new CreateArticleRequest() { Name = articleName, PublicationID = publicationID, PublicationDate = publicationDate };
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}CreateArticle", article).Result;

                if (response?.StatusCode != HttpStatusCode.OK)
                {//If the response is not OK, place the status code into the remote error message for logging 
                    return new ArticleStruct { RemoteErrorMessage = response?.StatusCode.ToString() };
                }
                else
                {
                    var articleItem = JsonConvert.DeserializeObject<ArticleStruct>(response.Content.ReadAsStringAsync().Result, new IsoDateTimeConverter());
                    TimezoneUtil.ConvertArticleDatesToLocalTimezone(articleItem);
                    return articleItem;
                }
            }
        }

        public static bool DoesArticleNameAlreadyExistInIssue(ArticleStruct articleDetails)
        {
            TimezoneUtil.ConvertArticleDatesToServerTimezone(articleDetails);
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}DoesArticleNameAlreadyExistInIssue").Result;
                var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
                TimezoneUtil.ConvertArticleDatesToLocalTimezone(articleDetails);
                return flag;
            }
        }

        public static void SaveMetadataToSitecore(string articleID, ArticleStruct articleData)
        {
            TimezoneUtil.ConvertArticleDatesToServerTimezone(articleData);
            using (var client = new HttpClient(_handler, false))
            {
                var article = new SaveArticleDetails() { ArticleNumber = articleID, ArticleData = articleData };
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}SaveArticleDetails", article).Result;
                TimezoneUtil.ConvertArticleDatesToLocalTimezone(articleData);
            }
        }

        public static void SaveArticleDetailsByGuid(Guid articleGuid, ArticleStruct articleData)
        {
            TimezoneUtil.ConvertArticleDatesToServerTimezone(articleData);
            using (var client = new HttpClient(_handler, false))
            {
                var article = new SaveArticleDetailsByGuid() { ArticleGuid = articleGuid, ArticleData = articleData };
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}SaveArticleDetailsByGuid", article).Result;
                TimezoneUtil.ConvertArticleDatesToLocalTimezone(articleData);
            }
        }

        public static string GetArticleUrl(string articleNumber)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetArticleUrl", articleNumber).Result;
                var url = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                return url;
            }
        }

        public static string GetArticlePreviewUrl(string articleNumber)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}PreviewUrlArticle", articleNumber).Result;
                var url = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                return url;
            }
        }

        public static string GetArticleDynamicUrl(string articleNumber)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetArticleDynamicUrl", articleNumber).Result;
                var url = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                return url;
            }
        }

        public static bool DoesArticleHaveText(string articleNumber)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}DoesArticleHaveText", articleNumber).Result;
                var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
                return flag;
            }
        }

        public static bool DoesArticleHaveText(Guid articleGuid)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}DoesArticleGuidHaveText", articleGuid).Result;
                var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
                return flag;
            }
        }

        public static bool DoesArticleExist(string articleNumber)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}DoesArticleExist", articleNumber).Result;
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException(response.StatusCode.ToString() + ": Connection Timeout");

                var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
                return flag;
            }
        }

        public static bool DoesArticleExist(Guid articleGuid)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}DoesArticleGuidExist", articleGuid).Result;
                var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
                return flag;
            }
        }

        public static CheckoutStatus GetLockedStatus(string articleNumber)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetLockedStatus", articleNumber).Result;
                return JsonConvert.DeserializeObject<CheckoutStatus>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public static CheckoutStatus GetLockedStatus(Guid articleGuid)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetLockedStatusByGuid", articleGuid).Result;
                return JsonConvert.DeserializeObject<CheckoutStatus>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public static bool CheckOutArticle(string articleNumber, string username)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}CheckOutArticle", articleNumber).Result;
                var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
                return flag;
            }
        }

        public static bool CheckOutArticle(Guid articleGuid, string username)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}CheckOutArticleByGuid", articleGuid).Result;
                var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
                return flag;
            }
        }

        public static bool CheckInArticle(string articleNumber)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}CheckInArticle", articleNumber).Result;
                var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
                return flag;
            }
        }

        public static bool CheckInArticle(Guid articleGuid)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}CheckInArticleByGuid", articleGuid).Result;
                var flag = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
                return flag;
            }
        }

        public static string GetArticleGuidByArticleNumber(string articleNumber)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetArticleGuidByNum", articleNumber).Result;
                var articleGuid = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result, new IsoDateTimeConverter());
                return articleGuid;
            }
        }

        public static ArticlePreviewInfo GetArticlePreviewInfo(string articleNumber)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetArticlePreviewInfo", articleNumber).Result;
                var previewInfo = JsonConvert.DeserializeObject<ArticlePreviewInfo>(response.Content.ReadAsStringAsync().Result, new IsoDateTimeConverter());
                TimezoneUtil.ConvertArticleDatesToLocalTimezone(previewInfo);
                return previewInfo;
            }
        }

        public static List<ArticlePreviewInfo> GetArticlePreviewInfo(List<Guid> guids)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetArticlePreviewInfoByGuids", guids).Result;
                var articlePreviewCollection = JsonConvert.DeserializeObject<List<ArticlePreviewInfo>>(response.Content.ReadAsStringAsync().Result, new IsoDateTimeConverter());
                if (articlePreviewCollection != null)
                    articlePreviewCollection.ForEach(f => TimezoneUtil.ConvertArticleDatesToLocalTimezone(f));
                return articlePreviewCollection;
            }
        }

        public static void SaveArticleText(string articleNumber, string text, ArticleStruct articleStruct)
        {
            TimezoneUtil.ConvertArticleDatesToServerTimezone(articleStruct);
            using (var client = new HttpClient(_handler, false))
            {
                var article = new SaveArticleText() { ArticleNumber = articleNumber, ArticleData = articleStruct, WordText = text };
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}SaveArticleText", article).Result;
                TimezoneUtil.ConvertArticleDatesToLocalTimezone(articleStruct);
            }
        }

        public static void SaveArticleText(Guid articleGuid, string text, ArticleStruct articleStruct)
        {
            TimezoneUtil.ConvertArticleDatesToServerTimezone(articleStruct);
            using (var client = new HttpClient(_handler, false))
            {
                var article = new SaveArticleTextByGuid() { ArticleGuid = articleGuid, ArticleData = articleStruct, WordText = text };
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}SaveArticleTextByGuid", article).Result;
                TimezoneUtil.ConvertArticleDatesToLocalTimezone(articleStruct);
            }
        }

        public static int GetWordVersionNumber(string articleNumber)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetWordVersionNumByNumber", articleNumber).Result;
                var versionNumber = JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
                return versionNumber;
            }
        }

        public static int GetWordVersionNumber(Guid articleguid)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetWordVersionNumByGuid", articleguid).Result;
                var versionNumber = JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
                return versionNumber;
            }
        }

        public int SendDocumentToSitecore(string articleNumber, byte[] data, string extension, string username)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}SendDocumentToSitecore", new SendDocumentToSitecore() { ArticleNumber = articleNumber, Data = data, Extension = extension }).Result;
                return JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public int SendDocumentToSitecoreByGuid(Guid articleGuid, byte[] data, string extension, string username)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}SendDocumentToSitecoreByGuid", new SendDocumentToSitecoreByGuid() { ArticlGuid = articleGuid, Data = data, Extension = extension }).Result;
                return JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public static UserStatusStruct AuthenticateUser(string username, string password)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}AuthenticateUser", new LoginModel() { Username = username, Password = password }).Result;
                //var userStatus = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                var userStatus = JsonConvert.DeserializeObject<UserStatusStruct>(response.Content.ReadAsStringAsync().Result);

                var cookies = _handler.CookieContainer.GetCookies(new Uri(Constants.EDITOR_ENVIRONMENT_SERVERURL));

                if (cookies != null && cookies.Count > 0 && string.Equals(".ASPXAUTH", cookies[0].Name))
                    _reader.WriteCookie(cookies, username);


                return userStatus;
            }
        }

        public static ArticleWorkflowState GetWorkflowState(string articleNumber)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response =
                    client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}Workflow?articleNumber=" + articleNumber).Result;
                var workflowState =
                    JsonConvert.DeserializeObject<ArticleWorkflowState>(response.Content.ReadAsStringAsync().Result);

                return workflowState;
            }
        }

        public static ArticleWorkflowState GetWorkflowState(Guid articleGuid)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response =
                    client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}Workflow?articleGuid=" + articleGuid).Result;
                var workflowState =
                    JsonConvert.DeserializeObject<ArticleWorkflowState>(response.Content.ReadAsStringAsync().Result);

                return workflowState;
            }
        }

        public static string GetDocumentPassword()
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}" + "/api/"}GetDocumentPassword").Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public List<string> SaveArticle(Document activeDocument, ArticleStruct articleDetails, Guid workflowCommand, List<StaffStruct> notifications, string articleNumber, string body = null, string notificationText = null)
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
                articleDetails.NotificationText = notificationText;
                articleDetails.WordCount = activeDocument.ComputeStatistics(0);
                articleDetails.ReferencedDeals = ReferencedDealParser.GetReferencedDeals(activeDocument).ToList();
                articleDetails.CommandID = workflowCommand;
                articleDetails.SupportingDocumentPaths = _wordUtils.GetSupportingDocumentPaths().ToList();
                if (articleDetails.RelatedArticles == null)
                    articleDetails.RelatedArticles = new List<Guid>();
                if (articleDetails.RelatedInlineArticles == null)
                    articleDetails.RelatedInlineArticles = new List<Guid>();

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
                    extension = ".docx";
                }
            }
            return extension;
        }

        protected WordUtils _wordUtils = new WordUtils();
        protected static StructConverter _structConverter = new StructConverter();

        public static string ReExportArticleNlm(string articleNumber)
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.PostAsJsonAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}ReExportNlm", articleNumber).Result;
                string result = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                return result;
            }
        }

        public static TimeZoneInfo GetServerTimezone()
        {
            using (var client = new HttpClient(_handler, false))
            {
                var response = client.GetAsync($"{$"{Constants.EDITOR_ENVIRONMENT_SERVERURL}/api/"}GetServerTimezone").Result;
                return JsonConvert.DeserializeObject<TimeZoneInfo>(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
