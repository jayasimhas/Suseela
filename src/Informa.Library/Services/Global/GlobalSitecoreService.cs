using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Glass.Mapper.Sc;
using Informa.Library.Site;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Global;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Data.Items;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;

namespace Informa.Library.Services.Global {

    [AutowireService(LifetimeScope.PerScope)]
    public class GlobalService : IGlobalSitecoreService
    {
        protected readonly ISitecoreService SitecoreService;
        protected readonly ICacheProvider CacheProvider;
        protected readonly IItemReferences ItemReferences;
        protected readonly ISiteRootContext SiteRootContext;

        public GlobalService(
            ISitecoreService sitecoreService,
            ICacheProvider cacheProvider,
            IItemReferences itemReferences,
            ISiteRootContext siteRootContext
            )
        {
            SitecoreService = sitecoreService;
            CacheProvider = cacheProvider;
            ItemReferences = itemReferences;
            SiteRootContext = siteRootContext;
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
            return GetListing(ItemReferences.AccountSalutations);
        }

        public IEnumerable<ListItem> GetNameSuffixes()
        {
            return GetListing(ItemReferences.AccountNameSuffixes);
        }

        public IEnumerable<ListItem> GetJobFunctions()
        {
            return GetListing(ItemReferences.AccountJobFunctions);
        }

        public IEnumerable<ListItem> GetJobIndustries()
        {
            return GetListing(ItemReferences.AccountJobIndustries);
        }

        public IEnumerable<ListItem> GetPhoneTypes() {
            return GetListing(ItemReferences.AccountPhoneTypes);
        }

        public IEnumerable<ListItem> GetCountries() {
            return GetListing(ItemReferences.AccountCountries);
        }

        private IEnumerable<ListItem> GetListing(Guid g)
        {
            string cacheKey = CreateCacheKey(g.ToString());
            return CacheProvider.GetFromCache(cacheKey, () => BuildListing(g));
        }

        private IEnumerable<ListItem> BuildListing(Guid g) { 

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
            if (g == Guid.Empty)
                return default(T);

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
                : default(T);
        }

        public string GetPageTitle(I___BasePage page)
        {
            string cacheKey = CreateCacheKey($"GetPageTitle-{page._Id}");
            return CacheProvider.GetFromCache(cacheKey, () => BuildPageTitle(page));
        }

        private string BuildPageTitle(I___BasePage page)
        {
            var pageTitle = string.Copy(page?.Meta_Title_Override.StripHtml() ?? string.Empty);
            if (string.IsNullOrWhiteSpace(pageTitle))
                pageTitle = string.Copy(page?.Title?.StripHtml() ?? string.Empty);
            if (string.IsNullOrWhiteSpace(pageTitle))
                pageTitle = string.Copy(page?._Name ?? string.Empty);

            var publicationName = (SiteRootContext.Item == null)
                ? string.Empty
                : $" :: {SiteRootContext.Item.Publication_Name.StripHtml()}";

            return string.Concat(pageTitle, publicationName);
        }

        public ISite_Root GetSiteRootAncestor(Guid g)
        {
            string cacheKey = CreateCacheKey($"BuildSiteRootAncestor-{g}");
            return CacheProvider.GetFromCache(cacheKey, () => BuildSiteRootAncestor(g));
        }

        private ISite_Root BuildSiteRootAncestor(Guid g)
        {
            var curItem = SitecoreService.GetItem<Item>(g);
            if (curItem == null)
                return null;

					if (curItem.TemplateID == ISite_RootConstants.TemplateId)
		        return SitecoreService.GetItem<ISite_Root>(curItem.ID.Guid);

	        var rootItem = curItem.Axes
                  .GetAncestors()
                  .FirstOrDefault(a => a.Template.ID.Guid.Equals(ISite_RootConstants.TemplateId.Guid));

            if (rootItem == null)
                return null;

            return SitecoreService.GetItem<ISite_Root>(rootItem.ID.Guid);
        }

        public string GetPublicationName(Guid g)
        {
            if (g == null || g == Guid.Empty)
                return string.Empty;

            ISite_Root r = GetSiteRootAncestor(g);
            return (r == null)
                ? string.Empty
                : r.Publication_Name;
        }
    }
}
