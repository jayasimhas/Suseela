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
			var blockquote = new XElement("blockquote");
			blockquote.SetAttributeValue("class", "article-pullquote");
			foreach(Paragraph p in paragaphs)
			{
				var paragraphXml = new XElement("p");
				paragraphXml = transformer.GetCharacterStyledElement(paragraphXml, p, CharacterStyleFactory.GetCharacterStyles(), false);
				blockquote.Add(paragraphXml);
			}
			return blockquote;
		}
	}
}
