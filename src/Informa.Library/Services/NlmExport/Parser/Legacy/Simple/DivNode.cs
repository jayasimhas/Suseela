using System;
using Informa.Library.Services.NlmExport.Parser.Legacy.Container;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;
using Informa.Library.Services.NlmExport.Parser.Legacy.Paragraph;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Simple
{
    public class DivNode : IDocumentNode
    {
        public void Convert(HtmlAgilityPack.HtmlNode node, System.Xml.XmlWriter writer)
        {
            var divClass = node.GetAttributeValue("class", null);

            if (!string.IsNullOrEmpty(divClass))
            {
                if (divClass.Equals("sidebox", StringComparison.CurrentCultureIgnoreCase))
                {
                    var sidebox = new SideboxNode();
                    sidebox.Convert(node, writer);
                }
                else if (divClass.Equals("root", StringComparison.CurrentCultureIgnoreCase))
                {
                    var innerContent = new InnerTextNode();
                    innerContent.Convert(node, writer);
                }
                else if (divClass.Equals("article-interview__question", StringComparison.CurrentCultureIgnoreCase))
                {
                    var interviewQuestion = new InterviewQuestionNode();
                    interviewQuestion.Convert(node, writer);
                }
                else if (divClass.Equals("article-interview__answer", StringComparison.CurrentCultureIgnoreCase))
                {
                    var interviewAnswer = new InterviewAnswerNode();
                    interviewAnswer.Convert(node, writer);
                }
                else if (divClass.Equals("quick-facts", StringComparison.CurrentCultureIgnoreCase))
                {
                    var sidebox = new SideboxNode();
                    sidebox.Convert(node, writer);
                }
            }
        }
    }
}
