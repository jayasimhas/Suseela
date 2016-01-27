using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http.Dispatcher;
using System.Web.OData;
using System.Web.OData.Query;
using Microsoft.OData.Core.UriParser;
using Sitecore.ContentSearch.Linq;
using Velir.Search.Core.Models;
using Velir.Search.Core.Search.Queries;
using Velir.Search.OData.SearchResultItems;

namespace Velir.Search.OData.Queries
{
	[ODataQueryParameterBinding]
	public class SearchODataQueryOptions<T> : ODataQueryOptions<T>, ISearchQueryable<T> where T : CustomSearchResultItem
	{
		public FacetQueryOption<T> Facet { get; private set; }

		public SearchODataQueryOptions(ODataQueryOptions<T> options) : base(options.Context, options.Request)
		{
			
		} 

		public SearchODataQueryOptions(ODataQueryContext context, HttpRequestMessage request)
			: base(context, request)
		{
			BuildQueryOptions(request.GetQueryNameValuePairs());
		}

		private void BuildQueryOptions(IEnumerable<KeyValuePair<string, string>> queryParameters)
		{
			foreach (KeyValuePair<string, string> keyValuePair in queryParameters)
			{
				switch (keyValuePair.Key.ToLowerInvariant())
				{
					case "$facet":
						this.Facet = new FacetQueryOption<T>(keyValuePair.Value, this.Context);
						break;
				}
			}
		}

		public override IQueryable ApplyTo(IQueryable query, ODataQuerySettings querySettings)
		{
			var baseQuery = base.ApplyTo(query, querySettings);

			if (Facet != null)
			{
				baseQuery = Facet.ApplyTo(baseQuery);
			}
			
			return baseQuery;
		}

		public ISearchRequest SearchRequest { get; set; }

		public IQueryable<T> ApplyFilters(IQueryable<T> query)
		{
			return Filter.ApplyTo(query, new ODataQuerySettings()) as IQueryable<T>;
		}

		public IQueryable<T> ApplySorts(IQueryable<T> query)
		{
			return OrderBy.ApplyTo(query);
		}

		public IQueryable<T> ApplyFacets(IQueryable<T> query)
		{
			return Facet.ApplyTo(query) as IQueryable<T>;
		}

		public IQueryable<T> ApplyTake(IQueryable<T> query)
		{
			return Top.ApplyTo(query, new ODataQuerySettings());
		}

		public IQueryable<T> ApplySkip(IQueryable<T> query)
		{
			return Skip.ApplyTo(query, new ODataQuerySettings());
		}

		public IQueryable<T> ApplyAll(IQueryable<T> query)
		{
			return ApplyTo(query) as IQueryable<T>;
		}
	}

	public class FacetQueryOption<T> where T : CustomSearchResultItem
	{
		private static readonly IAssembliesResolver _defaultAssembliesResolver = (IAssembliesResolver)new DefaultAssembliesResolver();
		private ODataQueryOptionParser _queryOptionParser;

		public ODataQueryContext Context { get; private set; }

		public string RawValue { get; private set; }

		public FacetQueryOption(string rawValue, ODataQueryContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(rawValue))
				throw new ArgumentNullException("rawValue");
			this.Context = context;
			this.RawValue = rawValue;
			//this.Validator = new FilterQueryValidator();
			this._queryOptionParser = new ODataQueryOptionParser(context.Model, context.ElementType, context.NavigationSource, (IDictionary<string, string>)new Dictionary<string, string>()
      {
        {
          "$facet",
          rawValue
        }
      });
		}

		public IQueryable ApplyTo(IQueryable query)
		{
			return this.ApplyTo(query, _defaultAssembliesResolver);
		}

		public IQueryable ApplyTo(IQueryable query, IAssembliesResolver assembliesResolver)
		{
			if (query == null)
				throw new ArgumentNullException("query");
			if (assembliesResolver == null)
				throw new ArgumentNullException("assembliesResolver");

			var typeQuery = query as IQueryable<T>;

			if (typeQuery == null) return query;

			var facetFields = RawValue.Split(',');

			return facetFields.Aggregate(typeQuery, (current, facetField) => current.FacetOn(GenerateMemberExpression<string>(facetField), 1));
		}

		private Expression<Func<T, TK>> GenerateMemberExpression<TK>(string propertyName)
		{
			var propertyInfo = typeof(T).GetProperty(propertyName);

			var entityParam = Expression.Parameter(typeof(T), "e");
			Expression columnExpr = Expression.Property(entityParam, propertyInfo);

			if (propertyInfo.PropertyType != typeof(TK))
				columnExpr = Expression.Convert(columnExpr, typeof(TK));
			
			return Expression.Lambda<Func<T, TK>>(columnExpr, entityParam);
		}
	}
}
