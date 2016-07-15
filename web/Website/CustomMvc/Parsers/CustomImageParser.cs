using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using HtmlAgilityPack;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.PrintStudio.Configuration;
using Sitecore.PrintStudio.PublishingEngine;
using Sitecore.PrintStudio.PublishingEngine.Rendering;
using Sitecore.PrintStudio.PublishingEngine.Text;
using Sitecore.PrintStudio.PublishingEngine.Text.Parsers.Html;
using Sitecore.Resources.Media;

namespace Informa.Web.CustomMvc.Parsers
{
	public class CustomImageParser : HtmlNodeParser
	{
		public override void ParseNode(HtmlNode htmlNode, XElement resultElement, ParseContext parseContext, XElement baseFormattingElement)
		{
			XElement xelement = (XElement)null;
			HtmlAttribute htmlAttribute = htmlNode.Attributes["src"];
			if (htmlAttribute != null)
			{
				MediaItem mediaItem = (MediaItem)null;
				string mediaPath = this.GetMediaPath(htmlAttribute.Value);
				if (!string.IsNullOrEmpty(mediaPath))
					mediaItem = (MediaItem)(ID.IsID(mediaPath) ? parseContext.Database.GetItem(ID.Parse(mediaPath)) : parseContext.Database.GetItem(mediaPath));
				if (mediaItem != null)
				{
					XElement element = this.CreateImageParagraphElement(htmlNode, (Item)mediaItem, parseContext);
					element.Add(new XCData("Image name:" + mediaItem.Name));
					resultElement.Add((object)element);

					string width;
					string height;
					StyleParser.ParseDimensions(htmlNode, out width, out height);
					if (string.IsNullOrEmpty(width))
						width = HtmlParseHelper.ParseDimensionValue(mediaItem.InnerItem["Width"], true);
					if (string.IsNullOrEmpty(height))
						height = HtmlParseHelper.ParseDimensionValue(mediaItem.InnerItem["Height"], true);
					xelement = this.CreateInlineElement(width, height);
					XElement imageElement = this.CreateImageElement(htmlNode, (Item)mediaItem, parseContext, width, height);
					xelement.Add((object)imageElement);
				}
			}
			if (xelement == null)
				return;
			resultElement.Add((object)xelement);
		}

		protected virtual XElement CreateImageParagraphElement(HtmlNode htmlNode, Item mediaContentItem, ParseContext parseContext)
		{
			XElement xelement = new XElement((XName)"ParagraphStyle");
			xelement.SetAttributeValue((XName)"Style", (object)"5.1 Exhibit Title");
			return xelement;
		}

		protected virtual XElement CreateInlineElement(string width, string height)
		{
			XElement xelement = new XElement((XName)"Inline");
			xelement.SetAttributeValue((XName)"Type", (object)"Graphic");
			xelement.SetAttributeValue((XName)"Height", (object)height);
			xelement.SetAttributeValue((XName)"Width", (object)width);
			xelement.SetAttributeValue((XName)"Position", (object)"AboveLine");
			xelement.SetAttributeValue((XName)"Alignment", (object)"Left");
			return xelement;
		}

		protected virtual XElement CreateImageElement(HtmlNode htmlNode, Item mediaContentItem, ParseContext parseContext, string width, string height)
		{
			XElement xelement = RenderItemHelper.CreateXElement("Image", ((TemplateItem)parseContext.Database.GetItem(WebConfigHandler.PrintStudioEngineSettings.EngineTemplates + "P_Image")).StandardValues, parseContext.PrintOptions.IsClient, (Item)null);
			ParseDefinition definition = parseContext.ParseDefinitions[htmlNode];
			if (definition != null)
				this.SetAttributes(xelement, htmlNode, definition);
			xelement.SetAttributeValue((XName)"Height", (object)height);
			xelement.SetAttributeValue((XName)"Width", (object)width);
			xelement.SetAttributeValue((XName)"SitecoreMediaID", (object)mediaContentItem.ID.Guid.ToString());
			string imageOnServer = ImageRendering.CreateImageOnServer(parseContext.PrintOptions, (MediaItem)mediaContentItem);
			string str = parseContext.PrintOptions.FormatResourceLink(imageOnServer);
			xelement.SetAttributeValue((XName)"LowResSrc", (object)str);
			xelement.SetAttributeValue((XName)"HighResSrc", (object)str);
			return xelement;
		}

		[Obsolete]
		protected MediaItem GetMediaItem(string sourceAttributeValue)
		{
			string mediaPath = this.GetMediaPath(sourceAttributeValue);
			MediaItem mediaItem = (MediaItem)null;
			if (!string.IsNullOrEmpty(mediaPath))
				mediaItem = (MediaItem)(ID.IsID(mediaPath) ? Context.Database.GetItem(ID.Parse(mediaPath)) : Context.Database.GetItem(mediaPath));
			return mediaItem;
		}

		private string GetMediaPath(string localPath)
		{
			int indexA = -1;
			string strB = string.Empty;
			foreach (string str in MediaManager.Provider.Config.MediaPrefixes)
			{
				indexA = localPath.IndexOf(str, StringComparison.InvariantCultureIgnoreCase);
				if (indexA >= 0)
				{
					strB = str;
					break;
				}
			}
			if (indexA < 0 || string.Compare(localPath, indexA, strB, 0, strB.Length, true, CultureInfo.InvariantCulture) != 0)
				return string.Empty;
			string id = StringUtil.Divide(StringUtil.Mid(localPath, indexA + strB.Length), '.', true)[0];
			if (id.EndsWith("/"))
				return string.Empty;
			if (ShortID.IsShortID(id))
				return ShortID.Decode(id);
			char[] chArray = new char[1]
			{
		'/'
			};
			return "/sitecore/media library/" + id.TrimStart(chArray);
		}
	}
}