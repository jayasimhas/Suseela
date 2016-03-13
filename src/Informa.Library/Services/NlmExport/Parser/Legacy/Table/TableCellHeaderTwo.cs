using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Container;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Table
{
    public class TableCellHeaderTwo : IDocumentNode
    {
        public void Convert(HtmlNode node, XmlWriter writer)
        {
            using (writer.StartElement(NlmGlobals.Output.TableCellStyledContentNodeName))
            {
                writer.WriteAttributeString(NlmGlobals.Output.TableCellStyledContentStyleAttributeName, "2.2 Story Sub-head");

                var innerContent = new InnerTextNode();
                innerContent.Convert(node, writer);
            }
        }
    }
}
