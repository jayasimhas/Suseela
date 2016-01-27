using Sitecore.Data.Items;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Sites;
using SiteFactory = Sitecore.Configuration.Factory;

namespace Velir.Search.Core.Search.ComputedFields
{
	public abstract class BaseContentComputedField : IComputedIndexField
	{
		public virtual string FieldName { get; set; }
		public virtual string ReturnType { get; set; }

		public object ComputeFieldValue(IIndexable indexable)
		{
			Item indexItem = indexable as SitecoreIndexableItem;

			if (indexItem == null) return null;

			if (indexItem.Database == null || indexItem.Database.Name == "core")
			{
				return null;
			}

			using (new SiteContextSwitcher(SiteFactory.GetSite("website")))
			{
				return GetFieldValue(indexItem);
			}
		}

		public abstract object GetFieldValue(Item indexItem);
	}
}
