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
                Item item = Sitecore.Context.ContentDatabase.GetItem(new ID(indexItem._Id));
                var rootItem = item.Axes.GetAncestors().FirstOrDefault(ancestor => ancestor.TemplateID.ToString() == "{DE3615F6-1562-4CB4-80EA-7FA45F49B7B7}");

                var subjectTaxonomyItems = indexItem.Taxonomies.Where(x => SearchTaxonomyUtil.IsSubjectTaxonomy(x._Path, rootItem.Name));

                return SearchTaxonomyUtil.GetHierarchicalFacetFieldValue(subjectTaxonomyItems);
            }
			return new List<string>();
		}
	}
}
