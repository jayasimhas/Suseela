using System.Collections.Generic;
using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Figure;
using Informa.Library.Services.NlmExport.Parser.Legacy.Iframe;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;
using Informa.Library.Services.NlmExport.Parser.Legacy.Link;
using Informa.Library.Services.NlmExport.Parser.Legacy.List.Ordered;
using Informa.Library.Services.NlmExport.Parser.Legacy.List.Unordered;
using Informa.Library.Services.NlmExport.Parser.Legacy.Paragraph;
using Informa.Library.Services.NlmExport.Parser.Legacy.Simple;
using Informa.Library.Services.NlmExport.Parser.Legacy.Span;
using Informa.Library.Services.NlmExport.Parser.Legacy.Table;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Container
{
    public class Content : IDocumentNode
    {
        public bool IsBody { get; private set; }

        public Content(bool isBody)
        {
            IsBody = isBody;
        }

        public Content() : this(false)
        {
            
        }

        protected readonly Dictionary<string, IDocumentNode> Nodes =
            new Dictionary<string, IDocumentNode>()
                {
                    {"table", new TableNode()},
                    {"p", new ParagraphNode()},
                    {"h2", new HeaderTwoNode()},
                    {"h3", new HeaderThreeNode()},
                    {"h4", new HeaderFourNode()},
                    {"ol", new OrderListNode()},
                    {"ul", new UnorderedListNode()},
                    {"div", new DivNode()},
                    {"span", new SpanNode()},
                    {"#text", new TextNode()},
                    {"em", new SimpleNode()},
                    {"i", new SimpleNode()},
                    {"strong", new SimpleNode()},
                    {"b", new SimpleNode()},
                    {"u", new SimpleNode()},
                    {"strike", new SimpleNode()},
                    {"sup", new SimpleNode()},
                    {"sub", new SimpleNode()},
                    {"img", new ImageNode()},
                    {"a", new LinkNode()},
                    {"iframe", new IframeNode()}
                };

        public virtual void Convert(HtmlNode node, XmlWriter writer)
        {
            foreach (var currentNode in node.ChildNodes)
            {
                // special case, we don't want free form text in the body.
                if (currentNode.Name == "#text")
                {
                    if (IsBody)
                    {
                        // surround text in a paragraph
                        var paragraph = new ParagraphNode();
                        paragraph.Convert(currentNode, writer);
                        continue;
                    }
                }

                IDocumentNode handler;
                if (Nodes.TryGetValue(currentNode.Name, out handler))
                {
                    handler.Convert(currentNode, writer);
                }
                else if (currentNode.ChildNodes.Count != 0)
                {
                    var innerText = new InnerTextNode();
                    innerText.Convert(currentNode, writer);
                }
            }
        }
    }
}
