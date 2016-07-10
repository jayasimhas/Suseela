using System;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Glass.Models;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Sites;

namespace Informa.Library.Services.Search.Fields.Base
{
	public abstract class BaseGlassComputedField<T> : IComputedIndexField where T : class, IGlassBase
	{
		private const string SitecoreCoreDatabaseName = "core";

		public virtual string FieldName { get; set; }
		public virtual string ReturnType { get; set; }

		public object ComputeFieldValue(IIndexable indexable)
		{
			Item indexItem = indexable as SitecoreIndexableItem;

			// We don't care about the Core database, so don't do anything in that context
			if (indexItem?.Database == null || indexItem.Database.Name == SitecoreCoreDatabaseName)
			{
				return null;
			}

			string siteName = indexItem.GetSiteName();
			if (string.IsNullOrEmpty(siteName))
			{
				return null;
			}

			try
			{
				using (new SiteContextSwitcher(Factory.GetSite(siteName)))
				{
					return GetFieldValue(indexItem);
				}
			}
			catch (Exception e)
			{
				Sitecore.Diagnostics.Log.Error($"Error indexing field: {FieldName}, Type: {typeof(T)}, Item Path: {indexItem.Paths.FullPath}", e, GetType());
			}

			return null;
		}

		protected virtual object GetFieldValue(Item indexItem)
		{
			var glassItem = indexItem.GlassCast<IGlassBase>(true, true) as T;

			return glassItem != null ? GetFieldValue(glassItem) : null;
		}

		public abstract object GetFieldValue(T glassItem);
	}
}
