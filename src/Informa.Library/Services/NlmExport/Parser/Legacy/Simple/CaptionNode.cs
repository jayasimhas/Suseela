using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Container;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Simple
{
    public class CaptionNode : IDocumentNode
    {
        public void Convert(HtmlNode node, XmlWriter writer)
        {
            using (writer.StartElement("caption"))
            {
                using (writer.StartElement("title"))
                {
                    var innerContent = new ParagraphContent();
                    innerContent.Convert(node, writer);
                }
            }
        }
    }
}
