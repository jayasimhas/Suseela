using Microsoft.Office.Interop.Word;

namespace SitecoreTreeWalker.Util.CharacterStyles
{
	class BoldStyle : CharacterStyle
	{
		public BoldStyle(string elementName)
			: base(elementName)
		{
		}

		public override int StyleValueOf(Range range)
		{
			return range.Bold;
		}

		public override int StyleValueOf(Style style)
		{
			return style.Font.Bold;
		}
	}
}
