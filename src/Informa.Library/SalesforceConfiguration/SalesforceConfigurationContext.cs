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
        private string _tokenEndPoints = "/services/oauth2/token";
        private string _userInforEndPoints = "{0}/services/oauth2/userinfo";
        private string _authorizationRequestEndPoints = "{0}/services/apexrest/identity/{1}/services/oauth2/authorize?response_type=code&client_id={2}&redirect_uri={3}&state={4}";
        private string _userEntitlementsRequestEndPoints = "/services/apexrest/UserEntitlements/{0}";
        private string _registrationEndpoints = "{0}/registration?referralurl={1}&referralid={2}";
        private string _logoutEndpoints = "{0}/secur/logout.jsp";
        private string _userDetailsEndPoints = "/services/apexrest/UserPreferences/{0}";
        private string _changePasswordEndpoints = "{0}/changepassword?referralurl={1}&referralid={2}";
        private string _getUserProductPreferencesEndpoints = "/services/data/v20.0/query/?q={0}";
        private string _addUserProductPreferencesEndpoints = "/services/data/v34.0/composite/tree/Product_Preference__c";
        private string _deleteUserProductPreferenceEndpoints = "/services/data/v20.0/sobjects/Product_Preference__c/{0}";
        private string _updateUserProductPreferenceEndpoints = "/services/data/v20.0/sobjects/Product_Preference__c/{0}";


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
            string loginUrl = (SalesForceConfiguration?.Salesforce_Login_Url != null
                && !string.IsNullOrEmpty(SalesForceConfiguration?.Salesforce_Login_Url.Url) ?
                SalesForceConfiguration?.Salesforce_Login_Url.Url : string.Empty);

            url = string.Format(_authorizationRequestEndPoints, loginUrl,
                  productCode, SalesForceConfiguration?.Salesforce_Session_Factory_Username, callbackUrl, state);
            return url;
        }

        public string GetUserEntitlementsEndPoints(string userName)
        {
            return string.Format(_userEntitlementsRequestEndPoints, userName);
        }

        public string GetUserAccessTokenEndPoints()
        {
            return _tokenEndPoints;
        }

        public string GetUserInfoEndPoints()
        {
            return string.Format(_userInforEndPoints,
                SalesForceConfiguration?.Salesforce_Service_Url?.Url);
        }

        public string GetLogoutEndPoints()
        {
            return string.Format(_logoutEndpoints, SalesForceConfiguration?.Salesforce_Service_Url?.Url);
        }

        public string GetRegistrationEndPoints(string referralurl, string referralid)
        {
            return string.Format(_registrationEndpoints,
            SalesForceConfiguration?.Salesforce_Service_Url?.Url, referralurl, referralid);
        }
        public string GetChangePasswordEndpoint(string referralurl, string referralid)
        {
            return string.Format(_changePasswordEndpoints,
                        SalesForceConfiguration?.Salesforce_Service_Url?.Url, referralurl, referralid);
        }
        public string GetUpdateUserDetailsEndPoints(string userName)
        {
           return string.Format(_userDetailsEndPoints, userName);
        }

        public string GetUserProductPreferencesEndPoints(string query)
        {
            return string.Format(_getUserProductPreferencesEndpoints, query);
        }

        public string AddUserProductPreferencesEndPoints()
        {
            return _addUserProductPreferencesEndpoints;
        }

        public string DeleteUserProductPreferenceEndPoints(string itemId)
        {
            return string.Format(_deleteUserProductPreferenceEndpoints, itemId);
        }

        public string UpdateUserProductPreferenceEndPoints(string itemId)
        {
            return string.Format(_updateUserProductPreferenceEndpoints, itemId);
        }
    }
}
