using System.Linq;
using Glass.Mapper.Sc;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.ContentSearch.Linq;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.ContentCuration.Search.Extensions;
using Informa.Library.Search.Extensions;
using System.Collections.Generic;
using System;
using Informa.Library.Search;
using Informa.Library.Utilities.References;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;

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
					.FilteryByEScenicID(filter);

				if (filter.PageSize > 0)
				{
					query = query.Page(filter.Page > 0 ? filter.Page - 1 : 0, filter.PageSize);
				}

				query = query.OrderByDescending(i => i.ActualPublishDate);

				var results = query.GetResults();

				return new ArticleSearchResults
				{
					Articles = results.Hits.ToList().Select(h => SitecoreContext.GetItem<IArticle>(h.Document.ItemId.Guid))
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
					Articles = results.Hits.ToList().Select(h => localSearchContext.GetItem<IArticle>(h.Document.ItemId.Guid))
				};
			}
		}
		public int GetNextArticleNumber(Guid publicationGuid)
		{
			using (var context = SearchContextFactory.Create())
			{
				var parent = SitecoreContext.GetItem<Item>(publicationGuid);
				//var result = context.GetQueryable<ArticleSearchResultItem>().
				//	Filter(i => i.TemplateId == IArticleConstants.TemplateId && i.Path.Contains(parent.Paths.FullPath)).OrderByDescending(i => i.ArticleNumber).FirstOrDefault();

				string parPath = parent.Paths.FullPath.ToLower();
				//var result = context.GetQueryable<ArticleSearchResultItem>().Filter(i => i.TemplateId == IArticleConstants.TemplateId);
				var result = context.GetQueryable<ArticleSearchResultItem>()
					.Where(i => i.TemplateId == IArticleConstants.TemplateId && i.Path.StartsWith(parPath)).ToList();
				var filteredList = result.Where( i=>  i.ArticleNumber != null).OrderBy(i => i.ArticleNumber).FirstOrDefault();

				if (result == null) return 0;
				//string num = result.Replace(GetPublicationPrefix(publicationGuid), "");
				int n;
				//return (int.TryParse(num, out n)) ? n + 1 : 0;
				return 0;
			}
		}

		/// <summary>
		/// This method gets the Publication Prefix which is used in Article Number Generation.
		/// </summary>
		/// <param name="publicationGuid"></param>
		/// <returns></returns>
		public static string GetPublicationPrefix(Guid publicationGuid)
		{
			var publicationPrefixDictionary =
				new Dictionary<Guid, string>
					{
						{new Guid("{3818C47E-4B75-4305-8F01-AB994150A1B0}"), "SC"},
					};

			string value;
			return publicationPrefixDictionary.TryGetValue(publicationGuid, out value) ? value : null;
		}

		public static string GetArticleCustomPath(IArticle a)
		{
			string procName = a._Name.Replace(" ", "-");
			return $"/{a.Article_Number}/{procName}";
		}
	}
}
