using Informa.Library.SalesforceConfiguration;
using Sitecore.Configuration;

namespace Informa.Library.Salesforce
{
    public class SalesforceSessionFactoryConfiguration : ISalesforceSessionFactoryConfiguration
    {
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        public SalesforceSessionFactoryConfiguration(
            ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

       public string Url => SalesforceConfigurationContext?.SalesForceConfiguration?.Salesforce_Session_Factory_Url?.Url ?? string.Empty;
        public string Username => SalesforceConfigurationContext?.SalesForceConfiguration?.Salesforce_Session_Factory_Username ?? string.Empty;
        public string Password => SalesforceConfigurationContext?.SalesForceConfiguration?.Salesforce_Session_Factory_Password ?? string.Empty;
        public string Token => SalesforceConfigurationContext?.SalesForceConfiguration?.Salesforce_Session_Factory_Token ?? string.Empty;
        public int Timeout => int.Parse(SalesforceConfigurationContext?.SalesForceConfiguration?.Salesforce_Session_Factory_Timeout ?? "0");
    }
}
