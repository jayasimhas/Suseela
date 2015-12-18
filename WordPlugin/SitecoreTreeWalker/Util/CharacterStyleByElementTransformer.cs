using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Office.Interop.Word;
using SitecoreTreeWalker.Sitecore;
using SitecoreTreeWalker.SitecoreTree;
using SitecoreTreeWalker.UI.TreeBrowser.TreeBrowserControls;
using SitecoreTreeWalker.Util.Document;

namespace SitecoreTreeWalker.Util
{
	public class CharacterStyleByElementTransformer
	{
		private readonly Dictionary<string, string> _styles;
		public SupportingDocumentsReferenceBuilder SupDocs = new SupportingDocumentsReferenceBuilder();
		private static CharacterStyleByElementTransformer _transformer;
		public static List<string> BlacklistCharacters = new List<string>
		            {
			       		"\a",
						"\b",
						"\f",
						"\n",
						"\r",
						"\t",
						"\v",
			       	};

		private CharacterStyleByElementTransformer(Dictionary<string, string> styles)
		{
			_styles = styles;
			_styles.TryGetValue("Bold", out _boldElement);
			_styles.TryGetValue("Italic", out _italicElement);
			_styles.TryGetValue("Underline", out _underlineElement);
			_styles.TryGetValue("Strikethrough", out _strikethroughElement);
			_styles.TryGetValue("Superscript", out _superscriptElement);
			_styles.TryGetValue("Subscript", out _subscriptElement);
			InvalidStylesHighlighter.GetParser();
		}

		public static CharacterStyleByElementTransformer GetTransformer()
		{
			if (_transformer == null || string.IsNullOrEmpty(_boldElement))
			{
				List<WordStyleStruct> styles = SitecoreGetter.GetCharacterStyles().ToList();
				var _characterStyles = styles.ToDictionary(style => style.WordStyle, style => style.CssElement);
				_transformer = new CharacterStyleByElementTransformer(_characterStyles); 
			}

			return _transformer;
		}

		public XElement GetCharacterStyledElement(Paragraph paragraph, int limit=-1)
		{
			return GetCharacterStyledElement(paragraph.Range.Characters, (Style)paragraph.get_Style(), null, null, false, limit);
		}

		public XElement GetCharacterStyledElement(Characters characters, Style style = null, String elementType = null, string attr = null, bool ignoreParagraphStyle = false, int limit=-1)
		{
			XElement rootElement = elementType != null ? new XElement(elementType) : new XElement("p");
			if(!string.IsNullOrWhiteSpace(attr))
			{
				rootElement.SetAttributeValue("class", attr);
			}
			XElement currentElement = rootElement;
			List<Range> ranges = characters.Cast<Range>().ToList();
			if(limit > -1 && ranges.Count() > limit)
			{
				ranges = ranges.GetRange(0, limit);
			}
			List<Range>.Enumerator e = ranges.GetEnumerator();

			while(e.MoveNext())
			{
				var cur = e.Current;
				if (cur == null || BlacklistCharacters.Any(cur.Text.Contains))
				{
					continue;
				}
				//too computationally expensive
				//invalidStylesHighlighter.HighlightInvalidCharacter(cur, Globals.SitecoreAddin.Is2010OrAbove());
				Hyperlink parsedHyperlink;
				if(ParseHyperlink(ref e, currentElement, out parsedHyperlink))
				{
					Range currentHyperlinkRange = parsedHyperlink.Range;
					if (currentHyperlinkRange != null)
					{
						while (e.Current != null && (e.Current.End < currentHyperlinkRange.End && e.MoveNext()))
						{ } 
					}
					continue;
				}
				currentElement = PruneAncestors(cur, currentElement, style, ignoreParagraphStyle);
				currentElement = BuildAncestors(cur, currentElement, style, ignoreParagraphStyle);
				currentElement.Add(cur.Text);
			}

			return rootElement;
		}

		public string GetCharacterStylesValue(Paragraph paragraph)
		{
			var element = GetCharacterStyledElement(paragraph.Range.Characters, (Style)paragraph.get_Style());
			var reader = element.CreateReader();
			reader.MoveToContent();
			return reader.ReadInnerXml();
		}

