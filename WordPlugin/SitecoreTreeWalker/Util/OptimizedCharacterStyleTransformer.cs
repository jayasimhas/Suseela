using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using InformaSitecoreWord.UI.TreeBrowser.TreeBrowserControls;
using InformaSitecoreWord.Util.CharacterStyles;
using Microsoft.Office.Interop.Word;
using InformaSitecoreWord.Config;

namespace InformaSitecoreWord.Util
{
	public class OptimizedCharacterStyleTransformer
	{
		public SupportingDocumentsReferenceBuilder SupportingDocumentsReferenceBuilder = new SupportingDocumentsReferenceBuilder();

		public XElement GetCharacterStyledElement(XElement element, Paragraph paragraph,
			List<CharacterStyle> characterStyles, bool ignoreParagraphStyle, int limit = -1)
		{
			if (limit == 0) return element;
			var traverse = element;
			Range paragraphRange = paragraph.Range;
			var paragraphStyle = (Style)paragraph.get_Style();
			var stylesPartialMatch = new List<CharacterStyle>();
			foreach (var style in characterStyles)
			{
				MatchType matchType = style.Match(paragraphRange);
				if (matchType == MatchType.NoneMatch)
				{ //style doesn't exist period
					continue;
				}
				if(!ignoreParagraphStyle && paragraphStyle != null && style.Match(paragraphStyle) == MatchType.TotalMatch)
				{ //style is part of the paragraph style
					continue;
				}
				if (matchType == MatchType.PartialMatch)
				{
					stylesPartialMatch.Add(style);
					continue;
				}
				if (matchType == MatchType.TotalMatch)
				{ //the entire paragraph is styled like that
					stylesPartialMatch.Remove(style);
					var temp = style.CreateElement();
					traverse.Add(temp);
					traverse = temp;
					continue;
				}
			}

			if(paragraphRange.Hyperlinks.Count > 0)
			{
				stylesPartialMatch.Add(CharacterStyleFactory.HyperlinkStyle);
			}
			if(stylesPartialMatch.Count > 0)
			{
				GetCharacterStyledElement(traverse, paragraphRange.Characters.Cast<Range>().ToList(), stylesPartialMatch, limit);
			}
			else
			{
				string content = paragraphRange.Text.RinseMsChars().Trim();
				if (content.Length > limit && limit != -1)
				{
					content = content.Substring(0, limit);
				}
				traverse.Add(content);
			}

			return element;
		}

		public void GetCharacterStyledElement(XElement element, List<Range> characters,
			List<CharacterStyle> characterStyles, int limit = -1)
		{
			if (limit == 0) return;
			int counter = 0;

			var characterEnumerator = characters.GetEnumerator();
			var stylesToPrune = new List<CharacterStyle>();
			var stylesToApply = new List<CharacterStyle>();
			while(characterEnumerator.MoveNext())
			{
				var currentCharacterRange = characterEnumerator.Current;
				if (currentCharacterRange == null)
				{
					continue;
				}
				string content = currentCharacterRange.Text;
				if (StringExtensions.BlacklistCharacters.Contains(content)) continue;

				counter++;
				if (limit != -1 && counter > limit) break;
				FindStylesToPruneAndApply(characterStyles, stylesToPrune, stylesToApply, currentCharacterRange);
				element = PruneStyles(element, stylesToPrune);
				element = ApplyStyles(element, stylesToApply);
				if (!stylesToApply.Any(s => s is HyperlinkStyle))
				{
					element.Add(content);
				}
				else
				{
					ParseHyperlink(ref characterEnumerator, element);
				}
				stylesToApply.Clear();
				stylesToPrune.Clear();
			}
		}

		protected XElement ApplyStyles(XElement xElement, IEnumerable<CharacterStyle> stylesToApply)
		{
			foreach (var style in stylesToApply)
			{
				if (xElement.AncestorsAndSelf(style.ElementName).Count() == 0)
				{
					var temp = style.CreateElement();
					xElement.Add(temp);
					xElement = temp;
				}
			}
			return xElement;
		}

		protected XElement PruneStyles(XElement xElement, IEnumerable<CharacterStyle> stylesToPrune)
		{
			foreach (var style in stylesToPrune)
			{
				var newRoot = xElement.AncestorsAndSelf(style.ElementName).SingleOrDefault();
				if (newRoot != null && newRoot.Parent != null) xElement = newRoot.Parent;
			}
			return xElement;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="characterStyles">List of relevant character styles</param>
		/// <param name="stylesToPrune">List which to add styles to prune</param>
		/// <param name="stylesToApply">List which to add styles to apply</param>
		/// <param name="currentCharacterRange">Character whose style we are interested in</param>
		protected void FindStylesToPruneAndApply(IEnumerable<CharacterStyle> characterStyles, List<CharacterStyle> stylesToPrune,
			List<CharacterStyle> stylesToApply, Range currentCharacterRange)
		{
			foreach (var style in characterStyles)
			{
				var matchType = style.Match(currentCharacterRange);
				switch (matchType)
				{
					case (MatchType.NoneMatch):
						stylesToPrune.Add(style);
						break;
					default:
						stylesToApply.Add(style);
						break;
				}
			}
		}

		/// <summary>
		/// Parses hyperlink if the current Range in the enumerator e
		/// is part of a hyperlink and moves the enumerator to the next
		/// Range not part of current hyperlink
		/// </summary>
		/// <param name="e"></param>
		/// <param name="currentElement"></param>
		/// <returns>True if is hyperlink, else false</returns>
		protected bool ParseHyperlink(ref List<Range>.Enumerator e, XElement currentElement)
		{
			var cur = e.Current;
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

			    if (!WordUtils.IsHyperlinkValid(currentHyperlink))
			    {
			        return false;
			    }

				XElement temp = SupportingDocumentsReferenceBuilder.GetHtmlHyperlink(currentHyperlink);
				if(temp != null)
				{
					currentElement.Add(temp);
					return true;
				}
				if (!ShouldNotTransformHyperlink(currentHyperlink))
				{
					currentElement.Add(GetExternalHyperlink(currentHyperlink));
					return true;
				}
				if (currentHyperlinkRange != null)
				{
					currentElement.Add(currentHyperlinkRange.Text.RinseMsChars());
					return false;
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

		protected static XElement GetExternalHyperlink(Hyperlink hyperlink)
		{
			var a = new XElement("a");
			a.SetAttributeValue("href", hyperlink.Address);
			a.SetAttributeValue("class", "article-link");
            a.Value = hyperlink.Range.Text;
			return a;
		}

		public string GetCharacterStylesValue(Paragraph paragraph)
		{
			var element = new XElement("p");
			element = GetCharacterStyledElement(element, paragraph, CharacterStyleFactory.GetCharacterStyles(), false);
			var reader = element.CreateReader();
			reader.MoveToContent();
			return reader.ReadInnerXml();
		}
	}
}
