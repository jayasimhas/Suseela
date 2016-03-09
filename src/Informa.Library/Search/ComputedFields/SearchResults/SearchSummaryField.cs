using System;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.StringUtils;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
    public class SearchSummaryField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            if (indexItem.TemplateID != IArticleConstants.TemplateId)
            {
                return string.Empty;
            }

            IArticle article = indexItem.GlassCast<IArticle>(inferType: true);

            return GetTruncatedText(article.Summary,20);
        }

        public string GetTruncatedText(string text,int maxWordCount)
        {
            string result;
            try
            {
                int maxWordCountLessEllipsis = maxWordCount;

                if (WordUtil.GetWordCount(text) <= maxWordCount)
                    result = text;

                else
                {
                    result = HtmlUtil.StripHtml(text);
                    var lastWordPosition = result.LastIndexOf(' ');

                    if (lastWordPosition < 0) lastWordPosition = 0;

                    result = result.Substring(0, lastWordPosition).Trim(new[] { '.', ',', '!', '?' }) + "...";

                }
            }
            catch (Exception)
            {
                return text;
            }

            return result;
        }
    }
}