		public XElement GetElementValue(Paragraph paragraph, string name, string clas, bool ignoreParagraphStyle = false)
		{
			var element = GetCharacterStyledElement(paragraph.Range.Characters, (Style)paragraph.get_Style(), name, clas, true);
			element.Name = name;
			
			return element;
		}

		/// <summary>
		/// Parses hyperlink if the current Range in the enumerator e
		/// is part of a hyperlink and moves the enumerator to the next
		/// Range not part of current hyperlink
		/// </summary>
		/// <param name="e"></param>
		/// <param name="currentElement"></param>
		/// <param name="hyperlink"></param>
		private bool ParseHyperlink(ref List<Range>.Enumerator e, XElement currentElement, out Hyperlink hyperlink)
		{
			var cur = e.Current;
			hyperlink = null;
			if (cur == null)
			{
				return false;
			}
			IEnumerable<Hyperlink> hs = cur.Hyperlinks.Cast<Hyperlink>().ToList();
			if (hs.Any())
			{
				Hyperlink currentHyperlink = hs.First();
				Range currentHyperlinkRange = currentHyperlink.Range;
				if (currentHyperlinkRange != null)
				{
					while (e.Current != null && (e.Current.End < currentHyperlinkRange.End && e.MoveNext()))
					{ }
				}
				if (!ShouldNotTransformHyperlink(currentHyperlink))
				{
					XElement toAdd = SupDocs.GetHtmlHyperlink(currentHyperlink);
					currentElement.Add(toAdd ?? GetExternalHyperlink(currentHyperlink));
					hyperlink = currentHyperlink;

					return true;
				}
				
			}
			return false;
		}

		/// <summary>
		/// Hyperlinks that should be transformed are those that author inputs, 
		/// not a related article, not a sidebar, and not a deal.
		/// </summary>
		/// <param name="currentHyperlink"></param>
		/// <returns></returns>
		private static bool ShouldNotTransformHyperlink(Hyperlink currentHyperlink)
		{
			return ArticlesSidebarsControl.IsArticleOrSidebarHyperlink(currentHyperlink)
			       || DealsDrugsCompaniesControl.IsADealHyperlink(currentHyperlink);
		}

		private static XElement GetExternalHyperlink(Hyperlink hyperlink)
		{
			var a = new XElement("a");
			a.SetAttributeValue("href", hyperlink.Address);
			a.Value = hyperlink.TextToDisplay;
			var img = new XElement("img");
			img.SetAttributeValue("width", "10");
			img.SetAttributeValue("height", "14");
			img.SetAttributeValue("class", "docicon");
			img.SetAttributeValue("src", "/images/icon_external.gif");
			a.Add(img);
			return a;
		}

		private XElement PruneAncestors(Range character, XElement current, Style style, bool ignoreParagraphStyle = false)
		{
			XElement parent = current;
			XElement temp = parent.AncestorsAndSelf(_boldElement).FirstOrDefault();
			if (!ShouldBeBold(character, style, ignoreParagraphStyle) && temp != null)
			{
				parent = temp.Parent;
			}
			temp = parent.AncestorsAndSelf(_italicElement).FirstOrDefault();
			if (!ShouldBeItalic(character, style, ignoreParagraphStyle) && temp != null)
			{
				parent = temp.Parent;
			}
			temp = parent.AncestorsAndSelf(_underlineElement).FirstOrDefault();
			if (!ShouldBeUnderline(character, style, ignoreParagraphStyle) && temp != null)
			{
				parent = temp.Parent;
			}
			temp = parent.AncestorsAndSelf(_strikethroughElement).FirstOrDefault();
			if (!ShouldBeStrikethrough(character) && temp != null)
			{
				parent = temp.Parent;
			}
			temp = parent.AncestorsAndSelf(_superscriptElement).FirstOrDefault();
			if (!ShouldBeSuperscript(character) && temp != null)
			{
				parent = temp.Parent;
			}
			temp = parent.AncestorsAndSelf(_subscriptElement).FirstOrDefault();
			if (!ShouldBeSubscript(character) && temp != null)
			{
				parent = temp.Parent;
			}
			return parent;
		}

