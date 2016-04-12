using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Container;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Paragraph
{
    public class InterviewQuestionNode : IDocumentNode
    {
        public void Convert(HtmlNode node, XmlWriter writer)
        {
            using (writer.StartElement("p"))
            {
                writer.WriteAttributeString("content-type", "InterviewQuestion");
                
                var innerContent = new InnerTextNode();
                innerContent.Convert(node, writer);
            }
        }
    }
}
