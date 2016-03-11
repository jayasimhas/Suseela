using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace InformaSitecoreWord.Util.CharacterStyles
{
	class StrikethroughStyle : CharacterStyle
	{
		public StrikethroughStyle(string elementName)
			: base(elementName)
		{
		}

		public override int StyleValueOf(Range range)
		{
			return range.Font.StrikeThrough;
		}

		public override int StyleValueOf(Style style)
		{
			return style.Font.StrikeThrough;
		}
	}
}
