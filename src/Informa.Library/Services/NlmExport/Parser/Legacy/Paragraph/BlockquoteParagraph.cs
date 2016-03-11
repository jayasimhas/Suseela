using System;
using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Paragraph
{
    public class BlockquoteParagraph : IDocumentNode
    {
        public void Convert(HtmlNode node, XmlWriter writer)
        {
            var classOfParagraph = node.GetAttributeValue("class", string.Empty);

            if (classOfParagraph.Equals("quickfactstitle", StringComparison.InvariantCultureIgnoreCase))
            {
                var titleNode = new QuickFactsTitleNode();
                titleNode.Convert(node, writer);
            }
            else if (classOfParagraph.Equals("quickfactstext", StringComparison.InvariantCultureIgnoreCase))
            {
                var titleNode = new QuickFactsTextNode();
                titleNode.Convert(node, writer);
            }
        }
    }
}
