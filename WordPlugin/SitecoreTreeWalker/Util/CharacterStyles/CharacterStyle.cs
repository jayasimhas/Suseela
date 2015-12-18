using System.Linq;
using System.Xml.Linq;
using Microsoft.Office.Interop.Word;

namespace SitecoreTreeWalker.Util.CharacterStyles
{
	public abstract class CharacterStyle
	{
		public string ElementName { get; private set; }

		protected CharacterStyle(string elementName)
		{
			ElementName = elementName;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="range"></param>
		/// <returns>-1 if total match, 0 if total not match, other for partial match</returns>
		abstract public int StyleValueOf(Range range);

		public abstract int StyleValueOf(Style style);

		public MatchType Match(Range range)
		{
			switch (StyleValueOf(range))
			{
				case (0):
					return MatchType.NoneMatch;
				case (-1):
					return MatchType.TotalMatch;
				//case((int)WdConstants.wdUndefined) :
				default:
					return MatchType.PartialMatch;
			}
		}

		public MatchType Match(Style style)
		{
			switch (StyleValueOf(style))
			{
				case (0):
					return MatchType.NoneMatch;
				case (-1):
					return MatchType.TotalMatch;
				//case((int)WdConstants.wdUndefined) :
				default:
					return MatchType.PartialMatch;
			}
		}

		public XElement CreateElement()
		{
			return new XElement(ElementName);
		}

		public bool IsContainedIn(XElement xelement)
		{
			return xelement.AncestorsAndSelf(ElementName).Count() > 0;
		}

		public XElement GetAncestorWithStyle(XElement xelement)
		{
			return xelement.AncestorsAndSelf(ElementName).FirstOrDefault();
		}

		public override bool Equals(object obj)
		{
			var style = obj as CharacterStyle;
			return style != null && style.ElementName == ElementName;
		}

		public bool Equals(CharacterStyle other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other.ElementName, ElementName);
		}

		public override int GetHashCode()
		{
			return (ElementName != null ? ElementName.GetHashCode() : 0);
		}
	}
	public enum MatchType
	{
		TotalMatch, NoneMatch, PartialMatch
	}
}
