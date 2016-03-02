using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Office.Interop.Word;

namespace InformaSitecoreWord.Util
{
	
	class SideboxBuilder
	{
		List<string> StyleNames;
		private List<Paragraph> _paragraphs;

		public SideboxBuilder(List<string> styleNames)
		{
			StyleNames = styleNames;
			_paragraphs = new List<Paragraph>();
		}

		public bool Match(string styleName)
		{
			return StyleNames.Contains(styleName);
		}

		public bool AddIfMatch(Paragraph paragraph, string style)
		{
			if (Match(style))
			{
				_paragraphs.Add(paragraph);
				return true;
			}
			return false;
		}

		public void Add(Paragraph element)
		{
			_paragraphs.Add(element);
		}

		public XElement GetSidebox(WordUtils wordUtils)
		{
			var sidebox = new XElement("div");
			// Changed the class from "side-box" to "quick-facts"
			sidebox.SetAttributeValue("class", "quick-facts");

			sidebox.Add(wordUtils.ParagraphsToXml(_paragraphs, null, this).Elements());

			return sidebox;
		}

		public void Clear()
		{
			_paragraphs = new List<Paragraph>();
		}

		public bool IsEmpty()
		{
			return _paragraphs.Count == 0;
		}
	}
}
