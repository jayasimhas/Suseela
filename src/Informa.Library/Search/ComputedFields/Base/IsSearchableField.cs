using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Models;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.Base
{
    public class IsSearchableField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {

            if (!indexItem.Paths.Path.StartsWith("/sitecore/content")) return false;

            if (indexItem.TemplateID != IArticleConstants.TemplateId)
            {
                return false;
            }

            IArticle article = indexItem.GlassCast<IArticle>(inferType: true);

            return article.Include_In_Search;
        }
    }
}
