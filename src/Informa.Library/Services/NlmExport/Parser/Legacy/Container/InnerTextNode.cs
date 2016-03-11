using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Container
{
    public class InnerTextNode : IDocumentNode
    {
        public void Convert(HtmlNode node, XmlWriter writer)
        {
            var content = new Content();
            content.Convert(node, writer);
        }
    }
}
