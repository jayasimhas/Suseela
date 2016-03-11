using System.Collections.Generic;
using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;
using Informa.Library.Services.NlmExport.Parser.Legacy.List.Ordered;
using Informa.Library.Services.NlmExport.Parser.Legacy.List.Unordered;
using Informa.Library.Services.NlmExport.Parser.Legacy.Paragraph;
using Informa.Library.Services.NlmExport.Parser.Legacy.Simple;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Container
{
    public class SideboxNode : IDocumentNode
    {
        private readonly Dictionary<string, IDocumentNode> _nodes =
            new Dictionary<string, IDocumentNode>()
                {
                    {"h2", new CaptionNode()},
                    {"h3", new CaptionNode()},
                    {"h4", new CaptionNode()},
                    {"ol", new SideboxOrderedList()},
                    {"ul", new SideboxUnorderedList()},
                    {"blockquote", new BlockquoteNode()},
                    {"p", new BlockquoteParagraph()}
                };

        public void Convert(HtmlNode node, XmlWriter writer)
        {
            using (writer.StartElement("boxed-text"))
            {
                foreach (var currentNode in node.ChildNodes)
                {
                    IDocumentNode handler;
                    if (_nodes.TryGetValue(currentNode.Name, out handler))
                    {
                        handler.Convert(currentNode, writer);
                    }
                }
            }
        }
    }
}
