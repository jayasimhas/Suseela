using Glass.Mapper.Sc;
using Informa.Library.Search.Results;
using Informa.Library.Search.SearchIndex;
using Informa.Library.Search.SearchManager;
using Informa.Library.Site;
using System.Web.Http;
using Velir.Search.Core.Managers;
using System;


namespace Informa.Web.Controllers.Search
{
    public class InformaBaseSearchController : ApiController
    {
        public ISearchManager<InformaSearchResultItem> _searchManager;

        
        public InformaBaseSearchController(ISiteRootContext siterootContext, ISitecoreContext context, ISearchIndexNameService indexNameService)
        {
            // Create search manager instance based on the querystring 'verticalroot'.
            string vertical = indexNameService.GetVerticalRootFromQuerystring();
            if (!string.IsNullOrEmpty(vertical))
                _searchManager = SearchManagerFactory.CreateSearchManager(indexNameService.GetIndexName(new Guid(vertical)), context);
            else
                _searchManager = SearchManagerFactory.CreateSearchManager(indexNameService.GetIndexName(), context);
        }
       

       
        }
}