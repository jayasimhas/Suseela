using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;

namespace InformaSitecoreWord.Util
{
	class DocumentPropertyEditor
	{
		public void WritePublicationAndDate(Microsoft.Office.Interop.Word.Document doc, string publication, string date)
		{
			var props = (DocumentProperties) doc.BuiltInDocumentProperties;
			DocumentProperty commentProp = props[WdBuiltInProperty.wdPropertyComments];
			commentProp.Value = date;
			DocumentProperty categoryProp = props[WdBuiltInProperty.wdPropertyCategory];
			categoryProp.Value = publication;
		}
	}
}
