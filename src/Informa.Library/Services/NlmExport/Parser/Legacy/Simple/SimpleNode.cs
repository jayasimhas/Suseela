using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Container;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Simple
{
    public class SimpleNode : IDocumentNode
    {
        public static readonly Dictionary<string, string> NodeMapping =
            new Dictionary<string, string>()
                {
                    {"em", "italic"},
                    {"i", "italic"},
                    {"strong", "bold"},
                    {"b", "bold"},
                    {"u", "underline"},
                    {"strike", "strike"},
                    {"sup", "sup"},
                    {"sub", "sub"}
                };

        public void Convert(HtmlNode node, XmlWriter writer)
        {
            string nodeName;
            string r = @"\[(\w)#(\d*):(.*)\]";
            var matches = Regex.Matches(node.InnerText, r);
            if (matches.Count > 0)
            {
                var innerText = new InnerTextNode();
                innerText.Convert(node, writer);
            }
            else if (NodeMapping.TryGetValue(node.Name, out nodeName))
            {
                using (writer.StartElement(nodeName))
                {
                    var innerText = new InnerTextNode();
                    innerText.Convert(node, writer);
                }
            }
        }
    }
}
