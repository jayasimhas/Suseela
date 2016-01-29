using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.ContentSearch;
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
		public ItemManuallyCuratedContentSearch(
			)
		{

		}

		public IEnumerable<Guid> Get(Guid itemId)
		{
			var index = ContentSearchManager.GetIndex("sitecore_web_index");

			using (var context = index.CreateSearchContext())
			{
				var query = context.GetQueryable<ManuallyCuratedContentSearchResult>().Filter(i => i.ItemId == ID.Parse(itemId));
				var results = query.GetResults();
				var result = results.Hits.FirstOrDefault();

				if (result == null)
				{
					return Enumerable.Empty<Guid>();
				}

				return result.Document.ManuallyCuratedContent;
			}
		}
	}
}
