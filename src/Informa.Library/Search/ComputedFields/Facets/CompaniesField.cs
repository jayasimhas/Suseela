using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc;
using Informa.Library.Search.Utilities;
using Informa.Model.DCD;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.Facets
{
    public class CompaniesField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            if (indexItem.TemplateID != IArticleConstants.TemplateId)
            {
                return new List<string>();
            }

            IArticle article = indexItem.GlassCast<IArticle>(inferType: true);

            if (string.IsNullOrEmpty(article.Referenced_Companies))
            {
                return new List<string>();
            }

            return SearchCompanyUtil.GetCompanyNames(article.Referenced_Companies);
        }
    }
}
