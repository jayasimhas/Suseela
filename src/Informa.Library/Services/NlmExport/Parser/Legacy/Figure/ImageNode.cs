using System.Xml;
using HtmlAgilityPack;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Figure
{
    public class ImageNode : FigureBase
    {
        protected override void ConvertFigure(HtmlNode node, XmlWriter writer)
        {
            using (writer.StartElement("graphic"))
            {
                var graphicLocation = node.GetAttributeValue("src", string.Empty);

                string internalUrl;
                if (Helpers.TryResolveInternalLink(graphicLocation, out internalUrl))
                {
                    graphicLocation = internalUrl;
                }

                writer.WriteAttributeString("xlink", "href", null, graphicLocation);
            }
        }
    }
}
