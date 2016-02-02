﻿using Informa.Library.Search;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.ContentSearch.Linq;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.ContentCuration.Search
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ItemManuallyCuratedContentSearch : IItemManuallyCuratedContent
	{
		protected readonly IProviderSearchContextFactory SearchContextFactory;

		public ItemManuallyCuratedContentSearch(
			IProviderSearchContextFactory searchContextFactory)
		{
			SearchContextFactory = searchContextFactory;
		}

		public IEnumerable<Guid> Get(Guid itemId)
		{
			using (var context = SearchContextFactory.Create())
			{
				var query = context.GetQueryable<ManuallyCuratedContentSearchResult>().Filter(i => i.ItemId == ID.Parse(itemId));
				var results = query.GetResults();
				var result = results.Hits.FirstOrDefault();

				if (result == null || result.Document.ManuallyCuratedContent == null)
				{
					return Enumerable.Empty<Guid>();
				}

				return result.Document.ManuallyCuratedContent;
			}
		}
	}
}
