using Glass.Mapper.Sc;
using Informa.Library.Search.Results;
using System;
using Velir.Search.Core.Managers;

namespace Informa.Library.Search.SearchManager
{
    public class MaritimeSearchManager
    {
        private static ISearchManager<InformaSearchResultItem> _searchManager;
        private static object syncRoot = new Object();

        public static ISearchManager<InformaSearchResultItem> CreateInstance(string indexName, ISitecoreContext context)
        {
            if (_searchManager == null)
            {
                lock (syncRoot)
                {
                    if (_searchManager == null)
                    {
                        _searchManager = new SearchManager<InformaSearchResultItem>(indexName, context);
                    }
                }
            }

            return _searchManager;
        }
    }
}


