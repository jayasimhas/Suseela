using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.PXM.Helpers
{
	public interface IPxmHtmlHelper
	{
		string ProcessIframe(string content);
        string ProcessIframeTag(string content);
		string ProcessQuickFacts(string content);
        string ProcessTableStyles(string content);
        string ProcessPullQuotes(string content);

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
			var xpath = @"//iframe";
			var iframes = doc.DocumentNode.SelectNodes(xpath);
			if (iframes == null)
			{
				return doc.DocumentNode.OuterHtml;
			}

			foreach (HtmlNode iframe in iframes)
			{
				var parent = iframe.ParentNode;
				
				var attr = iframe.Attributes[ClassAttributeName];
				if (attr != null && attr.Value.Contains("mobile"))
				{
					parent.RemoveChild(iframe);
					continue;
				}

				var src = iframe.Attributes["src"];
				if (src != null)
				{
					parent.InnerHtml = $"<p>Iframe Content: {src.Value}</p>";
				}
			}
			return doc.DocumentNode.OuterHtml;
		}

		public string ProcessIframe(string content)
		{
			var result = ProcessIframeTag(content);
			var doc = CreateDocument(result);
			
			var headerPath = @"//p[contains(@class,'iframe-header')]";
			var titlePath = @"//p[contains(@class,'iframe-title')]";
			var iframePath = @"//div[contains(@class,'iframe-component')]";
			var captionPath = @"//p[contains(@class,'iframe-caption')]";
			var sourcePath = @"//p[contains(@class,'iframe-source')]";

			var nodes = GethHtmlNodes(doc, headerPath, titlePath, iframePath, captionPath, sourcePath).ToList();
			if (nodes.Any())
			{
				var aside = doc.CreateElement("pre");
				doc.DocumentNode.FirstChild.ChildNodes.Insert(0, aside);
				ModifyHtmlStructure(doc, aside, nodes);
			}

			return doc.DocumentNode.OuterHtml;
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

		internal IEnumerable<HtmlNode> GethHtmlNodes(HtmlDocument doc, params string[] paths)
		{
			foreach (var path in paths)
			{
				var result = doc.DocumentNode.SelectSingleNode(path);
				if (result != null)
				{
					yield return result;
				}
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

        public string ProcessPullQuotes(string content) {
            var xpath = @"//div[contains(@class, 'sidebar-body')]//*/blockquote[contains(@class,'article-pullquote')]/p";
            var doc = CreateDocument(content);
            var elements = doc.DocumentNode.SelectNodes(xpath);
            if (elements == null)
                return doc.DocumentNode.OuterHtml;

            string classAttr = "class";
            string SidebarPullQuote = "sidebar";
            foreach (HtmlNode element in elements) {
                var attribute = element.Attributes[classAttr];
                if (attribute == null)
                    element.Attributes.Add(classAttr, SidebarPullQuote);
                else
                    attribute.Value = SidebarPullQuote;
            }
            return doc.DocumentNode.OuterHtml;
        }

    }
}