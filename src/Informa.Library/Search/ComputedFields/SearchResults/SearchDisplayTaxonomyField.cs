using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Informa.Library.Search.ComputedFields.SearchResults.Converter;
using Informa.Library.Search.Utilities;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
	public class SearchDisplayTaxonomyField : BaseGlassComputedField<I___BaseTaxonomy>
	{
		public override object GetFieldValue(I___BaseTaxonomy indexItem)
		{
			List<HtmlLink> displayTaxonomies = new List<HtmlLink>();

			if (indexItem?.Taxonomies == null || !indexItem.Taxonomies.Any())
			{
				return string.Empty;
			}

			int count = 0;
			foreach (ITaxonomy_Item taxonomy in indexItem.Taxonomies)
			{
				displayTaxonomies.Add(new HtmlLink { Title = taxonomy.Item_Name?.Trim() ?? string.Empty, Url = SearchTaxonomyUtil.GetSearchUrl(taxonomy) });
				count++;

				if (count == 3)
				{
					break;
				}
			}

			HtmlLinkList links = new HtmlLinkList
			{
				Links = displayTaxonomies
			};
			
			return new JavaScriptSerializer().Serialize(links);
		}
	}
}
