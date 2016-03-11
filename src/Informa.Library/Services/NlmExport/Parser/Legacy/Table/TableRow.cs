using System.Collections.Generic;
using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Table
{
    public class TableRow : List<TableCell>, IDocumentNode
    {
        public bool HasRows
        {
            get { return Count != 0; }
        }

        public void Convert(HtmlNode node, XmlWriter writer)
        {
            using (writer.StartElement("row"))
            {
                for (int i = 0; i < Count; i++)
                {
                    var cell = this[i];
                    cell.Position = i + 1;
                    cell.Convert(null, writer);
                }
            }
        }
    }
}
