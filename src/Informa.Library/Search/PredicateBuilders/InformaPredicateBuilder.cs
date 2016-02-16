using System;
using System.Linq.Expressions;
using Informa.Library.Search.Results;
using Sitecore.ContentSearch.Linq.Utilities;
using Velir.Search.Core.Models;
using Velir.Search.Core.Page;
using Velir.Search.Core.PredicateBuilders;

namespace Informa.Library.Search.PredicateBuilders
{
    public class InformaPredicateBuilder<T> : SearchPredicateBuilder<T> where T : InformaSearchResultItem
    {
        public DateTime DateRangeStart { get; set; }
        public DateTime DateRangeEnd { get; set; }

        public InformaPredicateBuilder(ISearchPageParser pageParser, ISearchRequest request = null)
            : base(pageParser, request)
        {
        }

        public override Expression<Func<T, bool>> Build()
        {
            var predicate = base.Build();

            predicate = predicate.And(x => x.IsSearchable);
            predicate = predicate.And(x => x.IsLatestVersion);

            //Date Searching
            if (DateRangeStart > DateTime.MinValue)
            {
                predicate = predicate.And(x => x.SearchDate > DateRangeStart);
                DateRangeEnd = DateRangeEnd != DateTime.MinValue ? DateRangeEnd : DateTime.Now;
                predicate = predicate.And(x => x.SearchDate < DateRangeEnd);
            }

            return predicate;
        }
    }
}