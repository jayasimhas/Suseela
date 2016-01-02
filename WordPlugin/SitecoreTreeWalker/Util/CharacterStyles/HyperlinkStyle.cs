using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace SitecoreTreeWalker.Util.CharacterStyles
{
	class HyperlinkStyle : CharacterStyle
	{
		public HyperlinkStyle(string elementName)
			: base(elementName)
		{
		}

		public override int StyleValueOf(Range range)
		{
			if (range.Hyperlinks.Count < 1) return 0;
			return (int)WdConstants.wdUndefined;
		}

		public override int StyleValueOf(Style style)
		{
			return 0;
		}
	}
}
