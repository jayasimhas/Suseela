using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Container;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Paragraph
{
    public class InterviewAnswerNode : IDocumentNode
    {
        public void Convert(HtmlNode node, XmlWriter writer)
        {
            using (writer.StartElement("p"))
            {
                writer.WriteAttributeString("content-type", "InterviewAnswer");

                //removes paragraph tags from interview answers because they break NLM validation
                if (string.IsNullOrEmpty(node.InnerHtml) == false)
                    node.InnerHtml = node.InnerHtml.Replace("<p>", string.Empty).Replace("</p>", string.Empty);

                var innerContent = new InnerTextNode();
                innerContent.Convert(node, writer);
            }
        }
    }
}
