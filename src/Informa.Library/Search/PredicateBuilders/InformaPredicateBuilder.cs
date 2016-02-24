using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Informa.Library.Search.Results;
using Sitecore;
using Sitecore.ContentSearch.Linq;
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

        private ISearchRequest _request;


        public InformaPredicateBuilder(ISearchPageParser pageParser, ISearchRequest request = null)
            : base(pageParser, request)
        {
            _request = request;
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

            //Add boosting if there is a search term
            if (_request?.QueryParameters.Count > 0)
            {
                if (_request.QueryParameters.ContainsKey("q"))
                {
                    string searchTerm = _request.QueryParameters["q"];
                    predicate =
                        predicate.And(
                            x => x.Summary.Contains(searchTerm).Boost(4) || x.Title.Contains(searchTerm).Boost(5) || x.SubTitle.Contains(searchTerm).Boost(3));
                }
            }

            return predicate;
        }
    }

}