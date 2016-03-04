using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc;
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
            return string.Empty;

            //if (indexItem.TemplateID != IArticleConstants.TemplateId)
            //{
            //    return string.Empty;
            //}

            //IArticle article = indexItem.GlassCast<IArticle>(inferType: true);

            //if (string.IsNullOrEmpty(article.Referenced_Companies))
            //{
            //    return string.Empty;
            //}

            //var company = new DCDManager().GetCompanyByRecordNumber(article.Referenced_Companies);

            //if (company == null)
            //{
            //    return string.Empty;
            //}

            //return company.Title;
        }
    }
}
