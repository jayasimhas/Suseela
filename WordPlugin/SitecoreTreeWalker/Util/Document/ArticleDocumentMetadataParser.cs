﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using InformaSitecoreWord.Config;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.Util.CharacterStyles;
using Microsoft.Office.Interop.Word;

namespace InformaSitecoreWord.Util.Document
{
	public class ArticleDocumentMetadataParser
	{
		public List<string> MetadataStyles = new List<string>();

		protected string TitleStyle = ApplicationConfig.GetPropertyValue("TitleStyle");
		protected string DeckStyle = ApplicationConfig.GetPropertyValue("DeckStyle");
		protected string ExecutiveSummaryStyle = ApplicationConfig.GetPropertyValue("ExecutiveSummaryStyle");
		protected string SubtitleStyle = ApplicationConfig.GetPropertyValue("SubtitleStyle");
	    protected string BodyStyle = ApplicationConfig.GetPropertyValue("StoryTextStyle");

		public string Title = "";
		public string Deck = "";
		public string ExecutiveSummary = "";
		public string Subtitle = "";

		public int TitleCount;

		private static ArticleDocumentMetadataParser _articleDocumentMetadataParser;

		private ArticleDocumentMetadataParser()
		{
			MetadataStyles.Add(TitleStyle);
			MetadataStyles.Add(DeckStyle);
			MetadataStyles.Add(ExecutiveSummaryStyle);
			MetadataStyles.Add(SubtitleStyle); 
		}

		public static ArticleDocumentMetadataParser GetInstance()
		{
			return _articleDocumentMetadataParser ?? (_articleDocumentMetadataParser = new ArticleDocumentMetadataParser());
		}

		public ArticleDocumentMetadataParser(Microsoft.Office.Interop.Word.Document doc, OptimizedCharacterStyleTransformer transformer)
		{
			TitleCount = 0;
			MetadataStyles.Add(TitleStyle);
			MetadataStyles.Add(DeckStyle);
			MetadataStyles.Add(ExecutiveSummaryStyle);
			MetadataStyles.Add(SubtitleStyle);
			int maxLengthLongSummary = SitecoreClient.GetMaxLengthLongSummary();

			int longSummaryLimit = maxLengthLongSummary;

		    Paragraph firstBodyParagraph = null;

			foreach(Paragraph paragraph in doc.Paragraphs)
			{
				Style style = (Style)paragraph.get_Style();
				string styleName = style.NameLocal;
			    if (firstBodyParagraph == null && styleName == BodyStyle)
			    {
			        firstBodyParagraph = paragraph;
			    }
				if (styleName == TitleStyle)
				{
					Title += GetInnerRichText(paragraph, transformer).Replace("\a", "").TrimEnd() + " ";
					TitleCount++;
				}
				if (styleName == DeckStyle)
				{
					Deck += GetInnerRichText(paragraph, transformer).Replace("\a", "");
				}
				if (styleName == ExecutiveSummaryStyle)
				{
					ExecutiveSummary += GetRichText(paragraph, transformer, longSummaryLimit, out longSummaryLimit).Replace("\a", "") + " ";
				} 
				if (styleName == SubtitleStyle)
				{
					Subtitle += GetInnerRichText(paragraph, transformer).Replace("\a", "").TrimEnd() + " ";
				}
			}

            if(string.IsNullOrWhiteSpace(ExecutiveSummary) && firstBodyParagraph != null)
				ExecutiveSummary += GetRichText(firstBodyParagraph, transformer, longSummaryLimit, out longSummaryLimit).Replace("\a", "") + " ";                         
        }

		protected string GetRichText(Paragraph paragraph, OptimizedCharacterStyleTransformer transformer, int limit, out int newLimit)
		{
			if (limit <= 0)
			{
				newLimit = limit;
				return "";
			}
			string text = GetCharacterStyledElement(paragraph, transformer, limit, out newLimit).ToString();
			return text;
		}

		private static XElement GetCharacterStyledElement(Paragraph paragraph, OptimizedCharacterStyleTransformer transformer, int limit, out int newLimit)
		{
			var characterStyledElement = new XElement("p");
			characterStyledElement = transformer.GetCharacterStyledElement(characterStyledElement, 
				paragraph, CharacterStyleFactory.GetCharacterStyles(), false, limit);
			newLimit = limit - characterStyledElement.Value.Length;
			return characterStyledElement;
		}

		protected string GetInnerRichText(Paragraph paragraph, OptimizedCharacterStyleTransformer transformer, int limit = -1)
		{
			var pelement = new XElement("p");
			var reader = transformer.GetCharacterStyledElement(pelement, paragraph, CharacterStyleFactory.GetCharacterStyles(), false, limit).CreateReader();
			reader.MoveToContent();
			return reader.ReadInnerXml();
		}
	}
}
