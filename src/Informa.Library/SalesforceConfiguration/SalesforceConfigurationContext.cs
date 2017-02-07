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
        private string _authorizationRequestEndPoint = "{0}/services/apexrest/identity/{1}/services/oauth2/authorize?response_type=code&client_id={2}&redirect_uri={3}&state={4}";
        private string _userEntitlementsRequestEndPoint = "{0}/services/apexrest/UserEntitlements/{1}";


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

        public string GetLoginEndPoints(string productCode, string callbackUrl, string state)
        {
            string url = string.Empty;
            url = string.Format(_authorizationRequestEndPoint, SalesForceConfiguration?.Salesforce_Login_Url.Url,
                productCode, SalesForceConfiguration?.Salesforce_Session_Factory_Username, callbackUrl, state);
            return url;
        }

        public string GetUserEntitlementsEndPoints(string userName)
        {
            string url = string.Empty;
            url = string.Format(_userEntitlementsRequestEndPoint,
                SalesForceConfiguration?.Salesforce_Entitlement_Api_Url.Url, userName);
            return url;
        }
    }
}
