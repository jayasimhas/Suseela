using System.Xml.Linq;
using Microsoft.Office.Interop.Word;

namespace SitecoreTreeWalker.Util
{
	public class ListBuilder
	{
		public XElement Root { get; private set; }
		private XElement Current;
		private int PreviousIndentLevel;

		private static readonly OptimizedCharacterStyleTransformer _characterStyleTransformer 
			= new OptimizedCharacterStyleTransformer();

		public XElement Parse(Paragraph paragraph)
		{
			if(IsList(paragraph))
			{
				if(Root == null)
				{
					InitializeRoot(paragraph);
				}
				else if (paragraph.Range.ListFormat.ListLevelNumber > PreviousIndentLevel)
				{
					NestList(paragraph);
				}
				else
				{
					UnindentList(paragraph);
				}
				//potentially still parsing, so return null
				PreviousIndentLevel = paragraph.Range.ListFormat.ListLevelNumber;
				return null;
			}
			if(Root != null)
			{
				var temp = Root;
				Root = null;
				//finished parsing, so "flush"
				return temp;
			}
			//not parsing and nothing to "flush"
			return null;
		}

		public XElement Flush()
		{
			var temp = Root;
			Root = null;
			return temp;
		}

		private void UnindentList(Paragraph paragraph)
		{
			int delta = PreviousIndentLevel - paragraph.Range.ListFormat.ListLevelNumber;
			for (int i = 0; i < delta; i++)
			{
				if (Current != null)
				{
					Current = Current.Parent;
				}
			}
			if (Current != null)
			{
				Current.Add(CreateInnerListElement(paragraph));
			}
		}

		private void NestList(Paragraph paragraph)
		{
			var temp = CreateListElement(paragraph);
			Current.Add(temp);
			Current = temp;
		}

		private static XElement CreateInnerListElement(Paragraph paragraph)
		{
			var temp = new XElement("li")
			           	{
							Value = _characterStyleTransformer.GetCharacterStylesValue(paragraph)
			           	};
			return temp;
		}

		private void InitializeRoot(Paragraph paragraph)
		{
			PreviousIndentLevel = paragraph.Range.ListFormat.ListLevelNumber;
			Root = CreateListElement(paragraph);
			Current = Root;
		}

		public static bool IsList(Paragraph paragraph)
		{
			return paragraph.Range.ListParagraphs.Count > 0;
		}

		public static XElement CreateListElement(Paragraph paragraph)
		{
			var element = paragraph.Range.ListFormat.ListType == WdListType.wdListBullet 
			                   	? CreateBulletedListElement() 
			                   	: CreateNumberedListElement();
			element.Add(CreateInnerListElement(paragraph));
			return element;
		}

		public static XElement CreateBulletedListElement()
		{
			return new XElement("ul");
		}
		public static XElement CreateNumberedListElement()
		{
			return new XElement("ol");
		}
	}
}
