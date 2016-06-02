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
using Sitecore.ContentSearch.Linq;
using System.Text;
using Informa.Library.Services.Global;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Core.Caching;
using Constants = Informa.Library.Utilities.References.Constants;

namespace Informa.Library.Article.Search
{
	[AutowireService]
	public class ArticleSearch : IArticleSearch
	{
		protected readonly IProviderSearchContextFactory SearchContextFactory;
		protected readonly IGlobalSitecoreService GlobalService;
		protected readonly Func<string, ISitecoreService> SitecoreFactory;
		protected readonly IItemReferences ItemReferences;
		protected readonly ICacheProvider CacheProvider;

		public ArticleSearch(
				IProviderSearchContextFactory searchContextFactory,
				IGlobalSitecoreService globalService,
				Func<string, ISitecoreService> sitecoreFactory,
				IItemReferences itemReferences,
				ICacheProvider cacheProvider
				)
		{
			SearchContextFactory = searchContextFactory;
			GlobalService = globalService;
			SitecoreFactory = sitecoreFactory;
			ItemReferences = itemReferences;
			CacheProvider = cacheProvider;
		}

		public IArticleSearchFilter CreateFilter()
		{
			return new ArticleSearchFilter
			{
				ExcludeManuallyCuratedItems = new List<Guid>(),
				TaxonomyIds = new List<Guid>(),
				ArticleNumbers = new List<string>(),
				PublicationNames = new List<string>()
			};
		}

		public IArticleSearchResults Search(IArticleSearchFilter filter)
		{
			using (var context = SearchContextFactory.Create())
			{
				var query = context.GetQueryable<ArticleSearchResultItem>()
						.Filter(i => i.TemplateId == IArticleConstants.TemplateId)
						.FilterByPublications(filter)
						.FilterTaxonomies(filter)
						.ExcludeManuallyCurated(filter)
						.FilteryByArticleNumbers(filter)
						.FilteryByEScenicID(filter)
						.FilteryByRelatedId(filter)
						.ApplyDefaultFilters();

				if (filter.PageSize > 0)
				{
					query = query.Page(filter.Page > 0 ? filter.Page - 1 : 0, filter.PageSize);
				}

				query = query.OrderByDescending(i => i.ActualPublishDate);

				var results = query.GetResults();

				return new ArticleSearchResults
				{
					Articles = results.Hits.Select(h => GlobalService.GetItem<IArticle>(h.Document.ItemId.Guid))
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
					.FilteryByArticleNumbers(filter)
					.FilteryByEScenicID(filter)
					.ApplyDefaultFilters();

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
			using (var context = SearchContextFactory.Create(Constants.MasterDb))
			{
				var publicationItem = GlobalService.GetItem<ISite_Root>(publicationGuid);
				if (publicationItem != null)
				{
					var filter = CreateFilter();

					var query = context.GetQueryable<ArticleSearchResultItem>()
							.Filter(i => i.TemplateId == IArticleConstants.TemplateId)
							.Filter(i => i.PublicationTitle == publicationItem.Publication_Name)
							.FilterTaxonomies(filter)
							.OrderByDescending(i => i.ArticleIntegerNumber)
							.Take(1);

					var results = query.GetResults();

					return results?.Hits?.FirstOrDefault()?.Document?.ArticleIntegerNumber + 1 ?? 0;
				}
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
			string value;
			return Constants.PublicationPrefixDictionary.TryGetValue(publicationGuid, out value) ? value : null;
		}

		public static string GetArticleCustomPath(IArticle a)
		{
			string procName = a._Name.Replace(" ", "-");
			return $"/{a.Article_Number}/{procName}";
		}

		public string GetArticleTaxonomies(Guid id, Guid taxonomyParent)
		{
			string cacheKey = $"{nameof(ArticleSearch)}-GetTaxonomy-{id}";
			return CacheProvider.GetFromCache(cacheKey, () => BuildArticleTaxonomies(id, taxonomyParent));
		}

		public string BuildArticleTaxonomies(Guid id, Guid taxonomyParent)
		{
			var article = GlobalService.GetItem<ArticleItem>(id);
			var taxonomyItems = article?.Taxonomies?.Where(x => x._Parent._Id.Equals(taxonomyParent));

			if (taxonomyItems != null)
			{
				StringBuilder str = new StringBuilder();
				foreach (ITaxonomy_Item taxonomyItem in taxonomyItems)
				{
					if (str.Length > 0)
						str.Append(",");
					str.Append($"'{taxonomyItem.Item_Name.Trim()}'");
				}
				return $"[{str}]";
			}

			return string.Empty;
		}

		public IArticleSearchResults GetLegacyArticleUrl(string path)
		{
			path = path.ToLower();
			using (var context = SearchContextFactory.Create())
			{
				var query = context.GetQueryable<ArticleSearchResultItem>()
					.Filter(i => i.TemplateId == IArticleConstants.TemplateId)
					.Filter(i => i.LegacyArticleUrl == path);
				var results = query.GetResults();

				return new ArticleSearchResults
				{
					Articles = results.Hits.Select(i => GlobalService.GetItem<IArticle>(i.Document.ItemId.Guid))
				};
			}
		}
	}
}