		private XElement BuildAncestors(Range character, XElement current, Style style, bool ignoreParagraphStyle = false)
		{
			XElement parent = current;
			XElement temp = parent.AncestorsAndSelf(_boldElement).FirstOrDefault();
			if (ShouldBeBold(character, style, ignoreParagraphStyle) && temp == null)
			{
				temp = new XElement(_boldElement);
				parent.Add(temp);
				parent = temp;
			}
			temp = parent.AncestorsAndSelf(_italicElement).FirstOrDefault();
			if (ShouldBeItalic(character, style, ignoreParagraphStyle) && temp == null)
			{
				temp = new XElement(_italicElement);
				parent.Add(temp);
				parent = temp;
			}
			temp = parent.AncestorsAndSelf(_underlineElement).FirstOrDefault();
			if (ShouldBeUnderline(character, style, ignoreParagraphStyle) && temp == null)
			{
				temp = new XElement(_underlineElement);
				parent.Add(temp);
				parent = temp;
			}
			temp = parent.AncestorsAndSelf(_strikethroughElement).FirstOrDefault();
			if (ShouldBeStrikethrough(character) && temp == null)
			{
				temp = new XElement(_strikethroughElement);
				parent.Add(temp);
				parent = temp;
			}
			temp = parent.AncestorsAndSelf(_subscriptElement).FirstOrDefault();
			if (ShouldBeSubscript(character) && temp == null)
			{
				temp = new XElement(_subscriptElement);
				parent.Add(temp);
				parent = temp;
			}
			temp = parent.AncestorsAndSelf(_superscriptElement).FirstOrDefault();
			if (ShouldBeSuperscript(character) && temp == null)
			{
				temp = new XElement(_superscriptElement);
				parent.Add(temp);
				parent = temp;
			}
			return parent;
		}

		#region Character Style Detection

		private static string _boldElement;
		private static string _italicElement;
		private static string _underlineElement;
		private static string _strikethroughElement;
		private static string _superscriptElement;
		private static string _subscriptElement;

		private bool ShouldBeSuperscript(Range character)
		{
			return character.Font.Superscript == -1;
		}

		private bool ShouldBeSubscript(Range character)
		{
			return character.Font.Subscript == -1;
		}

		private bool ShouldBeStrikethrough(Range character)
		{
			return character.Font.StrikeThrough == -1;
		}

		private bool ShouldBeBold(Range character, Style style, bool ignoreParagraphStyle = false)
		{
			Hyperlinks hyperlinks = character.Hyperlinks;
			if (!ignoreParagraphStyle)
			{
				return IsBold(character) && !IsBold(style) && hyperlinks.Count == 0; 
			}
			return IsBold(character) && hyperlinks.Count == 0; 

		}

		private bool ShouldBeItalic(Range character, Style style, bool ignoreParagraphStyle = false)
		{
			Hyperlinks hyperlinks = character.Hyperlinks;
			if (!ignoreParagraphStyle)
			{
				return IsItalic(character) && !IsItalic(style) && hyperlinks.Count == 0; 
			}
			return IsItalic(character) && hyperlinks.Count == 0; 

		}
		private bool ShouldBeUnderline(Range character, Style style, bool ignoreParagraphStyle = false)
		{
			Hyperlinks hyperlinks = character.Hyperlinks;
			if (!ignoreParagraphStyle)
			{
				return IsUnderline(character) && !IsUnderline(style) && hyperlinks.Count == 0; 
			}
			return IsUnderline(character) && hyperlinks.Count == 0; 
		}

		private bool IsBold(Style style)
		{
			if (style == null)
			{
				return false;
			}
			return style.Font.Bold == -1;
		}

		private bool IsItalic(Style style)
		{
			if (style == null)
			{
				return false;
			}
			return style.Font.Italic == -1;
		}

		private bool IsUnderline(Style style)
		{
			if (style == null)
			{
				return false;
			}
			return style.Font.Underline != WdUnderline.wdUnderlineNone;
		}

		private bool IsBold(Range character)
		{
			return character.Font.Bold == -1;
		}
		private bool IsItalic(Range character)
		{
			return character.Font.Italic == -1;
		}
		private bool IsUnderline(Range character)
		{
			return character.Font.Underline != WdUnderline.wdUnderlineNone;
		}
#endregion
	}	
}
