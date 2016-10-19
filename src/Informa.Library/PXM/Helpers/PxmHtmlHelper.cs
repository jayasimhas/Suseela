using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Jabberwocky.Autofac.Attributes;
using Informa.Models.DCD;
using System.Text.RegularExpressions;
using System;

namespace Informa.Library.PXM.Helpers
{
	public interface IPxmHtmlHelper
	{
		string ProcessIframe(string content);
        string ProcessIframeTag(string content);
		string ProcessQuickFacts(string content);
        string ProcessTableStyles(string content);
        string ProcessPullQuotes(string content);
        string ProcessTokens(string content);

    }

	[AutowireService]
	public class PxmHtmlHelper : IPxmHtmlHelper
	{
		private readonly IDependencies _dependencies;
		private const string ClassAttributeName = "childstyle";
        private const string QuickFactsTextStyle = "quick-facts__text";
		private const string ColumnHeadingStyle = "table_subhead 1";
		private const string SubHeadingStyle = "table_subhead 2";
		private const string SubHeadAltStyle = "table_subhead 3";
		private const string StoryTextAltStyle = "table_body";

		[AutowireService(IsAggregateService = true)]
		public interface IDependencies
		{

		}

		public PxmHtmlHelper(IDependencies dependencies)
		{
			_dependencies = dependencies;
		}

		public string ProcessIframeTag(string content)
		{
			var doc = CreateDocument(content);
			var wrapXPath = @"//div[contains(@class,'iframe-component')]";
            var iframeXPath = @"//div[contains(@class, 'ewf-desktop-iframe')]/iframe";

            var iframe = doc.DocumentNode.SelectSingleNode(iframeXPath);
            var wrapper = doc.DocumentNode.SelectSingleNode(wrapXPath);
			if (iframe == null || wrapper == null)
			    return doc.DocumentNode.OuterHtml;
			
			var src = iframe.Attributes["src"];
			if (src != null)
				wrapper.InnerHtml = src.Value;

            wrapper.Name = "p";
			return doc.DocumentNode.OuterHtml;
		}

		public string ProcessIframe(string content)
		{
			var result = ProcessIframeTag(content);
			var doc = CreateDocument(result);
			
			UpdateNode(doc, @"//p[contains(@class,'iframe-header')]", "exhibit_number");
            doc = CreateDocument(doc.DocumentNode.OuterHtml);
            UpdateNode(doc, @"//p[contains(@class,'iframe-title')]", "exhibit_title");
            doc = CreateDocument(doc.DocumentNode.OuterHtml);
            UpdateNode(doc, @"//p[contains(@class,'iframe-component')]", "exhibit_url");
            doc = CreateDocument(doc.DocumentNode.OuterHtml);
            UpdateNode(doc, @"//p[contains(@class,'iframe-caption')]", "exhibit_caption");
            doc = CreateDocument(doc.DocumentNode.OuterHtml);
            UpdateNode(doc, @"//p[contains(@class,'iframe-source')]", "exhibit_source");
            
			return doc.DocumentNode.OuterHtml;
		}

        public void UpdateNode(HtmlDocument doc, string xPath, string cssClass) {
            
            var node = doc.DocumentNode.SelectSingleNode(xPath);
            if (node == null)
                return;

            var attr = node.Attributes["class"];
            if (attr == null)
                return;

            attr.Value = cssClass;

            var newParent = doc.DocumentNode.SelectSingleNode(@"//pre");
            if(newParent == null) {
                newParent = doc.CreateElement("pre");
                doc.DocumentNode.FirstChild.ChildNodes.Insert(0, newParent);
            }            

            var oldParent = node.ParentNode;
            newParent.AppendChild(node);
            oldParent.RemoveChild(node);
        }

		public string ProcessQuickFacts(string content)
		{
			var result = AddCssClassToQuickFactsText(content);
			var doc = CreateDocument(result);

			var xpath = @"//div[@class='quick-facts']";
			var nodes = doc.DocumentNode.SelectNodes(xpath);
		
			if (nodes != null)
			{
				var aside = doc.CreateElement("pre");
				doc.DocumentNode.FirstChild.ChildNodes.Insert(0, aside);
                string qfBody = "qf_body";
                foreach (var node in nodes) {
                    foreach (var childNode in node.ChildNodes) {
                        if (!childNode.Name.Equals("p"))
                            continue;
                        var cAttr = childNode.Attributes["class"];
                        if (cAttr == null)
                            childNode.Attributes.Add("class", qfBody);                        
                    }
                }
                ModifyHtmlStructure(doc, aside, nodes);
			}
			
			return doc.DocumentNode.OuterHtml;
		}

		internal void ModifyHtmlStructure(HtmlDocument doc, HtmlNode root, IEnumerable<HtmlNode> nodes)
		{		
			foreach (var node in nodes)
			{
				AppendAndDeleteOriginal(doc, root, node);
			}
		}
        
		internal void AppendAndDeleteOriginal(HtmlDocument doc, HtmlNode root, HtmlNode element)
		{
			if (element != null)
			{
				var parent = element.ParentNode;
				root.AppendChild(element);
				parent.RemoveChild(element);
			}
		}

		public string AddCssClassToQuickFactsText(string content)
		{
			var xpath = @"//div[@class='quick-facts']/p[not(@class)]";
			var result = AddCssClassToElements(content, xpath, ClassAttributeName, QuickFactsTextStyle);
			return result;
		}

