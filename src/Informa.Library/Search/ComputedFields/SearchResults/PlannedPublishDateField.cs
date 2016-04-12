using System;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
    public class PlannedPublishDateField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            if (indexItem.TemplateID != IArticleConstants.TemplateId)
            {
                return DateTime.MinValue;
            }

            var article = indexItem.GlassCast<IArticle>(inferType: true);

            if (article.Planned_Publish_Date == DateTime.MinValue)
            {
                return DateTime.MinValue;
            }

            return article.Planned_Publish_Date;
        }
    }
}