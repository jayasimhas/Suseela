using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Core.Caching;
using Sitecore.Configuration;

namespace Informa.Library.SalesforceConfiguration

{
    [AutowireService(LifetimeScope.PerScope)]
    public class SalesforceConfigurationContext : ISalesforceConfigurationContext
    {
        protected readonly ICacheProvider CacheProvider;
        protected readonly ISiteRootContext SiteRootContext;
        private const string MultipleSalesforceSupportConfigKey = "MultipleSalesforceSupportEnabled";
        private string _isMultipleSalesforceSupportEnabled = Settings.GetSetting(MultipleSalesforceSupportConfigKey);

        public SalesforceConfigurationContext(
                        ISiteRootContext siteRootContext,
                        ICacheProvider cacheProvider)
        {
            SiteRootContext = siteRootContext;
            CacheProvider = cacheProvider;
        }

        private ISite_Root _siteRootItem => SiteRootContext?.Item;
        public ISalesforce_Configuration SalesForceConfiguration
        {
            get
            {
                string cacheKey = CreateCacheKey($"SalesForceConfiguration-{_siteRootItem?.Publication_Code}");
                return CacheProvider.GetFromCache(cacheKey, GetSalesforceConfiguration);
            }
        }

        public bool IsNewSalesforceEnabled => (!string.IsNullOrWhiteSpace(_isMultipleSalesforceSupportEnabled) &&
            _isMultipleSalesforceSupportEnabled.Equals("true", System.StringComparison.OrdinalIgnoreCase)) &&
            string.IsNullOrWhiteSpace(SalesForceConfiguration?.Salesforce_Session_Factory_Token);

        private string CreateCacheKey(string suffix)
        {
            return $"{nameof(SalesforceConfigurationContext)}-{suffix}";
        }

        public ISalesforce_Configuration GetSalesforceConfiguration()
        {
            return _siteRootItem?.Salesforce_Version;
        }
    }
}
