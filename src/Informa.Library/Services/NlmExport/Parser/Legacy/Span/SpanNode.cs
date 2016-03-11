using System;
using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Container;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Span
{
    public class SpanNode : IDocumentNode
    {
        private const string ItalicClassName = "italic";
        private const string BoldClassName = "bold";
        private const string UnderlineClassName = "underline";

        public void Convert(HtmlNode node, XmlWriter writer)
        {
            var classOfSpan = node.GetAttributeValue("class", string.Empty);
            var styleOfSpan = node.GetAttributeValue("style", string.Empty);

            if (classOfSpan.Equals(ItalicClassName, StringComparison.CurrentCultureIgnoreCase) ||
                styleOfSpan.Equals(ItalicClassName, StringComparison.CurrentCultureIgnoreCase))
            {
                using (writer.StartElement(ItalicClassName))
                {
                    var innerTextNode = new InnerTextNode();
                    innerTextNode.Convert(node, writer);
                }
            }
            else if (classOfSpan.Equals(BoldClassName, StringComparison.CurrentCultureIgnoreCase) ||
                     styleOfSpan.Equals(BoldClassName, StringComparison.CurrentCultureIgnoreCase))
            {
                using (writer.StartElement(BoldClassName))
                {
                    var innerTextNode = new InnerTextNode();
                    innerTextNode.Convert(node, writer);
                }
            }
            else if (classOfSpan.Equals(UnderlineClassName, StringComparison.CurrentCultureIgnoreCase) ||
                     styleOfSpan.Equals(UnderlineClassName, StringComparison.CurrentCultureIgnoreCase))
            {
                using (writer.StartElement(UnderlineClassName))
                {
                    var innerTextNode = new InnerTextNode();
                    innerTextNode.Convert(node, writer);
                }
            }
        }
    }
}
