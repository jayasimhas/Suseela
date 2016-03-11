using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Container;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Link
{
    public class LinkNode : IDocumentNode
    {
        public static readonly string LessThanTemporaryIdentifier = "[LESS_THAN]";
        public static readonly string GreaterThanTemporaryIdentifier = "[GREATER_THAN]";

        private static readonly Dictionary<string, AssetLinkBase> AssetLinkMappings =
            new Dictionary<string, AssetLinkBase>()
                {
                    {"A", new CompanionLink()},
                    {"W", new DealLink()},
                    {"D", new DrugLink()},
                    {"C", new CompanyLink()}
                };

        public static string FromId(string linkId)
        {
            string fragment = string.Empty;
            int indexOfPoundSign = linkId.IndexOf('#');

            if (indexOfPoundSign != -1)
            {
                fragment = linkId.Substring(0, indexOfPoundSign);
            }

            AssetLinkBase link;
            if (AssetLinkMappings.TryGetValue(fragment, out link))
            {
                using (var ms = new MemoryStream())
                {
                    using (var sw = new StreamWriter(ms))
                    {
                        link.Write(sw, linkId.Substring(indexOfPoundSign + 1));
                        sw.Flush();
                    }

                    var bytes = ms.ToArray();
                    return Encoding.UTF8.GetString(bytes);
                }
            }

            return string.Empty;
        }


        public void Convert(HtmlNode node, XmlWriter writer)
        {
            var classOfNode = node.GetAttributeValue("class", string.Empty);
            var nodeLink = node.GetAttributeValue("href", string.Empty);

            string internalUrl;
            if (Helpers.TryResolveInternalLink(nodeLink, out internalUrl))
            {
                nodeLink = internalUrl;
            }

            AssetLinkBase link = null;
            if (classOfNode.Equals("enlarge", StringComparison.CurrentCultureIgnoreCase))
            {
                var content = new InnerTextNode();
                content.Convert(node, writer);
                return;
            }

            if (SupportingDocumentLink.IsSupportedExtension(nodeLink))
            {
                link = new SupportingDocumentLink();
            }
            else if (!string.IsNullOrEmpty(nodeLink))
            {
                link = new ExternalLink();
            }

            if (link != null)
            {
                using (var ms = new MemoryStream())
                {
                    using (var sw = new StreamWriter(ms))
                    {
                        link.Write(sw, nodeLink, node.InnerText);
                    }

                    var bytes = ms.ToArray();
                    var value = Encoding.UTF8.GetString(bytes);
                    writer.WriteString(value);
                }
            }
            else
            {
                // render the inside of an element.
                var content = new InnerTextNode();
                content.Convert(node, writer);
            }
        }
    }
}
