using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace SitecoreTreeWalker.Util
{
	public class ParagraphAlignmentParser
	{
		public static string GetClass(WdParagraphAlignment alignment)
		{
			switch(alignment)
			{
				case WdParagraphAlignment.wdAlignParagraphCenter:
					return "aligncenter";
				case WdParagraphAlignment.wdAlignParagraphLeft:
					return "";
				case WdParagraphAlignment.wdAlignParagraphRight:
					return "alignright";
				default:
					return null;
			}
		}
	}
}
