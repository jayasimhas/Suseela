using System.Linq;
using System.Xml;
using HtmlAgilityPack;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Utilities
{
	public interface IPxmXmlHelper
	{
		string FinalizeStyles(string content);
	}

	[AutowireService]
	public class PxmXmlHelper : IPxmXmlHelper
	{
		private readonly IDependencies _dependencies;
		private const string SidebarStyling = "Sidebar styling";
		private const string BlockquoteStyle = "quote";

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
	}
}