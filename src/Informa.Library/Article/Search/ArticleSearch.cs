using Glass.Mapper.Sc;
using Jabberwocky.Glass.Autofac.Attributes;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.ContentCuration.Search.Extensions;
using Informa.Library.Search.Extensions;
using System.Collections.Generic;
using System;
using System.Linq;
using Informa.Library.Search;
using Informa.Library.Utilities.References;
using Sitecore.Buckets.Extensions;
using Sitecore.Configuration;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Abstractions;
using Sitecore.ContentSearch.Diagnostics;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Linq.Methods;
using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.ContentSearch.Linq.Solr;

namespace Informa.Library.Article.Search
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ArticleSearch : IArticleSearch
	{
		protected readonly IProviderSearchContextFactory SearchContextFactory;
		protected readonly ISitecoreService SitecoreContext;
		protected readonly Func<string, ISitecoreService> SitecoreFactory;

		public ArticleSearch(
			IProviderSearchContextFactory searchContextFactory,
			ISitecoreService sitecoreContext, Func<string, ISitecoreService> sitecoreFactory
			)
		{
			SearchContextFactory = searchContextFactory;
			SitecoreContext = sitecoreContext;
			SitecoreFactory = sitecoreFactory;
		}

		public IArticleSearchFilter CreateFilter()
		{
			return new ArticleSearchFilter
			{
				ExcludeManuallyCuratedItems = new List<Guid>(),
				TaxonomyIds = new List<Guid>(),
				ArticleNumber = string.Empty
			};
		}

		public IArticleSearchResults Search(IArticleSearchFilter filter)
		{
			using (var context = SearchContextFactory.Create())
			{
				var query = context.GetQueryable<ArticleSearchResultItem>()
					.Filter(i => i.TemplateId == IArticleConstants.TemplateId)
					.FilterTaxonomies(filter)
					.ExcludeManuallyCurated(filter)
					.FilteryByArticleNumber(filter)
					.FilteryByEScenicID(filter)
					.FilteryByRelatedId(filter);

				if (filter.PageSize > 0)
				{
					query = query.Page(filter.Page > 0 ? filter.Page - 1 : 0, filter.PageSize);
				}

				query = query.OrderByDescending(i => i.ActualPublishDate);

				var results = query.GetResults();

				return new ArticleSearchResults
				{
					Articles = results.Hits.Select(h => SitecoreContext.GetItem<IArticle>(h.Document.ItemId.Guid))
				};
			}
		}

		/// <summary>
		/// This is a search implementatation where you want to pass the database name along with the filter.
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="database"></param>
		/// <returns></returns>
		public IArticleSearchResults SearchCustomDatabase(IArticleSearchFilter filter, string database)
		{
			using (var context = SearchContextFactory.Create(database))
			{
				var query = context.GetQueryable<ArticleSearchResultItem>()
					.Filter(i => i.TemplateId == IArticleConstants.TemplateId)
					.FilterTaxonomies(filter)
					.ExcludeManuallyCurated(filter)
					.FilteryByArticleNumber(filter)
					.FilteryByEScenicID(filter);

				if (filter.PageSize > 0)
				{
					query = query.Page(filter.Page > 0 ? filter.Page - 1 : 0, filter.PageSize);
				}

				query = query.OrderByDescending(i => i.ActualPublishDate);
				ISitecoreService localSearchContext = SitecoreFactory(database);
				var results = query.GetResults();
				return new ArticleSearchResults
				{
					Articles = results.Hits.Select(h => localSearchContext.GetItem<IArticle>(h.Document.ItemId.Guid))
				};
			}
		}

		public long GetNextArticleNumber(Guid publicationGuid)
		{
			using (var context = SearchContextFactory.Create("master"))
			{
				var filter = CreateFilter();

				var query = context.GetQueryable<ArticleSearchResultItem>()
					.Filter(i => i.TemplateId == IArticleConstants.TemplateId)
					.FilterTaxonomies(filter)
					//.Max(x => x.ArticleIntegerNumber);
					.OrderByDescending(i => i.ArticleIntegerNumber)
					.Take(1);

				var results = query.GetResults();


				return results.Hits.FirstOrDefault().Document.ArticleIntegerNumber + 1;


				//var results = articleSearchResultItem.GetResults();


				//var articleResults2 = context.GetQueryable<ArticleSearchResultItem>()     Max(x => x.ArticleIntegerNumber);


				//return results.Hits.FirstOrDefault().Document.ArticleIntegerNumber;
			}
			return 0;
		}

		/// <summary>
		/// This method gets the Publication Prefix which is used in Article Number Generation.
		/// </summary>
		/// <param name="publicationGuid"></param>
		/// <returns></returns>
		public static string GetPublicationPrefix(Guid publicationGuid)
		{
			string value;
			return Constants.PublicationPrefixDictionary.TryGetValue(publicationGuid, out value) ? value : null;
		}

		public static string GetArticleCustomPath(IArticle a)
		{
			string procName = a._Name.Replace(" ", "-");
			return $"/{a.Article_Number}/{procName}";
		}
	}
}
