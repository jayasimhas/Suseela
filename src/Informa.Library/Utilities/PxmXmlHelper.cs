using System.Linq;
using System.Xml;
using HtmlAgilityPack;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Utilities
{
	public interface IPxmXmlHelper
	{
		string FinalizeStyles(string content);
		string ProcessIframeTag(string content);
		string AddCssClassToQuickFactsText(string content);
	}

	[AutowireService]
	public class PxmXmlHelper : IPxmXmlHelper
	{
		private readonly IDependencies _dependencies;
		private const string SidebarStyling = "Sidebar styling";
		private const string BlockquoteStyle = "Callout";

		[AutowireService(IsAggregateService = true)]
		public interface IDependencies
		{

		}

		public PxmXmlHelper(IDependencies dependencies)
		{
			_dependencies = dependencies;
		}

		public string FinalizeStyles(string content)
		{
			var doc = new XmlDocument();
			doc.LoadXml(content);
			AddSidebarStyles(doc);
			AddBlockquoteStyles(doc);
			return doc.OuterXml.Replace("<TextFrame>", "").Replace("</TextFrame>", "");
		}

		public void AddSidebarStyles(XmlDocument doc)
		{
			var inlines = doc.SelectNodes("//Inline");
			if (inlines != null)
			{
				ApplyStyles(ref doc, inlines, "Inline", "sidebar", SidebarStyling);
			}
		}

		public void AddBlockquoteStyles(XmlDocument doc)
		{
			var inlines = doc.SelectNodes("//Inline");
			if (inlines != null)
			{
				ApplyStyles(ref doc, inlines, "Inline", "blockquote", BlockquoteStyle);
			}
		}

		public void ApplyStyles(ref XmlDocument doc, XmlNodeList xmlNodeList, string parentNodeType, string childNodeType, string style)
		{
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				var children = xmlNode.SelectNodes("//" + parentNodeType + "[@ArticleSource='" + childNodeType + "']/ParagraphStyle");
				if (children == null) continue;

				foreach (XmlNode child in children)
				{
					if (child.Attributes?["Style"] != null)
					{
						child.Attributes["Style"].Value = style;
					}
					else
					{
						var attr = doc.CreateAttribute("Style");
						attr.Value = style;
						child.Attributes?.Append(attr);
					}
				}
			}
		}

		public string ProcessIframeTag(string content)
		{
			var doc = new HtmlDocument();
			doc.LoadHtml(content);
			var xpath = @"//iframe";
			var iframes = doc.DocumentNode.SelectNodes(xpath);
			if (iframes == null)
			{
				return doc.DocumentNode.OuterHtml;
			}

			foreach (HtmlNode iframe in iframes)
			{
				var parent = iframe.ParentNode;
				var attr = iframe.Attributes["class"];
				if (attr != null && attr.Value.Contains("mobile"))
				{
					continue;
				}

				var src = iframe.Attributes["src"];
				if (src != null)
				{
					parent.InnerHtml = $"<p class=\"iframe-content\"><pre type=\"\" height=\"\" width=\"\"><p>Iframe Content: {src.Value}</p></pre></p>";
				}
			}
			return doc.DocumentNode.OuterHtml;
		}

		public string AddCssClassToQuickFactsText(string content)
		{
			var doc = new HtmlDocument();
			doc.LoadHtml(content);
			var xpath = @"//div[@class='quick-facts']/p[not(@class)]";
			var textElements = doc.DocumentNode.SelectNodes(xpath);
			if (textElements == null)
			{
				return doc.DocumentNode.OuterHtml;
			}
			foreach (HtmlNode textElement in textElements)
			{
				textElement.Attributes.Add("class", "quick-facts__text");
			}
			return doc.DocumentNode.OuterHtml;
		}
	}
}