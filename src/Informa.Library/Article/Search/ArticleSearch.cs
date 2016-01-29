﻿using System.Linq;
using Glass.Mapper.Sc;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.ContentCuration.Search.Extensions;
using System.Collections.Generic;
using System;

namespace Informa.Library.Article.Search
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ArticleSearch : IArticleSearch
	{
		protected readonly ISitecoreService SitecoreContext;

		public ArticleSearch(
			ISitecoreService sitecoreContext)
		{
			SitecoreContext = sitecoreContext;
		}

		public IArticleSearchFilter CreateFilter()
		{
			return new ArticleSearchFilter
			{
				ExcludeManuallyCuratedItems = new List<Guid>()
			};
		}

		public IArticleSearchResults Search(IArticleSearchFilter filter)
		{
			var index = ContentSearchManager.GetIndex("sitecore_web_index");

			using (var context = index.CreateSearchContext())
			{
				var query = context.GetQueryable<ArticleSearchResultItem>().Filter(i => i.TemplateId == IArticleConstants.TemplateId);

				// Filter out manually curated content
				query = query.ExcludeManuallyCurated(filter);

				// Filter by subjects
				// Order by Actual Publish Date

				if (filter.PageSize > 0)
				{
					query = query.Page(filter.Page > 0 ? filter.Page - 1 : 0, filter.PageSize);
				}

				var results = query.GetResults();

				return new ArticleSearchResults
				{
					Articles = results.Hits.ToList().Select(h => SitecoreContext.GetItem<IArticle>(h.Document.ItemId.Guid))
				};
			}
		}
	}
}
