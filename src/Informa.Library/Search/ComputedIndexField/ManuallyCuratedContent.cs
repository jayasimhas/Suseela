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

namespace Informa.Library.Search.ComputedIndexField
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
				var dupeRenderings = DuplicateRenderingsFactory.Create();
				var itemRenderings = ItemRenderingsFactory.Get(item);
				var matchingDupeRenderings = itemRenderings.Where(ir => dupeRenderings.Any(dr => dr.RenderingItemId == ir.RenderingItemId));
				var datasourceItems = matchingDupeRenderings.Select(r => Context.Database.GetItem(r.Datasource)).Where(i => i != null);
				// Check data source to see if item is of a specific template (optional)

				manuallyCuratedContent.AddRange(datasourceItems.Select(i => i.ID.Guid));
			};

			return manuallyCuratedContent;
		}
	}
}
