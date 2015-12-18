using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace SitecoreTreeWalker.Util.CharacterStyles
{
	class SubscriptStyle : CharacterStyle
	{
		public SubscriptStyle(string elementName)
			: base(elementName)
		{
		}

		public override int StyleValueOf(Range range)
		{
			return range.Font.Subscript;
		}

		public override int StyleValueOf(Style style)
		{
			return style.Font.Subscript;
		}
	}
}
