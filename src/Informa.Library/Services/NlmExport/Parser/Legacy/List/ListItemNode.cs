using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Container;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.List
{
    public class ListItemNode
    {
        public string ContentType { get; private set; }

        public ListItemNode(string contentType)
        {
            ContentType = contentType;
        }

        public void Convert(HtmlNode node, XmlWriter writer)
        {
            Convert(node, writer, NlmGlobals.Output.UnorderedListBulletLabel);
        }

        public void Convert(HtmlNode node, XmlWriter writer, int nodeIndex)
        {
            Convert(node, writer, string.Format("{0}. ", nodeIndex.ToString()));
        }

        private void Convert(HtmlNode node, XmlWriter writer, string labelText)
        {
            using (writer.StartElement(NlmGlobals.Output.ListItemNodeName))
            {
                using (writer.StartElement(NlmGlobals.Output.LabelNodeName))
                {
                    writer.WriteString(labelText);
                }

                using (writer.StartElement(NlmGlobals.Output.ParagraphNodeName))
                {
                    writer.WriteAttributeString(NlmGlobals.Output.ParagraphContentTypeAttributeName, ContentType);
                    
                    var innerContent = new InnerTextNode();
                    innerContent.Convert(node, writer);
                }
            }
        }
    }
}
