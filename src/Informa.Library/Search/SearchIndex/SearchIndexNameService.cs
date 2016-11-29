namespace Informa.Library.Search.SearchIndex
{
    using Glass.Mapper.Sc;
    using Jabberwocky.Autofac.Attributes;
    using Library.Utilities.CMSHelpers;
    using Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
    using Services.Global;
    using Site;
    using System;
    using System.Web;

    [AutowireService]
    public class SearchIndexNameService : ISearchIndexNameService
    {
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IVerticalRootContext VerticalRootContext;
        protected readonly IGlobalSitecoreService GlobalService;

        public SearchIndexNameService(ISitecoreContext sitecoreContext, ISiteRootContext siterootContext, IVerticalRootContext verticalRootContext
            ,IGlobalSitecoreService globalService)
        {
            SitecoreContext = sitecoreContext;
            SiteRootContext = siterootContext;
            VerticalRootContext = verticalRootContext;
            GlobalService = globalService;
        }

        /// <summary>
        /// Gets solr index name based on the current request context
        /// </summary>
        /// <returns></returns>
        public string GetIndexName()
        {
            if (SiteRootContext?.Item?.Search_Index_Name != null)
            {
                //return string.Format(SiteRootContext.Item.Search_Index_Name, SitecoreContext.Database.Name);
                return SiteRootContext.Item.Search_Index_Name;
            }
            else if(VerticalRootContext?.Item?.Search_Index_Name != null)
            {
                //return string.Format(VerticalRootContext.Item.Search_Index_Name, SitecoreContext.Database.Name);
                return VerticalRootContext.Item.Search_Index_Name;
            }

            //return string.Format("sitecore_{0}_index", SitecoreContext.Database.Name);
            return "sitecore_{0}_index";
        }

        public string GetAutherIndexName()
        {
            if (VerticalRootContext?.Item?.Auther_Search_Index_Name != null)
            {
                //return string.Format(VerticalRootContext.Item.Search_Index_Name, SitecoreContext.Database.Name);
                return VerticalRootContext.Item.Auther_Search_Index_Name;
            }
            return "sitecore_{0}_index";
        }

        public string GetVerticalRootFromQuerystring()
        {
            return HttpContext.Current.Request.QueryString["verticalroot"];
        }

        /// <summary>
        /// Gets solr index name based on the current request context for the given publication id
        /// </summary>
        /// <param name="publicationGuid"></param>
        /// <returns></returns>
        public string GetIndexName(Guid publicationGuid = default(Guid))
        {
            if (publicationGuid != default(Guid))
            {
                var publicationItem = GlobalService.GetItem<ISite_Root>(publicationGuid);

                //Not getting value from Search_Index_Name field
                //var publicationItem = SitecoreContext.GetDynamicItem(SitecoreSettingResolver.Instance.CurrentPublicationGuid);
                if (publicationItem?.Search_Index_Name != null)
                {
                    return publicationItem?.Search_Index_Name;
                }
                else if (publicationItem != null)
                {
                    var verticalItem = GlobalService.GetItem<IVertical_Root>(publicationItem._Parent._Id);
                    if (verticalItem?.Search_Index_Name != null)
                    {
                        return verticalItem?.Search_Index_Name;
                    }
                }
            }
            return GetIndexName();
        }

    }
}
