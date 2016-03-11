using Informa.Library.Services.NlmExport.Parser.Legacy.Container;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Simple
{
    public class HeaderFourNode : IDocumentNode
    {
        public void Convert(HtmlAgilityPack.HtmlNode node, System.Xml.XmlWriter writer)
        {
            using (writer.StartElement("p"))
            {
                writer.WriteAttributeString("content-type", "2.2 Story Sub-head 3");

                var innerContent = new ParagraphContent();
                innerContent.Convert(node, writer);
            }
        }
    }
}
