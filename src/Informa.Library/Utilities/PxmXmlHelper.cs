using System.Linq;
using System.Xml;
using HtmlAgilityPack;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Utilities
{
	public interface IPxmXmlHelper
	{
		string AddSidebarStyles(string content);
	}

	[AutowireService]
	public class PxmXmlHelper : IPxmXmlHelper
	{
		private readonly IDependencies _dependencies;
		private const string SidebarStyling = "Sidebar styling";

		[AutowireService(IsAggregateService = true)]
		public interface IDependencies
		{

		}

		public PxmXmlHelper(IDependencies dependencies)
		{
			_dependencies = dependencies;
		}

		public string AddSidebarStyles(string content)
		{
			var doc = new XmlDocument();
			doc.LoadXml(content);
			var inlines = doc.SelectNodes("//Inline");
			if (inlines != null)
			{
				foreach (XmlNode inline in inlines)
				{
					var children = inline.SelectNodes("//Inline/ParagraphStyle");
					if (children != null)
					{
						foreach (XmlNode child in children)
						{
							if (child.Attributes?["Style"] != null)
							{
								child.Attributes["Style"].Value = SidebarStyling;
							}
							else
							{
								var attr = doc.CreateAttribute("Style");
								attr.Value = SidebarStyling;
								child.Attributes?.Append(attr);
							}
						}
					}
				}
			}
			return doc.OuterXml.Replace("<TextFrame>", "").Replace("</TextFrame>", "");
		}
	}
}