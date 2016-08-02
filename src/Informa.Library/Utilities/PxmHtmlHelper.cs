using HtmlAgilityPack;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Utilities
{
	public interface IPxmHtmlHelper
	{
		string ProcessIframeTag(string content);
		string AddCssClassToQuickFactsText(string content);
		string ProcessTableStyles(string content);
	}

	[AutowireService]
	public class PxmHtmlHelper : IPxmHtmlHelper
	{
		private readonly IDependencies _dependencies;
		private const string ClassAttributeName = "class";
        private const string QuickFactsTextStyle = "quick-facts__text";
		private const string ColumnHeadingStyle = "table-column-heading";
		private const string SubHeadingStyle = "table-sub-heading";
		private const string SubHeadAltStyle = "table-subhead-alt";
		private const string StoryTextAltStyle = "table-storytext-alt";

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
					parent.InnerHtml = $"<p class=\"iframe-content\"><pre type=\"\" height=\"\" width=\"\"><p>Iframe Content: {src.Value}</p></pre></p>";
				}
			}
			return doc.DocumentNode.OuterHtml;
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
				var attribute = element.Attributes[attributeName];
				if (attribute == null)
				{
					element.Attributes.Add(attributeName, attributeValue);
				}
				else
				{
					attribute.Value = attributeValue;
				}
			}
			return doc.DocumentNode.OuterHtml;
		}

		internal HtmlDocument CreateDocument(string content)
		{
			var doc = new HtmlDocument();
			doc.LoadHtml(content);
			return doc;
		}
	}
}