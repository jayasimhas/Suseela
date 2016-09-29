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
            return Regex.Replace(text, Informa.Models.DCD.DCDConstants.SidebarTokenRegex, string.Empty);
        }

        private static string ConvertLinks(string text)
        {
            // matches links in pattern [LETTER#NUMBERS]
            string r = @"\[(\w)#(\w*):?(.*?)\]";
            var matches = Regex.Matches(text, r);

            foreach (Match match in matches)
            {
                if (match.Groups.Count < 4)
                    continue;

                var type = match.Groups[1].Value.Trim();
                var id = match.Groups[2].Value.Trim();
                var name = match.Groups[3].Value.Trim();
                var replacementText = LinkNode.FromId(type, id, name);
                text = text.Replace(match.Value, replacementText);
            }

            return text;
        }
    }
}
