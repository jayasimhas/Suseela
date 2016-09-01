using System.Xml;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.PXM.Helpers
{
	public interface IPxmXmlHelper
	{
		string FinalizeStyles(string content);
	}

	[AutowireService]
	public class PxmXmlHelper : IPxmXmlHelper
	{
		private readonly IDependencies _dependencies;
		private const string BlockquoteStyle = "quote";
        private const string SidebarBlockquoteStyle = "sidebar_quote";
		private const string OrderedListsStyle = "body_numbered list";
		private const string UnOrderedListsStyle = "body_bullet";

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
			AddBlockquoteStyles(doc);
			AddOrderedListStyles(doc);
			AddUnOrderedListStyles(doc);
			ApplyTableStyles(doc);
            return doc.OuterXml.Replace("<TextFrame>", "").Replace("</TextFrame>", "");
		}

		public void AddBlockquoteStyles(XmlDocument doc)
		{   
			var inlines = doc.SelectNodes("//Inline");
            if (inlines == null)
                return;
			
            foreach(XmlNode xmlNode in inlines) {
                var children = xmlNode.SelectNodes("//Inline[@ArticleSource='blockquote']/ParagraphStyle");
                if (children == null) continue;

                foreach (XmlNode xn in children) {
                    var styleAttr = xn.Attributes["Style"];
                    if (styleAttr == null) {
                        styleAttr = doc.CreateAttribute("Style");
                        xn.Attributes?.Append(styleAttr);
                    }
                    if (styleAttr != null && (styleAttr.Value.Equals("sidebar") || styleAttr.Value.Equals(SidebarBlockquoteStyle)))
                        styleAttr.Value = SidebarBlockquoteStyle;
                    else
                        styleAttr.Value = BlockquoteStyle;
                }
            }
		}

		public void AddOrderedListStyles(XmlDocument doc)
		{
			var textFrames = doc.SelectNodes("//TextFrame");
			if (textFrames != null)
			{
				ApplyListStyles(ref doc, textFrames, "ParagraphStyle", "ordered_lists", OrderedListsStyle);
			}
		}

		public void AddUnOrderedListStyles(XmlDocument doc)
		{
			var textFrames = doc.SelectNodes("//TextFrame");
			if (textFrames != null)
			{
				ApplyListStyles(ref doc, textFrames, "ParagraphStyle", "unordered_lists", UnOrderedListsStyle);
			}
		}

		public void ApplyStyles(ref XmlDocument doc, XmlNodeList xmlNodeList, string parentNodeType, string childNodeType, string style)
		{
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				var children = xmlNode.SelectNodes("//" + parentNodeType + "[@ArticleSource='" + childNodeType + "']/ParagraphStyle");
				if (children == null) continue;

				AddStyles(doc, children, style);
			}
		}

		public void ApplyListStyles(ref XmlDocument doc, XmlNodeList xmlNodeList, string parentNodeType, string childNodeType, string style)
		{
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				var children = xmlNode.SelectNodes("//" + parentNodeType + "[@ArticleSource='" + childNodeType + "']");
				if (children == null) continue;

				AddStyles(doc, children, style);
			}
		}

		public void AddStyles(XmlDocument doc, XmlNodeList children, string style)
		{
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

		public void ApplyTableStyles(XmlDocument doc)
		{
			var tds = doc.SelectNodes("//Cell[@ChildStyle != '']");
			if (tds != null)
			{
				foreach (XmlNode td in tds)
				{
					foreach (XmlNode childNode in td.ChildNodes)
					{
						if (childNode.Attributes?["Style"] != null)
						{
							childNode.Attributes["Style"].Value = td.Attributes?["ChildStyle"].Value;
						}
					}
				}
			}			
		}
	}
}