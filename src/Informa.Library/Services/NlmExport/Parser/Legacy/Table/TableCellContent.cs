using System.Collections.Generic;
using System.Linq;
using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Table
{
    public class TableCellContent : Container.Content
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

        /// <summary>
        /// These nodes override any base Content nodes.
        /// </summary>
        protected readonly Dictionary<string, IDocumentNode> TableCellNodes =
            new Dictionary<string, IDocumentNode>()
                {
                    {"h2", new TableCellHeaderTwo()}
                };

        public override void Convert(HtmlNode node, XmlWriter writer)
        {
            foreach (var currentNode in node.ChildNodes)
            {
                // p tags are not allowed in table cells!
                if (IgnoredFields.Contains(currentNode.Name))
                {
                    // render all the children instead
                    var cellContent = new TableCellContent();
                    cellContent.Convert(currentNode, writer);
                }
                else
                {
                    IDocumentNode handler;
                    if (TableCellNodes.TryGetValue(currentNode.Name, out handler))
                    {
                        handler.Convert(currentNode, writer);
                        continue;
                    }

                    if (Nodes.TryGetValue(currentNode.Name, out handler))
                    {
                        handler.Convert(currentNode, writer);
                    }
                }
            }
        }
    }
}
