using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;

namespace InformaSitecoreWord.Util.Document
{
	class DocumentPropertyEditor
	{
		public static void WritePublicationAndDate(Microsoft.Office.Interop.Word.Document doc, string publication, string date)
		{
			var props = (DocumentProperties) doc.BuiltInDocumentProperties;
			DocumentProperty commentProp = props[WdBuiltInProperty.wdPropertyComments];
			commentProp.Value = date + " EST";
			DocumentProperty categoryProp = props[WdBuiltInProperty.wdPropertyCategory];
			categoryProp.Value = publication;
		}

		public static void Clear(Microsoft.Office.Interop.Word.Document doc)
		{
			var props = (DocumentProperties)doc.BuiltInDocumentProperties;
			DocumentProperty commentProp = props[WdBuiltInProperty.wdPropertyComments];
			commentProp.Value = @"[Publication Date]";
			DocumentProperty categoryProp = props[WdBuiltInProperty.wdPropertyCategory];
			categoryProp.Value = @"[Publication Name]";
		}
	}
}
