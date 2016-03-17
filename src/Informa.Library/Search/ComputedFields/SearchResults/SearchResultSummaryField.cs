using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.StringUtils;
using Informa.Library.Utilities.TokenMatcher;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
    public class SearchResultSummaryField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            string test = base.FieldName;

            if (indexItem.TemplateID != IArticleConstants.TemplateId)
            {
                return string.Empty;
            }

            var article = indexItem.GlassCast<IArticle>(inferType: true);

            //string 
            string processedText = HtmlUtil.StripHtml(article.Summary);
            processedText = EscapeXMLValue(processedText);
            

            processedText = WordUtil.TruncateArticle(processedText, 20,false);
            processedText = HtmlUtil.StripHtml(processedText);
            processedText = UnescapeXMLValue(processedText);
           // processedText = ReplaceArticleTokens(processedText);

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