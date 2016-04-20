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

			// If the inprogress flag is available then add that as as filter, this is used in VWB
			if (_request.QueryParameters.ContainsKey(Constants.QueryString.InProgressKey))
			{
				if (_request.QueryParameters[Constants.QueryString.InProgressKey] == "1")
				{
					predicate = predicate.And(x => x.InProgress);
				}
			}

			return predicate;
		}
	}
}