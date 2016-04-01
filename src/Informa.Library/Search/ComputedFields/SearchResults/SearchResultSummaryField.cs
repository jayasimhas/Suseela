using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Glass.Mapper.Sc;
using Informa.Library.Search.Utilities;
using Informa.Library.Utilities.StringUtils;
using Informa.Models.DCD;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
    public class SearchResultSummaryField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            if (indexItem.TemplateID != IArticleConstants.TemplateId)
            {
                return string.Empty;
            }

            var article = indexItem.GlassCast<IArticle>(inferType: true);

            return SearchSummaryUtil.GetTruncatedSearchSummary(article.Summary);
        }

        
    }
}