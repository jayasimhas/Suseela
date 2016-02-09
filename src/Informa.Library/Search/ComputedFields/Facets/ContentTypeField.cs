using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.Facets
{
    public class ContentTypeField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            I___BaseTaxonomy taxonomItem = indexItem.GlassCast<I___BaseTaxonomy>(inferType: true);

            if (taxonomItem?.Taxonomies != null)
            {
                var subjectTaxonomyItems =
                    taxonomItem.Taxonomies.Where(
                        x =>
                            x._Path.ToLower()
                                .StartsWith("/sitecore/content/scripintelligence/globals/taxonomy/content types"));

                return subjectTaxonomyItems.Select(x => x.Item_Name).ToList();
            }
            return new List<string>();
        }
    }
}
