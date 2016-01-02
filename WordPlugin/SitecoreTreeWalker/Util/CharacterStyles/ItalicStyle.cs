using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace SitecoreTreeWalker.Util.CharacterStyles
{
	class ItalicStyle : CharacterStyle
	{
		public ItalicStyle(string elementName)
			: base(elementName)
		{
		}

		public override int StyleValueOf(Range range)
		{
			return range.Italic;
		}

		public override int StyleValueOf(Style style)
		{
			return style.Font.Italic;
		}
	}
}
