using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Office.Interop.Word;
using SitecoreTreeWalker.Config;
using SitecoreTreeWalker.Sitecore;
using SitecoreTreeWalker.Util.CharacterStyles;

namespace SitecoreTreeWalker.Util.Document
{
	public class ArticleDocumentMetadataParser
	{
		public List<string> MetadataStyles = new List<string>();

		protected string TitleStyle = ApplicationConfig.GetPropertyValue("TitleStyle");
		protected string DeckStyle = ApplicationConfig.GetPropertyValue("DeckStyle");
		protected string LongSummaryStyle = ApplicationConfig.GetPropertyValue("LongSummaryStyle");
		protected string ShortSummaryStyle = ApplicationConfig.GetPropertyValue("ShortSummaryStyle");
		protected string SubtitleStyle = ApplicationConfig.GetPropertyValue("SubtitleStyle");
	    protected string BodyStyle = ApplicationConfig.GetPropertyValue("StoryTextStyle");

		public string Title = "";
		public string Deck = "";
		public string ShortSummary = "";
		public string LongSummary = "";
		public string Subtitle = "";

		public int TitleCount;

		private static ArticleDocumentMetadataParser _articleDocumentMetadataParser;

		private ArticleDocumentMetadataParser()
		{
			MetadataStyles.Add(TitleStyle);
			MetadataStyles.Add(DeckStyle);
			MetadataStyles.Add(LongSummaryStyle);
			MetadataStyles.Add(ShortSummaryStyle);
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
			MetadataStyles.Add(LongSummaryStyle);
			MetadataStyles.Add(ShortSummaryStyle);
			MetadataStyles.Add(SubtitleStyle);
			int maxLengthLongSummary = SitecoreClient.GetMaxLengthLongSummary();
			int maxLengthShortSummary = SitecoreClient.GetMaxLengthShortSummary();

			int longSummaryLimit = maxLengthLongSummary;
			int shortSummaryLimit = maxLengthShortSummary;

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
				if (styleName == LongSummaryStyle)
				{
					LongSummary += GetRichText(paragraph, transformer, longSummaryLimit, out longSummaryLimit).Replace("\a", "") + " ";
				} 
				if (styleName == ShortSummaryStyle)
				{
					ShortSummary += GetRichText(paragraph, transformer, shortSummaryLimit, out shortSummaryLimit).Replace("\a", "") + " ";

				}
				if (styleName == SubtitleStyle)
				{
					Subtitle += GetInnerRichText(paragraph, transformer).Replace("\a", "").TrimEnd() + " ";
				}
			}

            if(string.IsNullOrWhiteSpace(LongSummary))
                LongSummary += GetRichText(firstBodyParagraph, transformer, longSummaryLimit, out longSummaryLimit).Replace("\a", "") + " ";                         
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
