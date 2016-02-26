using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Glass.Mapper.Sc;
using Informa.Library.Search.ComputedFields.SearchResults.Converter;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Sitecore.Links;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.SearchResults
{


    public class SearchDisplayTaxonomyField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            List<HtmlLink> displayTaxonomies = new List<HtmlLink>();

            I___BaseTaxonomy taxonomyItem = indexItem.GlassCast<I___BaseTaxonomy>(inferType: true);

            if (taxonomyItem == null)
            {
                return string.Empty;
            }

            if (taxonomyItem.Taxonomies == null)
            {
                return string.Empty;
            }

            if (!taxonomyItem.Taxonomies.Any())
            {
                return string.Empty;
            }

            int count = 0;
            foreach (ITaxonomy_Item taxonomy in taxonomyItem.Taxonomies)
            {
                displayTaxonomies.Add(new HtmlLink() {Title = taxonomy.Item_Name.Trim(), Url = taxonomy._Url});
                count++;

                if (count == 2)
                {
                    break;
                }
            }

            HtmlLinkList links = new HtmlLinkList()
                                 {
                                     Links = displayTaxonomies
            };

        

            return new JavaScriptSerializer().Serialize(links);
        }
    }
}
