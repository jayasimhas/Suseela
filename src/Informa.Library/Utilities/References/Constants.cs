using System;
using System.Collections.Generic;

namespace Informa.Library.Utilities.References
{
	public static class Constants
	{
		public const string ArticleNumberLength = "000000";
		public const string MasterDb = "master";
		public const string ScripPublicationName = "Scrip Intelligence";

		public const string MediaLibraryRoot = "/sitecore/media library/";
		public const string MediaLibraryPath = "Documents/";

		public const string DTDPath = "Util\\DTD\\";

		public const string ScripRootNode = "{3818C47E-4B75-4305-8F01-AB994150A1B0}";
		public const string ScripRootNodeIntials = "SC";
		public const string ScripEmailConfig = "{077AB43D-8F0F-413E-A585-4DB65EA23234}";
		
		public const string MSWordDocumentRootNode = "{FDBFCAC8 -03CA-4B0B-BEFE-2171050E19C6}";

		public static string EditAfterPublishWorkflowCommand = "{322D0739-B3AC-416F-BF58-5E39E716F003}";

		public static string ScripWorkflow = "{926E6200-EB76-4AD4-8614-691D002573AC}";
		public static readonly Dictionary<Guid, string> PublicationPrefixDictionary = new Dictionary<Guid, string>
		{
			{ new Guid(ScripRootNode), ScripRootNodeIntials},
		};

		public static class QueryString
		{
			public const string InProgressKey = "inprogress";
		}
	}
}
