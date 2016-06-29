using System.Linq;
using System.Web.Script.Serialization;
using Informa.Library.Search.ComputedFields.SearchResults.Converter;
using Informa.Library.Search.Utilities;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
	public class SearchDisplayTaxonomyField : BaseGlassComputedField<I___BaseTaxonomy>
	{
		public override object GetFieldValue(I___BaseTaxonomy indexItem)
		{
			if (indexItem?.Taxonomies == null || !indexItem.Taxonomies.Any())
			{
				return string.Empty;
			}

			HtmlLinkList links = new HtmlLinkList
			{
				Links = indexItem.Taxonomies.Take(2).Select(t => new HtmlLink { Title = t.Item_Name?.Trim() ?? string.Empty, Url = SearchTaxonomyUtil.GetSearchUrl(t) }).ToList()
			};
			
			return new JavaScriptSerializer().Serialize(links);
		}
	}
}
