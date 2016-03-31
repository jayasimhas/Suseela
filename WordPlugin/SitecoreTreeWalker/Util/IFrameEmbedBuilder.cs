using System;
using System.Collections.Generic;
using System.Xml.Linq;
using HtmlAgilityPack;
using InformaSitecoreWord.Util.Document;
using PluginModels;
using Microsoft.Office.Interop.Word;
using InformaSitecoreWord.UI.TreeBrowser.TreeBrowserControls;

namespace InformaSitecoreWord.Util
{
	public class IFrameEmbedBuilder
	{

		public const string MobileMessage =
				"You have not provided a mobile view, your IFrame will appear as a link to a new window in mobile. To provide a mobile view replace this text with your code.";

		private static string _newUrlToInsert = String.Empty;
		public static string IFrameClassName = "iframe";
		protected Dictionary<string, WordStyleStruct> ParagraphStyles = new Dictionary<string, WordStyleStruct>();
		public static List<string> IFrameStyles = new List<string> { DocumentAndParagraphStyles.IFrameCodeStyle, DocumentAndParagraphStyles.IFrameMobileCodeStyle, DocumentAndParagraphStyles.IFrameCaptionStyle, DocumentAndParagraphStyles.IFrameTitleStyle, DocumentAndParagraphStyles.IFrameSourceStyle, DocumentAndParagraphStyles.IFrameHeaderStyle };
		protected OptimizedCharacterStyleTransformer Transformer;


		public static XElement Parse(Paragraph paragraph, string cssClass, bool isMobile = false)
		{

			return GetIFrameElement(paragraph.Range.Text, cssClass, isMobile);

		}



		private static XElement GetIFrameElement(string embed, string cssClass, bool isMobile)
		{
			//use html agilty pack to fill in all empty attributes and correct broken html
			string html = string.Format("<html><head></head><body>{0}</body></html>", embed);
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(html);
			HtmlNode embedNodeParent = doc.DocumentNode.SelectSingleNode("//body");

			var embedNode = embedNodeParent.ChildNodes[0];

			if (embedNode != null)
			{
				if (isMobile && embedNode.Attributes["src"] == null)
				{
					var link = new XElement("a");
					link.SetAttributeValue("href", "");
					link.SetAttributeValue("class", "iframe-component__desktop-showcase-link");
					link.SetAttributeValue("onclick", "window.open($(this).data('mediaid')); return false;");
					link.Value = "Open Media";
					return link;
				}

				if (embedNode.Attributes["src"] != null
					&& (embedNode.Attributes["src"].Value.StartsWith("https:") || embedNode.Attributes["src"].Value.StartsWith("//")))
				{
					embedNode.SetAttributeValue("class", String.Format(cssClass));
					var xElement = HtmlNodeToXElement(embedNode);
					if (xElement != null)
					{
						return xElement;
					}
				}
			}
			var removedInsecure = new XElement("div");
			removedInsecure.Add(new XAttribute("data-ewf-note", "iframe-removed-insecure"));
			removedInsecure.Value = string.Empty;
			return removedInsecure;
		}

		private static XElement HtmlNodeToXElement(HtmlNode root)
		{
			if (root != null)
			{
				if (!IsAllowed(root)) return null;
				XElement rootXEl = new XElement(root.Name);
				if (!String.IsNullOrEmpty(root.InnerText.Trim()))
				{
					rootXEl.Value = root.InnerText;
				}
				else
				{
					//add hidden div so that i frame/embed doesn't get dropped
					var hiddendiv = new XElement("div");
					hiddendiv.Add(new XAttribute("style", "display:none;"));
					hiddendiv.Value = "&nbsp;";
					rootXEl.Add(hiddendiv);
				}

				foreach (var attribute in root.Attributes)
				{
					rootXEl.Add(new XAttribute(attribute.Name, attribute.Value));

				}

				foreach (HtmlNode node in root.ChildNodes)
				{
					if (IsAllowed(node))
					{

						XElement child = HtmlNodeToXElement(node);
						rootXEl.Add(child);

					}
				}

				return rootXEl;
			}
			return null;
		}
		private static bool IsAllowed(HtmlNode node)
		{
			return (!node.Name.StartsWith("#") && !String.Equals(node.Name, "script")
					&& !String.Equals(node.Name, "form")
					&& !String.Equals(node.Name, "style") && !String.Equals(node.Name, "link"));
		}
	}
}
