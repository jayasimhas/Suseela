using System.Text.RegularExpressions;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.StringUtils;
using Informa.Library.Utilities.TokenMatcher;
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

            var processedText = WordUtil.TruncateArticle(article.Summary, 20);
            //processedText = ReplaceArticleTokens(processedText);

            return processedText;
        }

        /// <summary>
        ///     Replace article and deal/company tokens in the summary
        /// </summary>
        /// <param name="bodyText"></param>
        /// <returns></returns>
        public string ReplaceArticleTokens(string bodyText)
        {
            TokenReplacer tokenReplacer = new TokenReplacer();

            //Companies
            string processedText = tokenReplacer.ReplaceCompany(bodyText);

            //Articlees
            processedText = tokenReplacer.ReplaceRelatedArticles(bodyText);

            return processedText;
        }
    }
}