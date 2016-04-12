using System;
using Informa.Library.Services.NlmExport.Parser.Legacy.Container;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Paragraph
{
    public class ParagraphNode : IDocumentNode
    {
        public void Convert(HtmlAgilityPack.HtmlNode node, System.Xml.XmlWriter writer)
        {
            var classOfParagraph = node.GetAttributeValue("class", null);
            if (string.IsNullOrEmpty(classOfParagraph))
            {
                if (!string.IsNullOrEmpty(node.InnerText.Trim()))
                {
                    using (writer.StartElement("p"))
                    {
                        writer.WriteAttributeString("content-type", "2.2 Story Text");

                        var innerContent = new InnerTextNode();
                        innerContent.Convert(node, writer);
                    }
                }
            }
            else if (classOfParagraph.Equals("article-paragraph", StringComparison.CurrentCultureIgnoreCase))
            {
                using (writer.StartElement("p"))
                {
                    writer.WriteAttributeString("content-type", "2.2 Story Text");

                    var innerContent = new InnerTextNode();
                    innerContent.Convert(node, writer);
                }
            }
            else if (classOfParagraph.Equals("nomargin", StringComparison.CurrentCultureIgnoreCase))
            {
                using (writer.StartElement("p"))
                {
                    writer.WriteAttributeString("content-type", "2.25 Story Text - No Spacing");

                    var innerContent = new InnerTextNode();
                    innerContent.Convert(node, writer);
                }
            }
            else if (classOfParagraph.Equals("exhibitnumber", StringComparison.CurrentCultureIgnoreCase))
            {
                // exhibit numbers are handled by the image/table.
                return;
            }
            else if (classOfParagraph.Equals("exhibittitle", StringComparison.CurrentCultureIgnoreCase))
            {
                // exhibit titles are handled by the image/table.
                return;
            }
            else if (classOfParagraph.Equals("source", StringComparison.CurrentCultureIgnoreCase))
            {
                // sources are handled by the image/table.
                return;
            }
            else if (classOfParagraph.Equals("question", StringComparison.CurrentCultureIgnoreCase))
            {
                var interviewQuestion = new InterviewQuestionNode();
                interviewQuestion.Convert(node, writer);
            }
            else if (classOfParagraph.Equals("answer", StringComparison.CurrentCultureIgnoreCase))
            {
                var interviewAnswer = new InterviewAnswerNode();
                interviewAnswer.Convert(node, writer);
            }
            else if (classOfParagraph.Equals("CompanyName", StringComparison.CurrentCultureIgnoreCase))
            {
                var companyName = new CompanyNameNode();
                companyName.Convert(node, writer);
            }
        }
    }
}
