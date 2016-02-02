using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Glass.Mapper.Sc;
using Newtonsoft.Json;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Models;
using Velir.Search.Core.Refinements;
using Velir.Search.Core.Search.Results.Facets;

namespace Velir.Search.Core.Search.Results
{
	[DataContract]
	[JsonObject]
	public class QueryResults<T> : IQueryResults where T : SearchResultItem
	{
		private readonly Lazy<int> _totalResults;
		private readonly Lazy<IEnumerable<FacetGroup>> _facets;
		private readonly Lazy<IEnumerable<ISearchRefinement>> _availableRefinements;

		[DataMember]
		[JsonProperty]
		public ISearchRequest Request { get; private set; }

		[DataMember]
		[JsonProperty]
		public long TotalResults { get { return _totalResults.Value; } }

		[DataMember]
		[JsonProperty]
		public IList Results
		{
			get;
			set;
		}

		[DataMember]
		[JsonProperty]
		public IEnumerable<FacetGroup> Facets { get { return _facets.Value; } }

		public QueryResults(ISearchRequest request, SearchResults<T> results, FacetResults facets)
		{
			Request = request;
			Results = results.Hits.Select(x => x.Document).Where(x => x != null).ToList();

			_totalResults = new Lazy<int>(() => results.TotalSearchResults);
			_facets = new Lazy<IEnumerable<FacetGroup>>(() => facets == null
				? new List<FacetGroup>()
				: BuildFacetResults(request.QueryParameters, facets.Categories));

			_availableRefinements = new Lazy<IEnumerable<ISearchRefinement>>(request.GetRefinements);
		}

		public virtual IList<TResult> GetModels<TResult>() where TResult : class
		{
			return ((IList<T>)Results).Select(result => result.GetItem().GlassCast<TResult>(inferType: true)).ToList();
		}

		private IEnumerable<FacetGroup> BuildFacetResults(IDictionary<string, string> queryParams, IEnumerable<FacetCategory> categories)
		{
			return from category in categories let id = GetFacetGroupId(category.Name) let label = GetFacetGroupLabel(category.Name) select new FacetGroup
			{
				Id = id,
				Label = label,
				Values = category.Values.Select(r => new FacetResultValue(r.Name, r.AggregateCount, queryParams.ContainsKey(id) && queryParams[id].Split(',').Select(HttpUtility.HtmlDecode).Contains(r.Name)))
			};
		}

		private string GetFacetGroupId(string fieldName)
		{
			var facet = _availableRefinements.Value.FirstOrDefault(x => x.FieldName == fieldName);

			return facet != null ? facet.RefinementKey : string.Empty;
		}

		private string GetFacetGroupLabel(string fieldName)
		{
			var facet = _availableRefinements.Value.FirstOrDefault(x => x.FieldName == fieldName);

			return facet != null ? facet.RefinementLabel : string.Empty;
		}
	}
}
