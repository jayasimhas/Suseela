using System;
using System.Collections.Generic;
using System.Linq;
using PluginModels;
using Microsoft.Office.Core;
using Microsoft.VisualBasic;
using SitecoreTreeWalker.Sitecore;
using Microsoft.Office.Interop.Word;

namespace SitecoreTreeWalker.Util.Document
{
	public class InvalidStylesHighlighter
	{
		private bool _cannotParse;
		private readonly List<string> _validParagraphStyles = new List<string>();
		private static InvalidStylesHighlighter _parser;
		private InvalidStylesHighlighter()
		{
			var styles = SitecoreGetter.GetParagraphStyles();
			foreach (WordStyleStruct style in styles)
			{
				_validParagraphStyles.Add(style.WordStyle);
			}
			_validParagraphStyles.AddRange(Document.ArticleDocumentMetadataParser.GetInstance().MetadataStyles);
			_validParagraphStyles.AddRange(Tables.TableCellParagraphsTransformer.TableParagraphStyles);
			_validParagraphStyles.AddRange(ImageReferenceBuilder.ImageStyles);
			_validParagraphStyles.AddRange(IFrameEmbedBuilder.IFrameStyles);
			_validParagraphStyles.Add(SidebarArticleParser.SidebarStyle);
			_validParagraphStyles.Add(WordUtils.BlockquoteName);
		}

		public static InvalidStylesHighlighter GetParser()
		{
			return _parser ?? (_parser = new InvalidStylesHighlighter());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="document"></param>
		/// <returns>True if invalid character styles found</returns>
		public bool HighlightInvalidParagraphStyles(Microsoft.Office.Interop.Word.Document document)
		{
			bool invalid = false;
			foreach(Paragraph paragraph in document.Paragraphs)
			{
				Style style = (Style)paragraph.get_Style();
				if(!_validParagraphStyles.Contains(style.NameLocal))
				{
					var text = paragraph.Range.Text;
					text = text.Replace("\a", "");
					text = text.Replace("\r", "");
					if(text == "")
					{
						continue;
					}
					paragraph.Range.Font.Color = WdColor.wdColorRed;
					invalid = true;
				}
			}
			return invalid;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="document"></param>
		/// <param name="includeSpecialStyles">Should be false if word version is older than 2010</param>
		/// <returns>True if invalid character styles found</returns>
		public bool HighlightInvalidCharacterStyles(Microsoft.Office.Interop.Word.Document document, bool includeSpecialStyles = false)
		{
			bool invalid = false;
			foreach(Range cha in document.Content.Characters.Cast<Range>())
			{
				//if _cannotParse raised, assume valid style
				if (_cannotParse) return false;
				if (HighlightInvalidCharacter(cha, includeSpecialStyles))
				{
					invalid = true;
				}
			}
			return invalid;
		}

		public bool HighlightInvalidCharacter(Range character, bool includeSpecialStyles = false)
		{
			if (_cannotParse) return false;
			if (IsInvalidCharacterStyle(character) || (includeSpecialStyles && IsInvalidSpecialCharacterStyle(character)))
			{
				character.Font.Color = WdColor.wdColorRed;
				return true;
			}
			return false;
		}

		public bool IsInvalidCharacterStyle(Range character)
		{
			//if _cannotParse raised, assume valid style
			return (character.Font.Shadow == -1
					|| character.Font.DoubleStrikeThrough == -1
					|| character.Font.Emboss == -1
					|| character.Font.Engrave == -1);
			
			
		}

		public bool IsInvalidSpecialCharacterStyle(Range character)
		{
			bool notGlow, notDim, notRefl, notShad;
			if (_cannotParse) return false;
			Font font = character.Font;
			try
			{
				var glow = font.Glow;
				notGlow = glow.Radius == 0 && glow.Transparency == 0;
			}
			catch (Exception)
			{ //assume style is not present if it cannot be retrieved
				notGlow = true;
				_cannotParse = true;
			}

			try
			{
				var dim = font.ThreeD;
				notDim = dim.Visible == MsoTriState.msoFalse;
			}
			catch (Exception)
			{
				
				notDim = true;
				_cannotParse = true;
			}
			try
			{
				var refl = font.Reflection;
				notRefl = refl.Type == MsoReflectionType.msoReflectionTypeNone;
			}
			catch (Exception)
			{
				notRefl = true;
				_cannotParse = true;
			}
			try
			{
				var shad = font.TextShadow;
				notShad = shad.Visible == MsoTriState.msoFalse;
			}
			catch (Exception)
			{
				notShad = true;
				_cannotParse = true;
			}
			return !notGlow || !notDim || !notRefl || !notShad;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="activeDocument"></param>
		/// <returns>True if invalid style found</returns>
		public bool HighlightAllInvalidStyles(Microsoft.Office.Interop.Word.Document activeDocument)
		{
			return HighlightInvalidParagraphStyles(activeDocument);
		}
	}
}
