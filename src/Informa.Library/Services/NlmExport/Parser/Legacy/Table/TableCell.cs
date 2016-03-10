using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Table
{
    public class TableCell : IDocumentNode
    {
        public HtmlNode Node { get; set; }

        public string Width { get; set; }

        public int Colspan { get; set; }

        public int Rowspan { get; set; }

        public bool IsHeader { get; set; }

        public bool IsSubHeader { get; set; }

        public int Position { get; set; }

        public void Convert(HtmlNode node, XmlWriter writer)
        {
            using (writer.StartElement(NlmGlobals.Output.TableCellNodeName))
            {
                writer.WriteAttributeString(NlmGlobals.Output.TableCellColSepAttributeName, Colspan.ToString());
                writer.WriteAttributeString(NlmGlobals.Output.TableCellRowSepAttributeName, Rowspan.ToString());

                if (Rowspan > 1)
                {
                    writer.WriteAttributeString("morerows", (Rowspan - 1).ToString());
                }

                if (Colspan > 1)
                {
                    writer.WriteAttributeString("namest", string.Format("col{0}", Position));
                    writer.WriteAttributeString("nameend", string.Format("col{0}", (Position - 1) + Colspan));
                }

                string chartCellClass = NlmGlobals.Output.TableCellDefaultStyleClass;
                if (IsHeader)
                {
                    chartCellClass = NlmGlobals.Output.TableCellHeaderStyleClass;
                }
                else if (IsSubHeader)
                {
                    chartCellClass = NlmGlobals.Output.TableCellSubHeaderStyleClass;
                }

                using (writer.StartElement(NlmGlobals.Output.TableCellStyledContentNodeName))
                {
                    writer.WriteAttributeString(NlmGlobals.Output.TableCellStyledContentStyleAttributeName, chartCellClass);
                    
                    var innerContent = new TableCellContent();
                    innerContent.Convert(Node, writer);
                }
            }
        }
    }
}
