namespace Informa.Library.Search.SearchManager
{
    using Glass.Mapper.Sc;
    using Results;
    using System;
    using Velir.Search.Core.Managers;
    public class PharmaSearchManager
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
