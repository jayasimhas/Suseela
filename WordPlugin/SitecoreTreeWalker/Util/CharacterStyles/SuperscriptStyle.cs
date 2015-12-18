using System;
using Microsoft.Office.Interop.Word;

namespace SitecoreTreeWalker.Util.CharacterStyles
{
	class SuperscriptStyle : CharacterStyle
	{
		public SuperscriptStyle(string elementName)
			: base(elementName)
		{
		}

		public override int StyleValueOf(Range range)
		{
			return range.Font.Superscript;
		}

		public override int StyleValueOf(Style style)
		{
			return style.Font.Superscript;
		}
	}
}
