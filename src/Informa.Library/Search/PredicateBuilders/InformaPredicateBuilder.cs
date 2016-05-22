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

            if (Sitecore.Configuration.Settings.GetBoolSetting("Search.NewerArticlesBoosting.Enabled", false))
            {
                //Boost newer articles (See http://www.sitecoreblogger.com/2014/09/publication-date-boosting-in-sitecore-7.html)
                predicate = predicate.And(x => x.Val == Sitecore.Configuration.Settings.GetSetting("Search.NewerArticlesBoosting.BoostFunction"));
            }

            //Include Search for authors
            predicate = predicate.Or(x => x.Byline.Contains(_request.QueryParameters["q"]));

            // If the inprogress flag is available then add that as as filter, this is used in VWB
            if (_request.QueryParameters.ContainsKey("inprogress"))
            {
                if (_request.QueryParameters["inprogress"] == "1")
                {
                    predicate = predicate.And(x => x.InProgress);
                }
            }

            //Date Searching
            //if (DateRangeStart > DateTime.MinValue)
            //{
            //    predicate = predicate.And(x => x.SearchDate > DateRangeStart);
            //    DateRangeEnd = DateRangeEnd != DateTime.MinValue ? DateRangeEnd : DateTime.Now;
            //    predicate = predicate.And(x => x.SearchDate < DateRangeEnd);
            //}

            //Add boosting if there is a search term
            //if (_request?.QueryParameters.Count > 0)
            //{
            //    if (_request.QueryParameters.ContainsKey("q"))
            //    {
            //        string searchTerm = _request.QueryParameters["q"];
            //        predicate =
            //            predicate.And(
            //                x => x.Summary.Contains(searchTerm).Boost(4) || x.Title.Contains(searchTerm).Boost(5) || x.SubTitle.Contains(searchTerm).Boost(3));
            //    }
            //}

            // fiure-out time parameter
            if (_request.QueryParameters.ContainsKey("dateFilterLabel"))
            {
                if (_request.QueryParameters.ContainsKey("time"))
                {
                    int timeQuery = System.Convert.ToInt32(_request.QueryParameters["time"]);

                    if (_request.QueryParameters["dateFilterLabel"] == "hour")
                    {

                        int days = -1 * System.Convert.ToInt32(0.0416666667 * timeQuery);

                        DateTime searcheddate = DateTime.Now.AddDays(days);
                        predicate = predicate.And(x => x.SearchDate > searcheddate);
                    }

                    if (_request.QueryParameters["dateFilterLabel"] == "day")
                    {
                        int days = -1 * timeQuery;

                        DateTime searcheddate = DateTime.Now.AddDays(days);
                        predicate = predicate.And(x => x.SearchDate > searcheddate);
                    }

                    if (_request.QueryParameters["dateFilterLabel"] == "year")
                    {
                        int years = -1 * timeQuery;

                        DateTime searcheddate = DateTime.Now.AddYears(years);
                        predicate = predicate.And(x => x.SearchDate > searcheddate);
                    }

                    if (_request.QueryParameters["dateFilterLabel"] == "month")
                    {
                        int months = -1 * timeQuery;

                        DateTime searcheddate = DateTime.Now.AddMonths(months);
                        predicate = predicate.And(x => x.SearchDate > searcheddate);
                    }

                    if (_request.QueryParameters["dateFilterLabel"] == "week")
                    {
                        int weeks = -1 * timeQuery * 7;

                        DateTime searcheddate = DateTime.Now.AddDays(weeks);
                        predicate = predicate.And(x => x.SearchDate > searcheddate);
                    }

                }

            }

            return predicate;
        }
    }

}