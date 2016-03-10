using Sitecore.Configuration;

namespace Informa.Library.Salesforce
{
	public class SalesforceSessionFactoryConfiguration : ISalesforceSessionFactoryConfiguration
	{
		private const string UrlConfigKey = "SalesforceSessionFactoryConfiguration.Url";
		private const string UsernameConfigKey = "SalesforceSessionFactoryConfiguration.Username";
		private const string PasswordConfigKey = "SalesforceSessionFactoryConfiguration.Password";
		private const string TokenConfigKey = "SalesforceSessionFactoryConfiguration.Token";
		private const string TimeoutConfigKey = "SalesforceSessionFactoryConfiguration.Timeout";

		public string Url => Settings.GetSetting(UrlConfigKey);
		public string Username => Settings.GetSetting(UsernameConfigKey);
		public string Password => Settings.GetSetting(PasswordConfigKey);
		public string Token => Settings.GetSetting(TokenConfigKey);
		public int Timeout => Settings.GetIntSetting(TimeoutConfigKey, 3000);
	}
}
