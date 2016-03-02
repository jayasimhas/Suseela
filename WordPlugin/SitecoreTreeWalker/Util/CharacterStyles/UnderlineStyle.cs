using Microsoft.Office.Interop.Word;

namespace InformaSitecoreWord.Util.CharacterStyles
{
	class UnderlineStyle : CharacterStyle
	{
		public UnderlineStyle(string elementName)
			: base(elementName)
		{
		}

		public override int StyleValueOf(Range range)
		{
			var value = range.Underline;
			var hasHyperlink = range.Hyperlinks.Count > 0;
			if (value == WdUnderline.wdUnderlineNone) return 0;
			if (hasHyperlink && range.Text.Length < 2) return 0;
			return (int)WdConstants.wdUndefined;
		}

		public override int StyleValueOf(Style style)
		{
			if (style.Font.Underline == WdUnderline.wdUnderlineNone) return 0;
			return (int) WdConstants.wdUndefined;
		}
	}
}
