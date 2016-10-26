using System;
using System.Linq.Expressions;
using Informa.Library.Search.Results;
using Informa.Library.Utilities.References;
using Sitecore.ContentSearch.Linq.Utilities;
using Velir.Search.Core.Models;
using Velir.Search.Core.Page;
using Velir.Search.Core.PredicateBuilders;

namespace Informa.Library.Search.PredicateBuilders
{
    public class InformaPredicateBuilder<T> : SearchFilterPredicateBuilder<T> where T : InformaSearchResultItem
    {
        public DateTime DateRangeStart { get; set; }
        public DateTime DateRangeEnd { get; set; }

        private readonly ISearchRequest _request;

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

            if (_request.QueryParameters.ContainsKey("SearchPublicationTitle") && string.IsNullOrEmpty(_request.QueryParameters["SearchPublicationTitle"]) == false)
            {
                var pubs = _request.QueryParameters["SearchPublicationTitle"].Split(',');

                var codesPredicate = PredicateBuilder.True<T>();

                foreach (var item in pubs)
                {
                    codesPredicate = codesPredicate.Or(x => x.PublicationCode == item);
                }

                predicate = predicate.And(codesPredicate);
            }

            // If the inprogress flag is available then add that as as filter, this is used in VWB
            if (_request.QueryParameters.ContainsKey(Constants.QueryString.InProgressKey))
            {
                if (_request.QueryParameters[Constants.QueryString.InProgressKey] == "1")
                {
                    predicate = predicate.And(x => x.InProgress);
                }
            }

            // fiure-out time parameter
            if (_request.QueryParameters.ContainsKey(Constants.QueryString.DateRangeFilterLabelKey))
            {
                string dateRangeFilterLabelValue = _request.QueryParameters[Constants.QueryString.DateRangeFilterLabelKey];
                if (_request.QueryParameters.ContainsKey(Constants.QueryString.TimeKey))
                {
                    int timeQuery = -1 * Convert.ToInt32(_request.QueryParameters[Constants.QueryString.TimeKey]);

                    DateTime searchDate = DateTime.MinValue;
                    switch (dateRangeFilterLabelValue)
                    {
                        case "hour":
                            searchDate = DateTime.Now.AddHours(timeQuery);
                            break;
                        case "day":
                            searchDate = DateTime.Now.AddDays(timeQuery);
                            break;
                        case "year":
                            searchDate = DateTime.Now.AddYears(timeQuery);
                            break;
                        case "month":
                            searchDate = DateTime.Now.AddMonths(timeQuery);
                            break;
                        case "week":
                            searchDate = DateTime.Now.AddDays(timeQuery * 7);
                            break;
                    }

                    if (searchDate > DateTime.MinValue)
                    {
                        predicate = predicate.And(x => x.SearchDate > searchDate);
                    }
                }
            }

			if (_request.QueryParameters.ContainsKey(Constants.QueryString.Author))
			{
				predicate = predicate.And(x => x.Authors.Contains(_request.QueryParameters[Constants.QueryString.Author]));
			}
			if (_request.QueryParameters.ContainsKey(Constants.QueryString.AuthorFullName))
			{
				predicate = predicate.And(x => x.AuthorFullNames.Contains(_request.QueryParameters[Constants.QueryString.AuthorFullName]));
			}
			return predicate;
        }
    }
}