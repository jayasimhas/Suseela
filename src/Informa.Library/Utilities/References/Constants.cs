﻿using System;
using System.Collections.Generic;

namespace Informa.Library.Utilities.References
{
	public static class Constants
	{
		public const string ArticleNumberLength = "000000";
		public const string CoreDb = "core";
		public const string MasterDb = "master";
		public const string WebDb = "web";

		public const string AuthorsIndexName = "informa_authors";

		public const string ContentRootNode = "{0DE95AE4-41AB-4D01-9EB0-67441B7C2450}";

		public const string ScripPublicationName = "Scrip Intelligence";

		public const string MediaLibraryRoot = "/sitecore/media library/";
		public const string MediaLibraryPath = "Documents/";

		public const string DTDPath = "Util\\DTD\\";

		public const string CryptoKey = "Ajv4FMtL7Iuq3H04ZkQC";

		public const string StrategicTransactionsComponent = "{7072BDB1-1398-4C51-986F-404251CF3E8A}";
		public const string DCDSubscribeComponent = "{602FABB7-E964-45B2-A8DF-E29356A7A8ED}";

		public const string VirtualWhiteboardIssuesFolder = "{ECE677C9-3CC5-41F5-ADCB-D103C60783A7}";
		public const string VirtualWhiteboardArchivedIssuesFolder = "{AAB11244-7D15-4A43-A709-DC8951893FD2}";

		public const string ScripRootNode = "{3818C47E-4B75-4305-8F01-AB994150A1B0}";
		public const string ScripRootNodeIntials = "SC";
		public const string ScripEmailConfig = "{077AB43D-8F0F-413E-A585-4DB65EA23234}";

		public const string InVivoRootNode = "{920FAC0D-4DA5-465A-94F9-A8B4D03DFCE3}";
		public const string InVivoRootNodeIntials = "IV";

		public const string PinkSheetRootNode = "{87B710BC-BACD-4D38-895F-90F42D762393}";
		public const string PinkSheetRootNodeIntials = "PS";

		public const string MedtechInsightRootNode = "{989CA96C-8A54-4CC0-BD51-A127930935A9}";
		public const string MedtechInsightRootNodeIntials = "MT";

		public const string RoseSheetRootNode = "{204199B0-C381-4B9B-92BE-63E5754F5E90}";
		public const string RoseSheetNodeIntials = "RS";

		public const string MSWordDocumentRootNode = "{FDBFCAC8 -03CA-4B0B-BEFE-2171050E19C6}";

		public static string PublicationRootTemplateID = "{DD003F89-D57D-48CB-B428-FFB519AACA56}";

        public static string HomeRootTemplateID = "{88CACA5D-2AE1-4739-8145-399F3486C2DE}";
		//public static string EditAfterPublishWorkflowCommand = "{322D0739-B3AC-416F-BF58-5E39E716F003}";

		//public static string ScripWorkflow = "{926E6200-EB76-4AD4-8614-691D002573AC}";
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
		public static string NavigationItemTemplateID = "{354B0538-CB81-4B26-A25E-7B5DBA03C2F5}";
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
	}
}
