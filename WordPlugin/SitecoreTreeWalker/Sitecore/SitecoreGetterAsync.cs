//using System;
//using System.Collections.Generic;
//using System.Linq;
//using SitecoreTreeWalker.SitecoreTree;

//namespace SitecoreTreeWalker.Sitecore
//{
//    class SitecoreClient
//    {
//        public static List<TaxonomyStruct> SearchTaxonomy(Guid taxonomyGuid, string term)
//        {
//            var sctree = new SCTree();

//            return sctree.SearchTaxonomy(taxonomyGuid, "", SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password).ToList();
//        }

//        public static HDirectoryStruct GetHierarchyByGuid(Guid taxonomyGuid)
//        {
//            var sctree = new SCTree();

//            return sctree.GetHierarchyByGuid(taxonomyGuid, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        public static MediaItemStruct GetMediaStatistics(string path)
//        {
//            var sctree = new SCTree();
//            return sctree.GetMediaStatistics(path, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        /// <summary>
//        /// If list of authors has been set, return that. Otherwise, 
//        /// set and return.
//        /// </summary>
//        /// <returns></returns>
//        public static List<SitecoreTree.StaffStruct> LazyReadAuthors()
//        {
//            return SitecoreClient._authors ?? (SitecoreClient._authors = GetAuthors());
//        }

//        /// <summary>
//        /// [0] - display name
//        /// [1] - path
//        /// </summary>
//        /// <returns></returns>
//        public static string[] GetSupportingDocumentsRootNode()
//        {
//            var sctree = new SCTree();
//            //return sctree.GetSupportingDocumentsRootNodePath(_sitecoreUser.Username, _sitecoreUser.Password).ToArray();
//            return sctree.GetSDRootPath(SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password).ToArray();
//        }

//        /// <summary>
//        /// [0] - display name
//        /// [1] - path
//        /// </summary>
//        /// <returns></returns>
//        public static string[] GetGraphicsRootNode()
//        {
//            var sctree = new SCTree();
//            return sctree.GetGraphicRootPath(SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        /// <summary>
//        /// Force the list of authors to be updated and then returned. Otherwise, 
//        /// set and return.
//        /// </summary>
//        /// <returns></returns>
//        public static List<SitecoreTree.StaffStruct> ForceReadAuthors()
//        {
//            return SitecoreClient._authors = GetAuthors();
//        }

//        public static ArticleStruct LazyReadArticleDetails(string articleNumber)
//        {
//            if (SitecoreClient._articleDetails != null && SitecoreClient._articleDetails.ArticleNumber == articleNumber)
//            {
//                return SitecoreClient._articleDetails;
//            }
//            return SitecoreClient._articleDetails = GetArticleDetails(articleNumber);
//        }

//        public static ArticleStruct ForceReadArticleDetails(string articleNumber)
//        {
//            return SitecoreClient._articleDetails = GetArticleDetails(articleNumber);
//        }

//        public static ArticleStruct ForceReadArticleDetails(Guid articleGuid)
//        {
//            return SitecoreClient._articleDetails = GetArticleDetails(articleGuid);
//        }

//        public static List<ItemStruct> GetPublications()
//        {
//            var sctree = new SCTree();

//            return sctree.GetPublications(SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password).ToList();
//        }

//        public static List<ItemStruct> GetArticleCategories(Guid pubGuid)
//        {
//            var sctree = new SCTree();

//            return sctree.GetArticleCategories(pubGuid, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password).ToList();
//        }

//        public static List<ItemStruct> GetWebCategories(Guid pubGuid)
//        {
//            using (var sctree = new SCTree())
//            {
//                return sctree.GetWebCategories(pubGuid, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password).ToList();
//            }
//        }

//        public static string GetPublicationFrequency(Guid pubGuid)
//        {
//            var sctree = new SCTree();

//            return sctree.GetPublicationFrequency(pubGuid, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        public static List<ItemStruct> GetIssues(Guid pubGuid)
//        {
//            var sctree = new SCTree();

//            return sctree.GetIssues(pubGuid, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password).ToList();
//        }

//        public static DateTime GetIssueDate(Guid issueGuid)
//        {
//            var sctree = new SCTree();

