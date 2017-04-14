using Informa.Library.SalesforceConfiguration;
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
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        public SalesforceSessionFactoryConfiguration(
            ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

       public string Url => SalesforceConfigurationContext?.SalesForceConfiguration?.Salesforce_Session_Factory_Url?.Url ?? Settings.GetSetting(UrlConfigKey);
        public string Username => SalesforceConfigurationContext?.SalesForceConfiguration?.Salesforce_Session_Factory_Username ?? Settings.GetSetting(UsernameConfigKey);
        public string Password => SalesforceConfigurationContext?.SalesForceConfiguration?.Salesforce_Session_Factory_Password ?? Settings.GetSetting(PasswordConfigKey);
        public string Token => SalesforceConfigurationContext?.SalesForceConfiguration?.Salesforce_Session_Factory_Token ?? Settings.GetSetting(TokenConfigKey);
        public int Timeout => int.Parse(SalesforceConfigurationContext?.SalesForceConfiguration?.Salesforce_Session_Factory_Timeout ?? Settings.GetSetting(TimeoutConfigKey));
    }
}
