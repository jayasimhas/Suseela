using System.Collections.Generic;
using System.Linq;
using Informa.Library.Search.Utilities;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Informa.Library.Search.ComputedFields.Facets
{
	public class SubjectsField : BaseGlassComputedField<I___BaseTaxonomy>
	{
		public override object GetFieldValue(I___BaseTaxonomy indexItem)
		{
			if (indexItem?.Taxonomies != null)
			{
                var subjectTaxonomyItems = indexItem.Taxonomies.Where(x => SearchTaxonomyUtil.IsSubjectTaxonomy(x._Path));

                return SearchTaxonomyUtil.GetHierarchicalFacetFieldValue(subjectTaxonomyItems);
            }
			return new List<string>();
		}
	}
}