//            return sctree.GetIssueDate(issueGuid, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        public static List<ArticleSize> GetArticleSizes(Guid publicationID)
//        {
//            var sctree = new SCTree();

//            var sizes = sctree.GetArticleSizesForPublication(publicationID, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//            if (sizes == null)
//            {
//                return new List<ArticleSize>();
//            }

//            return sizes.ToList();
//        }

//        private static List<SitecoreTree.StaffStruct> GetAuthors()
//        {
//            var sctree = new SCTree();

//            return sctree.GetAuthors(SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password).ToList();
//        }

//        private static ArticleStruct GetArticleDetails(string articleNumber)
//        {
//            Globals.SitecoreAddin.Log("Getting article details from Sitecore...");
//            var sctree = new SCTree();
//            return sctree.GetArticleDetails(articleNumber, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        private static ArticleStruct GetArticleDetails(Guid articleGuid)
//        {
//            Globals.SitecoreAddin.Log("Getting article details from Sitecore...");
//            var sctree = new SCTree();
//            return sctree.GetArticleDetailsBG(articleGuid, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        public static string GetDynamicUrl(string path)
//        {
//            var sctree = new SCTree();
//            return sctree.GetDynamicUrl(path, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        public static List<GeneralTag> GetGeneralTags(Guid publication)
//        {
//            var sctree = new SCTree();
//            return sctree.GetGeneralTags(publication, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password).ToList();
//        }

//        public static int GetMaxLengthShortSummary()
//        {
//            var sctree = new SCTree();
//            return sctree.GetMaxLengthShortSummary(SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        public static int GetMaxLengthLongSummary()
//        {
//            var sctree = new SCTree();
//            return sctree.GetMaxLengthLongSummary(SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        public static bool IsAvailable()
//        {
//            var sctree = new SCTree();
//            try
//            {
//                return sctree.IsAvailable();
//            }
//            catch (Exception)
//            {

//                return false;
//            }
//        }

//        public static List<StaffStruct> GetStaffAndGroups()
//        {
//            var sctree = new SCTree();
//            return sctree.GetStaffAndGroups(SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password).ToList();
//        }

//        public static bool HasPrimaryIndustries(IEnumerable<Guid> industries)
//        {
//            var sctree = new SCTree();
//            return sctree.HasPrimaryIndustry(industries.ToArray(), SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        public static DealInfo GetDealInfo(string recordNumber)
//        {
//            var sctree = new SCTree();
//            return sctree.GetDealInfo(recordNumber, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        public static int[] GetWidthHeightOfMediaItem(string path)
//        {
//            var sctree = new SCTree();
//            return sctree.GetWidthHeightMI(path, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        public static List<CompanyWrapper> GetAllCompanies()
//        {
//            var sctree = new SCTree();

//            return sctree.GetAllCompanies(SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password).ToList();
//        }

//        public static IEnumerable<CompanyWrapper> GetAllCompaniesWithRelated()
//        {
//            var sctree = new SCTree();

//            return sctree.GetAllCompaniesWithRelated(SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        public static List<WordStyleStruct> GetParagraphStyles()
//        {
//            var sctree = new SCTree();
//            return sctree.GetParagraphStyles(SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password).ToList();
//        }

//        public static List<WordStyleStruct> GetCharacterStyles()
//        {
//            var sctree = new SCTree();
//            return sctree.GetCharacterStyles(SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password).ToList();
//        }

//        public static DirectoryStruct[] GetChildrenDirectories(string path)
//        {
//            var tree = new SCTree();
//            return tree.GetChildrenDirectories(path, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        public static MediaItemStruct GetMediaLibraryItem(string path)
//        {
//            var tree = new SCTree();
//            return tree.GetMediaLibraryItem(path, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        public static byte[] GetMediaLibraryItemData(string path)
//        {
//            return SitecoreClient.GetMediaLibraryItem(path).Data;
//        }

//        public static string MediaPreviewUrl(string path)
//        {
//            var tree = new SCTree();
//            return tree.MediaPreviewUrl(path, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }

//        public static bool IsContinuousPublishingPublication(Guid publication)
//        {
//            var tree = new SCTree();
//            return tree.IsContinuousPublishingPublication(publication, SitecoreClient._sitecoreUser.Username, SitecoreClient._sitecoreUser.Password);
//        }
//    }
//}