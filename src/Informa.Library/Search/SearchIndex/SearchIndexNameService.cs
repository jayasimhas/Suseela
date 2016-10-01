namespace Informa.Library.Search.SearchIndex
{
    using Glass.Mapper.Sc;
    using Jabberwocky.Autofac.Attributes;
    using Library.Utilities.CMSHelpers;
    using Site;

    [AutowireService]
    public class SearchIndexNameService : ISearchIndexNameService
    {
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IVerticalRootContext VerticalRootContext;

        public SearchIndexNameService(ISitecoreContext sitecoreContext, ISiteRootContext siterootContext, IVerticalRootContext verticalRootContext)
        {
            SitecoreContext = sitecoreContext;
            SiteRootContext = siterootContext;
            VerticalRootContext = verticalRootContext;
        }
        public string GetIndexName()
        {
            if (SiteRootContext?.Item?.Search_Index_Name != null)
            {
                return string.Format(SiteRootContext.Item.Search_Index_Name, SitecoreContext.Database.Name);
            }
            else if(VerticalRootContext?.Item?.Search_Index_Name != null)
            {
                return string.Format(VerticalRootContext.Item.Search_Index_Name, SitecoreContext.Database.Name);
            }

            return string.Format("sitecore_{0}_index", SitecoreContext.Database.Name);
        }
    }
}
