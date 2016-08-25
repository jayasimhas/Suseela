using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Link;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Figure
{
    public class ImageNode : FigureBase
    {
        protected override void ConvertFigure(HtmlNode node, XmlWriter writer)
        {
            var graphicLocation = node.GetAttributeValue("src", string.Empty);

            string internalUrl;
            if (Helpers.TryResolveInternalLink(graphicLocation, out internalUrl))
            {
                graphicLocation = internalUrl;
            }

            var startTag = string.Format("{2}graphic xlink:href=\"{0}\" /{1}", graphicLocation, LinkNode.GreaterThanTemporaryIdentifier, LinkNode.LessThanTemporaryIdentifier);

            writer.WriteString(startTag);
            //using (writer.StartElement("graphic"))
            //{
            //    var graphicLocation = node.GetAttributeValue("src", string.Empty);

            //    string internalUrl;
            //    if (Helpers.TryResolveInternalLink(graphicLocation, out internalUrl))
            //    {
            //        graphicLocation = internalUrl;
            //    }

            //    writer.WriteAttributeString("xlink", "href", null, graphicLocation);
            //}
        }
    }
}
