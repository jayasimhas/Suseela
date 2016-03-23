using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Glass.Mapper.Sc;
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

            //Replace any article tokens with a placeholder
            var processedTextAndArticleTokens = ArticleTokenProcessingStart(article.Summary);

            //Strip any HTML from the summary
            var processedText = HtmlUtil.StripHtml(processedTextAndArticleTokens.Item1);

            //Truncate the summary text
            processedText = TruncateSummary(processedText);

            //Replace the placholder tokens with the real article tokens
            processedText = ArticleTokenProcessingEnd(processedText, processedTextAndArticleTokens.Item2);

            return processedText;
        }

        private string TruncateSummary(string summary)
        {
            summary = EscapeXMLValue(summary);
            summary = WordUtil.TruncateArticle(summary, 20, false);
            summary = UnescapeXMLValue(summary);
            summary = HtmlUtil.StripHtml(summary);

            return summary;
        }

        /// <summary>
        ///     Replaces any article tokens like (<a>[A#SC049466]</a>) with text (#ArticleToken1 for example) that can be
        ///     replaced at the end of the summary processing process.
        ///     The problem is that the article tokens include HTML and in order for the
        ///     truncating process to work we need to strip any HTML to start.  The new tokens will be replaced with the original
        ///     tokens at the
        ///     end of the summary building process, these article tokens will then be replaced when the summary
        ///     is returned from the search service.
        /// </summary>
        /// <param name="summary"></param>
        /// <returns></returns>
        private Tuple<string, Dictionary<string, string>> ArticleTokenProcessingStart(string summary)
        {
            var tokenMappings = new Dictionary<string, string>();

            var ItemRegex = new Regex(DCDConstants.ArticleTokenRegex, RegexOptions.Compiled);
            var count = 1;
            foreach (Match ItemMatch in ItemRegex.Matches(summary))
            {
                summary = summary.Replace(ItemMatch.Value, "#ArticleToken" + count);
                tokenMappings.Add("#ArticleToken" + count, ItemMatch.Value);
                count++;
            }

            return new Tuple<string, Dictionary<string, string>>(summary, tokenMappings);
        }

        /// <summary>
        ///     Goes with ArticleTokenProcessingStart, this method replaces the #ArticleToken1 style tokens
        ///     with the original article token.
        /// </summary>
        /// <param name="summary"></param>
        /// <param name="tokenMappings"></param>
        /// <returns></returns>
        private string ArticleTokenProcessingEnd(string summary, Dictionary<string, string> tokenMappings)
        {
            foreach (var key in tokenMappings.Keys)
            {
                var value = tokenMappings[key];
                if (value == null)
                {
                    continue;
                }
                summary = summary.Replace(key, value);
            }

            return summary;
        }

        public string UnescapeXMLValue(string xmlString)
        {
            if (xmlString == null)
            {
                throw new ArgumentNullException("xmlString");
            }

            xmlString = xmlString.Replace("&apos;", "'");
            xmlString = xmlString.Replace("&quot;", "\"");
            xmlString = xmlString.Replace("&gt;", ">");
            xmlString = xmlString.Replace("&lt;", "<");
            xmlString = xmlString.Replace("&amp;", "&");

            return xmlString;
        }

        public string EscapeXMLValue(string xmlString)
        {
            if (xmlString == null)
            {
                throw new ArgumentNullException("xmlString");
            }

            xmlString = xmlString.Replace("&", "&amp;");
            xmlString = xmlString.Replace("'", "&apos;");
            xmlString = xmlString.Replace("\"", "&quot;");
            xmlString = xmlString.Replace(">", "&gt;");
            xmlString = xmlString.Replace("<", "&lt;");

            return xmlString;
        }
    }
}