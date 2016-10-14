using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.List
{
    public abstract class BaseListNode : IDocumentNode
    {
        public static int ListCounter = 0;

        public abstract string ListType { get; }

        public abstract string ContentType { get; }

        public abstract ListItemNodeType Type { get; }

        public void Convert(HtmlNode node, XmlWriter writer)
        {
            using (writer.StartElement(NlmGlobals.Output.ListNodeName))
            {
                writer.WriteAttributeString(NlmGlobals.Output.ListTypeAttributeName, ListType);
                writer.WriteAttributeString(NlmGlobals.Output.ListIdAttributeName, NlmGlobals.Output.ListIdAttributeDefaultValue + (ListCounter++));

                var listNodes = node.SelectNodes(NlmGlobals.Input.ListItemHtmlTag);
                var listItemNode = new ListItemNode(ContentType);

                if (listNodes != null)
                {
                    for (int i = 0; i < listNodes.Count; i++)
                    {
                        var listNode = listNodes[i];
                        if (Type == ListItemNodeType.Bullet)
                        {
                            listItemNode.Convert(listNode, writer);
                        }
                        else
                        {
                            listItemNode.Convert(listNode, writer, i + 1);
                        }
                    }
                }
            }
        }
    }
}
