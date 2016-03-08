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
		public const string MSWordDocumentRootNode = "{ FDBFCAC8 -03CA-4B0B-BEFE-2171050E19C6}";
		public static readonly Dictionary<Guid, string> PublicationPrefixDictionary = new Dictionary<Guid, string>
		{
			{ new Guid(ScripRootNode), "SC"},
		};

        public static readonly Dictionary<string, string> PublicationPrefixByName = new Dictionary<string, string>
        {
            {ScripPublicationName, "SC" }
        };
	}
}
