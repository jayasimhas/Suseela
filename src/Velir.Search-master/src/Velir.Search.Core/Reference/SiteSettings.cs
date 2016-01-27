using Sitecore.Configuration;

namespace Velir.Search.Core.Reference
{
	public static class SiteSettings
	{
		// SOLR/Lucene 4.0+ special characters
		private const string SolrLuceneSpecialCharacters = @"+ - && || ! ( ) { } [ ] ^ "" ~* ? : \ /";

		public static char ValueSeparator { get { return Settings.GetSetting("Velir.Search.ValueSeparator", ";")[0]; } }

		public static string[] SpecialCharacters { get { return Settings.GetSetting("Velir.Search.SpecialCharacters", SolrLuceneSpecialCharacters).Split(' '); } }
		public static string EscapeSequence { get { return Settings.GetSetting("Velir.Search.EscapeSequence", "\\"); } }

		public static class QueryString
		{
			public static string QueryKey { get { return Settings.GetSetting("Velir.Search.QueryString.QueryKey", string.Empty); } }
			public static string AllQueryKey { get { return Settings.GetSetting("Velir.Search.QueryString.AllQueryKey", string.Empty); } }
			public static string ExactQueryKey { get { return Settings.GetSetting("Velir.Search.QueryString.ExactQueryKey", string.Empty); } }
			public static string AnyQueryKey { get { return Settings.GetSetting("Velir.Search.QueryString.AnyQueryKey", string.Empty); } }
			public static string NoneQueryKey { get { return Settings.GetSetting("Velir.Search.QueryString.NoneQueryKey", string.Empty); } }
			public static string PageIdKey { get { return Settings.GetSetting("Velir.Search.QueryString.PageIdKey", string.Empty); } }
			public static string PageKey { get { return Settings.GetSetting("Velir.Search.QueryString.PageKey", string.Empty); } }
			public static string SortKey { get { return Settings.GetSetting("Velir.Search.QueryString.SortKey", string.Empty); } }
			public static string SortOrderKey { get { return Settings.GetSetting("Velir.Search.QueryString.SortOrderKey", string.Empty); } }
			public static string SortAscendingValue { get { return Settings.GetSetting("Velir.Search.QueryString.SortAscendingValue", string.Empty); } }
			public static string SortDescendingValue { get { return Settings.GetSetting("Velir.Search.QueryString.SortDescendingValue", string.Empty); } }
		}
	}
}
