using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Container;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Simple
{
    public class HeaderThreeNode : IDocumentNode
    {
        public void Convert(HtmlNode node, XmlWriter writer)
        {
            using (writer.StartElement("p"))
            {
                writer.WriteAttributeString("content-type", "2.2 Story Sub-head 2");

                var innerContent = new ParagraphContent();
                innerContent.Convert(node, writer);
            }
        }
    }
}
