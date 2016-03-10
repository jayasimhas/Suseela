using Sitecore.Configuration;

namespace Informa.Library.Salesforce
{
	public class SalesforceServiceConfiguration : ISalesforceServiceConfiguration
	{
		public const string UrlConfigKey = "SalesforceServiceConfiguration.Url";

		public string Url => Settings.GetSetting(UrlConfigKey);
	}
}
