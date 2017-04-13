using Informa.Library.SalesforceConfiguration;
using Sitecore.Configuration;

namespace Informa.Library.Salesforce
{
	public class SalesforceServiceConfiguration : ISalesforceServiceConfiguration
	{
        private const string EbiUrlConfigKey = "SalesforceSessionFactoryConfiguration.EbiUrl";
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        public SalesforceServiceConfiguration(
            ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

        public string Url => SalesforceConfigurationContext?.SalesForceConfiguration?.Salesforce_Service_Url?.Url ?? Settings.GetSetting(EbiUrlConfigKey);
	}
}
