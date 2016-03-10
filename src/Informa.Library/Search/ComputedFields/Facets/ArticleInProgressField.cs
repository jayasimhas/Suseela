using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Sitecore.Shell.Applications.ContentEditor;
using Velir.Search.Core.ComputedFields;
using DateTime = System.DateTime;

namespace Informa.Library.Search.ComputedFields.Facets
{
    public class ArticleInProgressField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            if (indexItem.TemplateID != IArticleConstants.TemplateId)
            {
                return false;
            }

            IArticle article = indexItem.GlassCast<IArticle>(inferType: true);

            if (article.Actual_Publish_Date == DateTime.MinValue)
            {
                return true;
            }

            return false;
        }
    }
}
