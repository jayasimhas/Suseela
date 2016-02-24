﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Informa.Web.Areas.Account.Models;
using Microsoft.Office.Interop.Word;
using SitecoreTreeWalker.Sitecore;
using SitecoreTreeWalker.Util.CharacterStyles;
using SitecoreTreeWalker.Util.Document;

namespace SitecoreTreeWalker.Util
{
	public class ImageReferenceBuilder
	{

		protected Dictionary<string, WordPluginModel.WordStyleStruct> ParagraphStyles = new Dictionary<string, WordPluginModel.WordStyleStruct>();
		public static List<string> ImageStyles = new List<string> { DocumentAndParagraphStyles.ExhibitNumberStyle, DocumentAndParagraphStyles.ExhibitTitleStyle, DocumentAndParagraphStyles.ImagePreviewStyle, DocumentAndParagraphStyles.SourceStyle, DocumentAndParagraphStyles.ExhibitCaptionStyle52 };
		protected OptimizedCharacterStyleTransformer Transformer;

		public static Dictionary<string, string> imageFloatDictionary = new Dictionary<string, string>
		{
			{ "left", "article-inline-image--pull-left"},
			{ "right", "article-inline-image--pull-right"},
			{ "none", "article-inline-image"},
		};

		public ImageReferenceBuilder(Dictionary<string, WordPluginModel.WordStyleStruct> styles, OptimizedCharacterStyleTransformer transformer)
		{
			ParagraphStyles = styles;
			Transformer = transformer;
		}

		public XElement Parse(Paragraph paragraph)
		{
			Style style = (Style)paragraph.get_Style();
			if (style.NameLocal == DocumentAndParagraphStyles.ImagePreviewStyle)
			{

				IEnumerable<Hyperlink> hs = paragraph.Range.Hyperlinks.Cast<Hyperlink>().ToArray();
				if (hs.Count() == 0)
				{
					return null;
				}

				try
				{
					var hyperline = hs.First();

					if (!WordUtils.IsHyperlinkValid(hyperline))
					{
						return null;
					}

					var src = hyperline.Address;
					XElement wrapper = GetImageElement(src, hyperline.ScreenTip);
					return wrapper;
				}
				catch (WebException e)
				{
					Globals.SitecoreAddin.LogException("", e);
					Globals.SitecoreAddin.AlertConnectionFailure();
				}
				catch (Exception e)
				{
					Globals.SitecoreAddin.LogException("", e);
					throw;
				}
			}
			if (ImageStyles.Contains(style.NameLocal))
			{
				WordPluginModel.WordStyleStruct w;
				if (!ParagraphStyles.TryGetValue(style.NameLocal, out w)) return null;
				var element = new XElement("p");
				element.SetAttributeValue("class", w.CssClass);
				element = Transformer.GetCharacterStyledElement(element, paragraph, CharacterStyleFactory.GetCharacterStyles(), false);//new XElement(w.CssElement);

				//var value = Transformer.GetCharacterStylesValue(paragraph).Replace("\a", "");
				string value = element.Value;
				if (value.StartsWith("SOURCE: "))
				{
					element.Value = value.Remove(0, 8);
				}
				return element;
			}
			return null;
		}

		private XElement GetImageElement(string src, string floatType)
		{
			var url = src.Replace("%22", string.Empty).Replace("\"", "");
			var wrapper = new XElement("a");

			var floatClass = string.Empty;
			try
			{
				string classValue;
				floatClass = imageFloatDictionary.TryGetValue(floatType.ToLower(), out classValue) ? classValue : string.Empty;
			}
			catch (Exception ex)
			{
			}
			var img = new XElement("img");
			img.SetAttributeValue("src", url);
			img.SetAttributeValue("class", floatClass);
			wrapper.SetAttributeValue("class", "enlarge");
			wrapper.Add(img);
			wrapper.Add(new XElement("br"));
			return wrapper;
		}

		private XElement GetImageElement(string src)
		{
			var url = src.Replace("%22", string.Empty);
			var wrapper = new XElement("a");

			//wrapper.SetAttributeValue("target", "_blank");
			//wrapper.SetAttributeValue("href", url);
			var img = new XElement("img");
			img.SetAttributeValue("src", url);
			wrapper.SetAttributeValue("class", "enlarge");
			wrapper.Add(img);
			wrapper.Add(new XElement("br"));
			return wrapper;
		}
	}
}
