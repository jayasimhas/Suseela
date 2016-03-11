using System.Xml;
using HtmlAgilityPack;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Interface
{
    public interface IDocumentNode
    {
        void Convert(HtmlNode node, XmlWriter writer);
    }
}
