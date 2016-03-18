﻿using System;
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
           // processedText = XmlStringUtil.EscapeXMLValue(processedText);
            

         //   processedText = WordUtil.TruncateArticle(processedText, 20,false);
         //   processedText = HtmlUtil.StripHtml(processedText);
            

            return processedText;
        }


       
    }
}