		public string ProcessTableStyles(string content)
		{
			var result = ProcessColumnHeading(content);
			result = ProcessSubHeading(result);
			result = ProcessSubHeadAlt(result);
			result = ProcessStoryTextAlt(result);
			var xpath = @"//table/tbody/tr/td/p";
			result = MoveTableContent(result, xpath);
            result = ProcessPullQuotes(result);
            return result;
		}

		internal string ProcessColumnHeading(string content)
		{
			var xpath = @"//table/tbody/tr/td[@class='header colored']/p";
			var result = AddCssClassToElements(content, xpath, ClassAttributeName, ColumnHeadingStyle);
            return result;
		}

		internal string ProcessSubHeading(string content)
		{
			var xpath = @"//table/tbody/tr/td[@class='cell']/p[contains(@class,'highlight')]";
			var result = AddCssClassToElements(content, xpath, ClassAttributeName, SubHeadingStyle);
			return result;
		}

		internal string ProcessSubHeadAlt(string content)
		{
			var xpath = @"//table/tbody/tr/td[@class='cell colored']/p[contains(@class,'highlight')]";
			var result = AddCssClassToElements(content, xpath, ClassAttributeName, SubHeadAltStyle);
			return result;
		}

		internal string ProcessStoryTextAlt(string content)
		{
			var xpath = @"//table/tbody/tr/td[@class='cell colored']/p[contains(@class,'small')]";
			var result = AddCssClassToElements(content, xpath, ClassAttributeName, StoryTextAltStyle);
			return result;
		}
        
        internal string AddCssClassToElements(string content, string xpath, string attributeName, string attributeValue)
		{
			var doc = CreateDocument(content);
			var elements = doc.DocumentNode.SelectNodes(xpath);
			if (elements == null)
			{
				return doc.DocumentNode.OuterHtml;
			}
			foreach (HtmlNode element in elements)
			{
				var attribute = element.ParentNode.Attributes[attributeName];
				if (attribute == null)
				{
					element.ParentNode.Attributes.Add(attributeName, attributeValue);
				}
				else
				{
					attribute.Value = attributeValue;
				}
			}
			return doc.DocumentNode.OuterHtml;
		}

		internal string MoveTableContent(string content, string xpath)
		{
			var doc = CreateDocument(content);
			var elements = doc.DocumentNode.SelectNodes(xpath);
			if (elements == null)
			{
				return doc.DocumentNode.OuterHtml;
			}
			foreach (HtmlNode element in elements)
			{
                var v = element.Attributes["class"];
                if (v == null)
                    element.Attributes.Append(doc.CreateAttribute("class", "table_paragraph"));
				var contentText = element.InnerHtml;
                if(element.ParentNode != null)
				    element.ParentNode.InnerHtml += contentText;
			}
			return doc.DocumentNode.OuterHtml;
		}

		internal HtmlDocument CreateDocument(string content)
		{
			var doc = new HtmlDocument();
			doc.LoadHtml(content);
			return doc;
		}

	    public string ProcessPullQuotes(string content)
	    {
	        var result = ProcessPullQuotesStyle(content);
	        result = ProcessPullQuotesHtml(result);
	        return result;
	    }

	    private string ProcessPullQuotesHtml(string content)
	    {
	        var xpath = @"//div[contains(@class, 'sidebar-body')]//blockquote[contains(@class,'article-pullquote')]";
	        var doc = CreateDocument(content);
	        var elements = doc.DocumentNode.SelectNodes(xpath);
	        if (elements == null)
	        {
	            return doc.DocumentNode.OuterHtml;
	        }
	        foreach (HtmlNode element in elements)
	        {
	            var nodeStr = element.OuterHtml.Replace("blockquote", "p");
	            var blockquote = HtmlNode.CreateNode(nodeStr);
	            element.ParentNode.ReplaceChild(blockquote, element);
	        }
	        return doc.DocumentNode.OuterHtml;
	    }

        private string ProcessPullQuotesStyle(string content) {
            var xpath = @"//div[contains(@class, 'sidebar-body')]//blockquote[contains(@class,'article-pullquote')]/p";
            var doc = CreateDocument(content);
            var elements = doc.DocumentNode.SelectNodes(xpath);
            if (elements == null)
                return doc.DocumentNode.OuterHtml;

            string classAttr = "class";
            string SidebarPullQuote = "sidebar_quote";
            foreach (HtmlNode element in elements) {
                var attribute = element.Attributes[classAttr];
                if (attribute == null)
                    element.Attributes.Add(classAttr, SidebarPullQuote);
                else
                    attribute.Value = SidebarPullQuote;
            }
            return doc.DocumentNode.OuterHtml;
        }

        public string ProcessTokens(string content) {
            var result = ReplaceCompanies(content);
            return result;
        }

        public string ReplaceCompanies(string content) {
            //Find all matches with Company token
            Regex regex = new Regex(DCDConstants.CompanyTokenRegex);

            var matchSet = new HashSet<string>();
            var matches = regex.Matches(content);
            foreach (Match match in matches) {
                var replace = match.Groups[1].Value.Split(':')[1];
                content = content.Replace(match.Value, $"<span class=\"indexentry\">{replace}</span>");
            }
            return content;
        }
    }
}