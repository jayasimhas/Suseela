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

            var article = indexItem.GlassCast<IArticle>(inferType: true);
            
            var processedText = WordUtil.TruncateArticle(article.Summary, 20);
            processedText = ReplaceArticleTokens(processedText);

            return processedText;
        }

        /// <summary>
        ///     Replace article and deal tokens in the summary
        /// </summary>
        /// <param name="bodyText"></param>
        /// <returns></returns>
        public string ReplaceArticleTokens(string bodyText)
        {
            return bodyText;

            //Informa.Web.Models.TokenTransform().RenderTokenBody(x => x.Body, "../Article/_ArticleSidebar")
        }

        /// <summary>
        ///     Truncate the summary text and add ...
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxWordCount"></param>
        /// <returns></returns>
        public string GetTruncatedSummaryText(string summaryText, int maxWordCount)
        {
            return WordUtil.TruncateArticle(summaryText, maxWordCount);
        }
    }
}