using Informa.Library.Services.Search.Fields.Base;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Glass.Models;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
	public class SearchResultPublicationTitleField : BaseGlassComputedField<IGlassBase>
	{
		public override object GetFieldValue(IGlassBase indexItem)
		{
			ISite_Root rootItem = indexItem.Crawl<ISite_Root>();

			return rootItem?.Publication_Name ?? string.Empty;
		}
	}
}
