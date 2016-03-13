using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Table
{
    public class TableNode : IDocumentNode
    {
        static internal int TableId = 0;

        public void Convert(HtmlNode node, XmlWriter writer)
        {
            var columnInformation = GetTableRows(node);

            if (columnInformation == null || columnInformation.Count() == 0)
            {
                return;
            }

            var firstRow = columnInformation[0];

            using (writer.StartElement("table-wrap"))
            {
                writer.WriteAttributeString("id", "ta" + TableId);
                ++TableId;
                writer.WriteAttributeString("position", "float");

                // write the table frame
                using (writer.StartElement("table"))
                {
                    writer.WriteAttributeString("frame", "all");

                    using (writer.StartElement("tgroup"))
                    {
                        writer.WriteAttributeString("cols", firstRow.Count().ToString());

                        for (int i = 0; i < firstRow.Count; i++)
                        {
                            var cell = firstRow[i];
                            using (writer.StartElement("colspec"))
                            {
                                string columnNumber = (i + 1).ToString();
                                writer.WriteAttributeString("colnum", columnNumber);
                                writer.WriteAttributeString("colname", string.Format("col{0}", columnNumber));
                                if (!string.IsNullOrEmpty(cell.Width))
                                {
                                    writer.WriteAttributeString("colwidth", cell.Width);
                                }
                            }
                        }

                        using (writer.StartElement("tbody"))
                        {
                            foreach (var tableRow in columnInformation)
                            {
                                tableRow.Convert(null, writer);
                            }
                        }
                    }
                }
            }
        }

        private static List<TableRow> GetTableRows(HtmlNode node)
        {
            var list = new List<TableRow>();

            var body = node.SelectSingleNode("tbody");
            HtmlNodeCollection rowChildren;

            if (body == null)
            {
                rowChildren = node.SelectNodes("tr");
            }
            else
            {
                rowChildren = body.SelectNodes("tr");
            }

            if (rowChildren == null)
            {
                return null;
            }

            foreach (var rowChild in rowChildren)
            {
                var currentRow = new TableRow();
                var rowCells = rowChild.SelectNodes("td");

                foreach (var rowCell in rowCells)
                {
                    var cell = new TableCell();
                    cell.Node = rowCell;
                    cell.Width = rowCell.GetAttributeValue("width", null);
                    cell.Colspan = rowCell.GetAttributeValue("colspan", 1);
                    cell.Rowspan = rowCell.GetAttributeValue("rowspan", 1);

                    string cellClass = rowCell.GetAttributeValue("class", string.Empty);
                    cell.IsHeader = cellClass.IndexOf("header", StringComparison.CurrentCultureIgnoreCase) != -1;
                    cell.IsSubHeader = cellClass.IndexOf("highlight", StringComparison.CurrentCultureIgnoreCase) != -1;

                    currentRow.Add(cell);
                }

                list.Add(currentRow);
            }

            return list;
        }
    }
}
