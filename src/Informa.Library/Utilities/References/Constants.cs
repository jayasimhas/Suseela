using Informa.Library.Utilities.CMSHelpers;
using System;
using System.Collections.Generic;

namespace Informa.Library.Utilities.References
{
    public static class Constants
    {
        public const string ArticleNumberLength = "000000";
        public const string CoreDb = "core";
        public const string MasterDb = "master";
        public const string WebDb = "web";

        public const string TableauPrefix = "[T#:";

        public const string AuthorsIndexName = "informa_authors";


        public static string ContentRootNode = ItemIdResolver.GetItemIdByKey("ContentRootNode");

        public const string ScripPublicationName = "Scrip Intelligence";

        public const string MediaLibraryRoot = "/sitecore/media library/";
        public const string MediaLibraryPath = "Documents/";

        public const string DTDPath = "Util\\DTD\\";

        public const string CryptoKey = "Ajv4FMtL7Iuq3H04ZkQC";
        public const string PublicationRootTemplateID = "{DD003F89-D57D-48CB-B428-FFB519AACA56}";
        public const string HomeRootTemplateID = "{88CACA5D-2AE1-4739-8145-399F3486C2DE}";

        public static string StrategicTransactionsComponent = ItemIdResolver.GetItemIdByKey("StrategicTransactionsComponent");
        public static string DCDSubscribeComponent = ItemIdResolver.GetItemIdByKey("DCDSubscribeComponent");

        public static string VirtualWhiteboardIssuesFolder = ItemIdResolver.GetItemIdByKey("VirtualWhiteboardIssuesFolder");
        public static string VirtualWhiteboardArchivedIssuesFolder = ItemIdResolver.GetItemIdByKey("VirtualWhiteboardArchivedIssuesFolder");


        public static string ScripRootNode = ItemIdResolver.GetItemIdByKey("ScripRootNode");
        public const string ScripRootNodeIntials = "SC";
        public static string ScripEmailConfig = ItemIdResolver.GetItemIdByKey("EmailConfig");

        public static string InVivoRootNode = ItemIdResolver.GetItemIdByKey("InVivoRootNode");
        public const string InVivoRootNodeIntials = "IV";

        public static string PinkSheetRootNode = ItemIdResolver.GetItemIdByKey("PinkSheetRootNode");
        public const string PinkSheetRootNodeIntials = "PS";

        public static string MedtechInsightRootNode = ItemIdResolver.GetItemIdByKey("MedtechInsightRootNode");
        public const string MedtechInsightRootNodeIntials = "MT";

        public static string RoseSheetRootNode = ItemIdResolver.GetItemIdByKey("RoseSheetRootNode");
        public const string RoseSheetNodeIntials = "RS";

        public const string MSWordDocumentRootNode = "{FDBFCAC8 -03CA-4B0B-BEFE-2171050E19C6}"; //not added to config, as it's missing in CMS

        public const string NavigationItemTemplateID = "{354B0538-CB81-4B26-A25E-7B5DBA03C2F5}";

        public static string EditAfterPublishWorkflowCommand = ItemIdResolver.GetItemIdByKey("EditAfterPublishWorkflowCommand");

        public static string ScripWorkflow = ItemIdResolver.GetItemIdByKey("FreeWithEntitlement");
        public static readonly Dictionary<Guid, string> PublicationPrefixDictionary = new Dictionary<Guid, string>
                {
                        { new Guid(ScripRootNode), ScripRootNodeIntials},
                        { new Guid(InVivoRootNode), InVivoRootNodeIntials},
                        { new Guid(PinkSheetRootNode), PinkSheetRootNodeIntials},
                        { new Guid(MedtechInsightRootNode), MedtechInsightRootNodeIntials},
                        { new Guid(RoseSheetRootNode), RoseSheetNodeIntials},
                };

        /// <summary>
        /// Why is this a dictionary?  Not changing because I'm afraid the word plugin will blow up.
        /// </summary>
        public static readonly Dictionary<string, string> PublicationPrefixCollection = new Dictionary<string, string>
                {
                        { ScripRootNodeIntials, ScripRootNodeIntials},
                        { InVivoRootNodeIntials, InVivoRootNodeIntials},
                        { PinkSheetRootNodeIntials, PinkSheetRootNodeIntials},
                        { MedtechInsightRootNodeIntials, MedtechInsightRootNodeIntials},
                        { RoseSheetNodeIntials, RoseSheetNodeIntials},
                };

        public static int[] PerPageSizes = { 10, 20, 50 };
        public static string VWBSearchPageId = new ItemReferences().VwbSearchPage.ToString().ToLower().Replace("{", "").Replace("}", "");

        public static class QueryString
        {
            public const string PageId = "pId";
            public const string InProgressKey = "inprogress";
            public const string SearchHeadlinesOnly = "headlinesOnly";
            public const string PerPageKey = "perPage";
            public const string UtmCampaign = "utm_campaign";
            public const string UtmSource = "utm_source";
            public const string UtmMedium = "utm_medium";
            public const string EncryptedToken = "token";
            public const string DateRangeFilterLabelKey = "dateFilterLabel";
            public const string TimeKey = "time";
            public const string Author = "author";
            public const string AuthorFullName = "authorName";
            public const string Company = "companies";
            public const string Publication = "publication";
        }

        public static class SettingKeys
        {
            public const string ExactTargetUseSandbox = "ExactTarget.UseSandbox";
            public const string ExactTargetClientId = "ExactTarget.ClientId";
            public const string ExactTargetSecretKey = "ExactTarget.SecretKey";
        }

        public static class ArticleDisplayModeTypes
        {
            public const string FeaturedArticleFullRow = "FeaturedArticleFullRow";
            public const string FeaturedArticleHalfRow = "FeaturedArticleHalfRow";
            public const string FeaturedArticleThreeRows = "FeaturedArticleThreeRows";
            public const string FeaturedArticleTwoColumn = "FeaturedArticleTwoColumn";
        }

        public static class CompaniesResultTableTypes
        {
            public const string FinancialResults = "financialresults";
            public const string AnnualResults = "annualresults";
            public const string QuarterlyResults = "quarterlyresults";
            public const string CompareFinancialResults = "comparefinancialresults";
        }

    }
}
