using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc;
using Informa.Library.Search;
using Informa.Library.Search.Results;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.ContentSearch.Linq;
using Informa.Library.Search.SearchIndex;

namespace Informa.Library.Services.RobotsText
{
    public interface IRobotsTextService
    {
        string GetDisallowedGeneralContentUrls();
    }

    [AutowireService(LifetimeScope.PerScope)]
    public class RobotsTextService : IRobotsTextService
    {
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly IProviderSearchContextFactory SearchContextFactory;
        protected readonly ISearchIndexNameService IndexNameService;

        public RobotsTextService(
            ISitecoreContext context,
            IProviderSearchContextFactory searchFactory, ISearchIndexNameService indexNameService)
        {
            SitecoreContext = context;
            SearchContextFactory = searchFactory;
            IndexNameService = indexNameService;
        }

        public string GetDisallowedGeneralContentUrls()
        {
            /*
            User-agent: *
            Disallow: /cyberworld/map/ 
            Disallow: /tmp/ 
            Disallow: /foo.html
            */
            
            IEnumerable<IGeneral_Content_Page> items = GetPages();
            if(!items.Any())
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (IGeneral_Content_Page p in items)
                sb.AppendLine($"Disallow: {p._Url}");

            return sb.ToString();
        }
        
        private IEnumerable<IGeneral_Content_Page> GetPages()
        {
            var home = SitecoreContext.GetHomeItem<IHome_Page>();
            var startPath = home._Path;
            using (var context = SearchContextFactory.Create(IndexNameService.GetIndexName()))
            {
                var query = context.GetQueryable<GeneralContentResult>()
                    .Filter(j => j.TemplateId == IGeneral_Content_PageConstants.TemplateId && j.Path.StartsWith(startPath.ToLower()) && j.ExcludeFromGoogleSearch);

                var results = query.GetResults();

                var pages = results.Hits.Select(h => SitecoreContext.GetItem<IGeneral_Content_Page>(h.Document.ItemId.Guid)).Where(a => a != null);
                return (!pages.Any())
                ? Enumerable.Empty<IGeneral_Content_Page>()
                : pages;
            }
        }
    }
}
