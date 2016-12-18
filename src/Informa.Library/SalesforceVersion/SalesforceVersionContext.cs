using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Core.Caching;

namespace Informa.Library.SalesforceVersion
{
    [AutowireService(LifetimeScope.PerScope)]
    public class SalesforceVersionContext : ISalesforceVersionContext
    {
        protected readonly ICacheProvider CacheProvider;
        protected readonly ISiteRootContext SiteRootContext;

        public SalesforceVersionContext(
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

        public bool IsNewSalesforceEnabled => string.IsNullOrWhiteSpace(SalesForceConfiguration?.Salesforce_Session_Factory_Token);

        private string CreateCacheKey(string suffix)
        {
            return $"{nameof(SalesforceVersionContext)}-{suffix}";
        }

        public ISalesforce_Configuration GetSalesforceConfiguration()
        {
            return _siteRootItem?.Salesforce_Version;
        }
    }
}
