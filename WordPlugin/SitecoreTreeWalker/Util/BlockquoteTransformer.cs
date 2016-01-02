using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Office.Interop.Word;
using SitecoreTreeWalker.Util.CharacterStyles;

namespace SitecoreTreeWalker.Util
{
	class BlockquoteTransformer
	{
		public static XElement Generate(List<Paragraph> paragaphs, OptimizedCharacterStyleTransformer transformer)
		{
			var sidebox = new XElement("div");
			sidebox.SetAttributeValue("class", "sidebox");
			
			var blockquote = new XElement("blockquote");
			sidebox.Add(blockquote);
			foreach(Paragraph p in paragaphs)
			{
				var paragraphXml = new XElement("p");
				paragraphXml = transformer.GetCharacterStyledElement(paragraphXml, p, CharacterStyleFactory.GetCharacterStyles(), false);
				blockquote.Add(paragraphXml);
			}
			return sidebox;
		}
	}
}
