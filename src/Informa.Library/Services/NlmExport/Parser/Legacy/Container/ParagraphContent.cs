using System.Linq;
using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Container
{
    public class ParagraphContent : Content
    {
        /// <summary>
        /// These fields are not rendered directly, but the children of these
        /// nodes are rendered.
        /// </summary>
        private static readonly string[] IgnoredFields =
            new[]
                {
                    "p"
                };

        public override void Convert(HtmlNode node, XmlWriter writer)
        {
            foreach (var currentNode in node.ChildNodes)
            {
                // p tags are not allowed in table cells!
                if (IgnoredFields.Contains(currentNode.Name))
                {
                    // render all the children instead
                    var cellContent = new ParagraphContent();
                    cellContent.Convert(currentNode, writer);
                }
                else
                {
                    IDocumentNode handler;
                    if (Nodes.TryGetValue(currentNode.Name, out handler))
                    {
                        handler.Convert(currentNode, writer);
                    }
                }
            }
        }
    }
}
