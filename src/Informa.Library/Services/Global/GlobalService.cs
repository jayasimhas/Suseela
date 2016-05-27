using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Global;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Data.Items;

namespace Informa.Library.Services.Global {

    [AutowireService(LifetimeScope.SingleInstance)]
    public class GlobalService : IGlobalService
    {
        protected readonly ISitecoreService SitecoreService;
        protected readonly ICacheProvider CacheProvider;
        protected readonly IItemReferences ItemReferences;

        public GlobalService(
            ISitecoreService sitecoreService,
            ICacheProvider cacheProvider,
            IItemReferences itemReferences
            )
        {
            SitecoreService = sitecoreService;
            CacheProvider = cacheProvider;
            ItemReferences = itemReferences;
        }

        private string CreateCacheKey(string suffix) {
            return $"{nameof(GlobalService)}-{suffix}";
        }

        public IInforma_Bar GetInformaBar()
        {
            string cacheKey = CreateCacheKey("InformaBar");
            return CacheProvider.GetFromCache(cacheKey, BuildInformaBar);
        }

        private IInforma_Bar BuildInformaBar()
        {
            return SitecoreService.GetItem<IInforma_Bar>(ItemReferences.InformaBar);
        }

        public IEnumerable<ListItem> GetSalutations()
        {
            string cacheKey = CreateCacheKey("Salutations");
            return CacheProvider.GetFromCache(cacheKey, () => GetListing(ItemReferences.AccountSalutations));
        }

        public IEnumerable<ListItem> GetNameSuffixes()
        {
            string cacheKey = CreateCacheKey("Suffixes");
            return CacheProvider.GetFromCache(cacheKey, () => GetListing(ItemReferences.AccountNameSuffixes));
        }

        public IEnumerable<ListItem> GetJobFunctions()
        {
            string cacheKey = CreateCacheKey("JobFunctions");
            return CacheProvider.GetFromCache(cacheKey, () => GetListing(ItemReferences.AccountJobFunctions));
        }

        public IEnumerable<ListItem> GetJobIndustries()
        {
            string cacheKey = CreateCacheKey("JobIndustries");
            return CacheProvider.GetFromCache(cacheKey, () => GetListing(ItemReferences.AccountJobIndustries));
        }

        public IEnumerable<ListItem> GetPhoneTypes() {
            string cacheKey = CreateCacheKey("PhoneTypes");
            return CacheProvider.GetFromCache(cacheKey, () => GetListing(ItemReferences.AccountPhoneTypes));
        }

        public IEnumerable<ListItem> GetCountries() {
            string cacheKey = CreateCacheKey("Countries");
            return CacheProvider.GetFromCache(cacheKey, () => GetListing(ItemReferences.AccountCountries));
        }

        private IEnumerable<ListItem> GetListing(Guid g) {
            var item = SitecoreService.GetItem<Item>(g);
            if (item == null)
                return Enumerable.Empty<ListItem>();

            return item.GetChildren()
                .Select(a => SitecoreService.GetItem<ITaxonomy_Item>(a.ID.Guid))
                .Select(b => new ListItem(b.Item_Name, (b.Item_Name.ToLower().Contains("select one")) 
                ? "" : 
                b.Item_Name));
        }

        public T GetItem<T>(Guid g) where T : class
        {
            string cacheKey = CreateCacheKey($"GetItem-{g}");
            return CacheProvider.GetFromCache(cacheKey, () => BuildItem<T>(g));
        }

        private T BuildItem<T>(Guid g) where T : class
        {
            return SitecoreService.GetItem<T>(g);
        }

        public T GetItem<T>(string id) where T : class
        {
            Guid g;
            return (Guid.TryParse(id, out g))
                ? GetItem<T>(g)
                : null;
        }
    }
}
