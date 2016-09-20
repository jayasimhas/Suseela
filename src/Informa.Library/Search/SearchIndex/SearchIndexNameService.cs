namespace Informa.Library.Search.SearchIndex
{
    using Glass.Mapper.Sc;
    using Jabberwocky.Autofac.Attributes;
    using Site;

    [AutowireService]
    public class SearchIndexNameService : ISearchIndexNameService
    {
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly ISiteRootContext SiteRootContext;

        public SearchIndexNameService(ISitecoreContext sitecoreContext, ISiteRootContext siterootContext)
        {
            SitecoreContext = sitecoreContext;
            SiteRootContext = siterootContext;
        }
        public string GetIndexName()
        {
            if (SiteRootContext?.Item?.SearchIndexName != null)
            {
                return string.Format(SiteRootContext.Item.SearchIndexName, SitecoreContext.Database.Name);
            }

            return string.Format("sitecore_{0}_index", SitecoreContext.Database.Name);
        }
    }
}
