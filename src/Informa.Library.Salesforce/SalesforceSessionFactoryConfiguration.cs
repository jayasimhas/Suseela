using Sitecore.Configuration;

namespace Informa.Library.Salesforce
{
	// TODO-Sforce: Read values from configuration files
	public class SalesforceSessionFactoryConfiguration : ISalesforceSessionFactoryConfiguration
	{
		private const string UsernameConfigKey = "SalesforceSessionFactoryConfiguration.Username";
		private const string PasswordConfigKey = "SalesforceSessionFactoryConfiguration.Password";
		private const string TokenConfigKey = "SalesforceSessionFactoryConfiguration.Token";
		private const string TimeoutConfigKey = "SalesforceSessionFactoryConfiguration.Timeout";

		public string Username => Settings.GetSetting(UsernameConfigKey);
		public string Password => Settings.GetSetting(PasswordConfigKey);
		public string Token => Settings.GetSetting(TokenConfigKey);
		public int Timeout => Settings.GetIntSetting(TimeoutConfigKey, 3000);
	}
}
