using System;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
	public class SearchResultDateField : BaseGlassComputedField<IArticle>
	{
		protected override object GetFieldValue(Item indexItem)
		{
			var result = base.GetFieldValue(indexItem);

			if(indexItem == null) return result;

			if (result == null) return indexItem.Statistics.Updated;

			return result;
		}

		public override object GetFieldValue(IArticle articleItem)
		{
			if (articleItem.Actual_Publish_Date != DateTime.MinValue) return articleItem.Actual_Publish_Date;

			if (articleItem.Created_Date != DateTime.MinValue) return articleItem.Created_Date;

			return null;
		}
	}
}