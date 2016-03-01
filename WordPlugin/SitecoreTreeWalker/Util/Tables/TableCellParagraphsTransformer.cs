using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using InformaSitecoreWord.Util.CharacterStyles;
using Microsoft.Office.Interop.Word;

namespace InformaSitecoreWord.Util.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public class TableCellParagraphsTransformer
	{
		public List<Paragraph> Paragraphs;
		public OptimizedCharacterStyleTransformer CharacterTransformer;
		private WordUtils _wordUtils;
		public string CellStyle { private set; get; }
		public const string TableHeader = "4.0 Table - Column Header";
		public const string TableSubhead = "4.1 Table - Subhead";
		public const string TableSubheadAlternate = "4.2 Table - Subhead Alternate";
		public const string TableTextAlternate = "4.3 Table - Story Text Alternate";
		public const string TableText = "2.2 Story Text";
		public static List<string> TableParagraphStyles = 
			new List<string>
		        {
		            TableHeader,
					TableSubhead,
					TableSubheadAlternate,
					TableTextAlternate,
					TableText
				};
 		private ListBuilder _listBuilder = new ListBuilder();

		public TableCellParagraphsTransformer(Paragraphs paragraphs)
		{
			Paragraphs = paragraphs.Cast<Paragraph>().ToList();
			_wordUtils = new WordUtils();
			CharacterTransformer = _wordUtils.CharacterStyleTransformer;
		}

		public XElement Parse(TableBuilder tableBuilder = null)
		{
			var paragraphs = new XElement("root");
			bool first = true;
			foreach(var paragraph in Paragraphs)
			{
				int tIndex = tableBuilder == null ? -1 : tableBuilder.GetTableIndexFor(paragraph.Range);

				if (tableBuilder != null && tIndex != -1 && tableBuilder.HasRetrieved(tIndex))
				{
					continue;
				}
				if(first)
				{
					CellStyle = GetCellStyle(((Style) paragraph.get_Style()).NameLocal);
					first = false;
				}
				var temp = ParseParagraph(paragraph);
				if(temp != null)
				{
					paragraphs.Add(temp);
				}
				
			}
			var flush = _listBuilder.Flush();
			if(flush != null)
			{
				paragraphs.Add(flush);
			}
			return paragraphs;
		}

		public string GetCellStyle(string paragraphStyle)
		{
			if (paragraphStyle == TableHeader)
			{
				return "header colored";
			}
			if (paragraphStyle == TableSubhead)
			{
				return "cell";
			}
			if (paragraphStyle == TableSubheadAlternate)
			{
				return "cell colored";
			}
			if (paragraphStyle == TableTextAlternate)
			{
				return "cell colored";
			}
			if (paragraphStyle == TableText)
			{
				return "cell";
			}
			return null;
			
		}

		public XElement ParseParagraph(Paragraph paragraph)
		{
			XElement root;
			if (ListBuilder.IsList(paragraph))
			{
				root = _listBuilder.Parse(paragraph);
				if (root == null) return null;
			}
			else
			{
				//all paragraphs are <p> and styling comes from cell
				root = CreateParagraphElement(paragraph);
			} 
			return root;
		}

		public XElement CreateParagraphElement(Paragraph paragraph)
		{
			var paragraphStyle = ((Style) paragraph.get_Style()).NameLocal;
			WdParagraphAlignment alignment = paragraph.Alignment;
			var alignattr = ParagraphAlignmentParser.GetClass(alignment) ?? "";
			var clas = alignattr;
			if(paragraphStyle == TableHeader)
			{
			}
			else if (paragraphStyle == TableSubhead)
			{
				clas += " highlight";
			}
			else if (paragraphStyle == TableSubheadAlternate)
			{
				clas += " highlight";
			}
			else if (paragraphStyle == TableTextAlternate)
			{
				clas += " small";
			}
			else if (paragraphStyle == TableText)
			{
				clas += " small";
			}
			var root = new XElement("p");
			root.SetAttributeValue("class", clas);
			root = CharacterTransformer.GetCharacterStyledElement(root, paragraph, CharacterStyleFactory.GetCharacterStyles(), false);
			return root;
		}
	}
}
