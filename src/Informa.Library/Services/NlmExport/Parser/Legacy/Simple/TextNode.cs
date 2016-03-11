using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;
using Informa.Library.Services.NlmExport.Parser.Legacy.Link;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Simple
{
    public class TextNode : IDocumentNode
    {
        public void Convert(HtmlNode node, XmlWriter writer)
        {
            var cleanText = node.InnerText;

            //cleanText = CleanText(cleanText);

            if (string.IsNullOrEmpty(cleanText))
            {
                return;
            }

            cleanText = ConvertSidebars(cleanText);
            cleanText = ConvertLinks(cleanText);

            cleanText = HttpUtility.HtmlDecode(cleanText);

            writer.WriteString(cleanText);
        }

        //private static readonly Dictionary<string, string> Replacements =
        //    new Dictionary<string, string>()
        //        {
        //            {"&", "&amp;"}
        //        };

        //private static string CleanText(string text)
        //{
        //    foreach (var replacement in Replacements)
        //    {
        //        text = text.Replace(replacement.Key, replacement.Value);
        //    }
        //    return text;
        //}
        
        private static string ConvertSidebars(string text)
        {
            return Regex.Replace(text, "\\[Sidebar#\\d*\\]", string.Empty);
        }

        private static string ConvertLinks(string text)
        {
            // matches links in pattern [LETTER#NUMBERS]
            var matches = Regex.Matches(text, @"\[(\w#[\w]*\s*?)\]");

            foreach (Match match in matches)
            {
                var id = match.Groups[1].Value.Trim();
                var replacementText = LinkNode.FromId(id);
                text = text.Replace(match.Value, replacementText);
            }

            return text;
        }
    }
}
