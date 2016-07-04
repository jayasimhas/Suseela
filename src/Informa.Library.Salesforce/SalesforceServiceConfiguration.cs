using Sitecore.Configuration;

namespace Informa.Library.Salesforce
{
	public class SalesforceServiceConfiguration : ISalesforceServiceConfiguration
	{
		public const string UrlConfigKey = "SalesforceServiceConfiguration.Url";

		private string _url;
		public string Url => _url ?? (_url = Settings.GetSetting(UrlConfigKey));
	}
}
