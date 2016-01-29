using Sitecore.ContentSearch.ComputedFields;
using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.ContentSearch;
using Informa.Library.Presentation;
using Autofac;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore;

namespace Informa.Library.ContentCuration.Search.ComputedIndexField
{
	public class ManuallyCuratedContent : IComputedIndexField
	{
		protected IDefaultItemRenderingsFactory ItemRenderingsFactory { get { return AutofacConfig.ServiceLocator.Resolve<IDefaultItemRenderingsFactory>(); } }
		protected IDuplicateRenderingsFactory DuplicateRenderingsFactory { get { return AutofacConfig.ServiceLocator.Resolve<IDuplicateRenderingsFactory>(); } }

		public string FieldName { get; set; }
		public string ReturnType { get; set; }

		public object ComputeFieldValue(IIndexable indexable)
		{
			var manuallyCuratedContent = new List<Guid>();
			Item item = indexable as SitecoreIndexableItem;

			if (item == null)
			{
				return manuallyCuratedContent;
			}

			using (new DatabaseSwitcher(Database.GetDatabase("master")))
			{
				var duplicateRenderings = DuplicateRenderingsFactory.Create();
				var itemRenderings = ItemRenderingsFactory.Get(item);
				var matchingDuplicateRenderings = itemRenderings.Where(ir => !string.IsNullOrEmpty(ir.Datasource) && duplicateRenderings.Any(dr => dr.RenderingItemId == ir.RenderingItemId));

				foreach (var rendering in matchingDuplicateRenderings)
				{
					// Check data source to see if item is of a specific template (optional)
					Guid manuallyCuratedItemId;

					if (!Guid.TryParse(rendering.Datasource, out manuallyCuratedItemId))
					{
						var manuallyCuratedItem = Context.Database.GetItem(rendering.Datasource);

						if (manuallyCuratedItem == null)
						{
							continue;
						}

						manuallyCuratedItemId = manuallyCuratedItem.ID.Guid;
					}

					if (!manuallyCuratedContent.Contains(manuallyCuratedItemId))
					{
						manuallyCuratedContent.Add(manuallyCuratedItemId);
					}
				}
			};

			return manuallyCuratedContent;
		}
	}
}
