using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Informa.Library.Utilities.References
{
	public static class Constants
	{
		public static string ArticleNumberLength = "000000";
		public static string MasterDb = "master";
		public static string WebDb = "web";
		public static string ScripPublicationName = "Scrip Intelligence";

		public static string MediaLibraryRoot = "/sitecore/media library/";
		public static string MediaLibraryPath = "Documents/";

		public static string ScripRootNode = "{3818C47E-4B75-4305-8F01-AB994150A1B0}";
		public static string MSWordDocumentRootNode = "{ FDBFCAC8 -03CA-4B0B-BEFE-2171050E19C6}";
		public static string ScripWorkflow = "{926E6200-EB76-4AD4-8614-691D002573AC}";
		public static Dictionary<Guid, string> PublicationPrefixDictionary = new Dictionary<Guid, string>
		{
			{ new Guid(ScripRootNode), "SC"},
		};

        public static Dictionary<string, string> PublicationPrefixByName = new Dictionary<string, string>
        {
            {ScripPublicationName, "SC" }
        };
	}
}
