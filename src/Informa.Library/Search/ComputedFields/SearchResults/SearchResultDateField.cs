using System;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
    public class SearchResultDateField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            //If the item does not have the Actual Published Date field then fallback to when the item was created.
            if (indexItem.TemplateID != IArticleConstants.TemplateId)
            {
                return indexItem.Statistics.Created;
            }

            var article = indexItem.GlassCast<IArticle>(inferType: true);

            //If the Actual Publish Date is not set then use the created date.
            if (article.Actual_Publish_Date == DateTime.MinValue)
            {
                if (article.Created_Date == DateTime.MinValue)
                {
                    return indexItem.Statistics.Created;
                }

                return article.Created_Date;
            }

            return article.Actual_Publish_Date;
        }
    }
}