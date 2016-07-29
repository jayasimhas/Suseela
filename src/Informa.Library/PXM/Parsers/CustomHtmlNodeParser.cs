using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using Glass.Mapper;
using HtmlAgilityPack;
using Sitecore.PrintStudio.PublishingEngine.Helpers;
using Sitecore.PrintStudio.PublishingEngine.Text;
using Sitecore.PrintStudio.PublishingEngine.Text.Parsers.Html;
using Sitecore.Reflection;

namespace Informa.Library.PXM.Parsers
{
	public class CustomHtmlNodeParser
	{
		public virtual void ParseNode(HtmlNode htmlNode, XElement resultElement, ParseContext parseContext, XElement baseFormattingElement)
		{
			XElement element = this.CreateElement(htmlNode, parseContext.ParseDefinitions[htmlNode]);
			resultElement.Add((object)element);
			if (!htmlNode.HasChildNodes)
				return;
			XElement formattingElement = StyleParser.GetFormattingElement(htmlNode, baseFormattingElement, element == null);
			this.ParseChildNodes(htmlNode, element ?? resultElement, parseContext, formattingElement);
			if (element == null)
				return;
			HtmlParseHelper.FixNestedElementsNodes(element);
		}

		protected virtual XElement CreateElement(HtmlNode htmlNode, ParseDefinition definition)
		{
			if (definition == null || !HtmlParseHelper.IsValidTargetTag(definition.XmlTag))
				return (XElement)null;
			XElement element = new XElement((XName)definition.XmlTag);
			this.SetAttributes(element, htmlNode, definition);
			return element;
		}

		protected virtual void SetAttributes(XElement element, HtmlNode htmlNode, ParseDefinition definition)
		{
			List<ParseAttribute> applicableAttributes = definition.FindApplicableAttributes(htmlNode);
			HtmlAttributeCollection attributes = htmlNode.Attributes;
			foreach (ParseAttribute parseAttribute in applicableAttributes)
			{
				try
				{
					if (!string.IsNullOrEmpty(parseAttribute.XmlAttribute))
					{
						string attributeValue = string.Empty;
						if (!string.IsNullOrEmpty(parseAttribute.HtmlAttribute) && attributes != null && attributes.Contains(parseAttribute.HtmlAttribute))
						{
							attributeValue = attributes[parseAttribute.HtmlAttribute].Value;
							if (attributeValue.Equals(parseAttribute.HtmlDefaultValue))
								attributeValue = parseAttribute.XmlDefaultValue;
							else if (parseAttribute.HtmlAttribute.Equals("width", StringComparison.InvariantCultureIgnoreCase) || parseAttribute.HtmlAttribute.Equals("height", StringComparison.InvariantCultureIgnoreCase))
								attributeValue = HtmlParseHelper.ParseDimensionValue(attributeValue, true);
						}
						if (string.IsNullOrEmpty(attributeValue))
							attributeValue = parseAttribute.XmlDefaultValue;
						element.SetAttributeValue((XName)parseAttribute.XmlAttribute, (object)attributeValue);
					}
				}
				catch (Exception ex)
				{
					Logger.Error("ParseAttributes", ex);
				}
			}
		}

		protected virtual void ParseChildNodes(HtmlNode htmlNode, XElement resultElement, ParseContext parseContext, XElement baseFormattingElement)
		{
			foreach (HtmlNode htmlNode1 in (IEnumerable<HtmlNode>)htmlNode.ChildNodes)
				this.ParseChildNode(htmlNode1, resultElement, parseContext, baseFormattingElement);
		}

		protected void ParseChildNode(HtmlNode htmlNode, XElement resultElement, ParseContext parseContext, XElement baseFormattingElement)
		{
			if (parseContext.ParseDefinitions.IsEscapedNode(htmlNode))
				return;
			ParseDefinition definition = parseContext.ParseDefinitions[htmlNode];
			if (definition != null)
			{
				Type typeInfo = ReflectionUtil.GetTypeInfo(definition.HtmlParserType);
				object parser = null;

				if (typeInfo.Name == "CustomHtmlNodeParser")
				{
					parser = CustomHtmlParseHelper.GetParser<CustomHtmlNodeParser>(definition);
					((CustomHtmlNodeParser)parser)?.ParseNode(htmlNode, resultElement, parseContext, baseFormattingElement);
				}
				else
				{
					parser = CustomHtmlParseHelper.GetParser<HtmlNodeParser>(definition);
					((HtmlNodeParser)parser)?.ParseNode(htmlNode, resultElement, parseContext, baseFormattingElement);
				}

				if (parser == null)
					return;
			}
			else if (HtmlParseHelper.IsPlainTextNode(htmlNode))
			{
				XCData xcData = new XCData(HtmlEntityHelper.DeEntitize(htmlNode.InnerHtml));
				if (baseFormattingElement != null)
				{
					XElement xelement = new XElement(baseFormattingElement);
					xelement.Add((object)xcData);
					resultElement.Add((object)xelement);
				}
				else
					resultElement.Add((object)xcData);
			}
			else
			{
				XElement formattingElement = StyleParser.GetFormattingElement(htmlNode, baseFormattingElement, true);
				this.ParseChildNodes(htmlNode, resultElement, parseContext, formattingElement);
			}
		}

		public static CustomHtmlNodeParser GetParser2(ParseDefinition definition)
		{
			if (!string.IsNullOrEmpty(definition.HtmlParserType))
			{
				try
				{
					Type typeInfo = ReflectionUtil.GetTypeInfo(definition.HtmlParserType);
					return typeInfo != (Type)null ? ReflectionUtil.CreateObject(typeInfo) as CustomHtmlNodeParser : (CustomHtmlNodeParser)null;
				}
				catch (Exception ex)
				{
					Logger.Error(ex.Message, (Exception)null);
				}
			}
			return (CustomHtmlNodeParser)null;
		}
	}
}