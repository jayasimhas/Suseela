using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Iframe
{
    public abstract class IframeBase : IDocumentNode
    {
        internal static int IframeId;

        protected abstract void ConvertIframe(HtmlNode node, XmlWriter writer);
        private const int MaximumNodeRangeUpward = 3;
        private const int MaximumNodeRangedownward = 5;

        private static IframeInformation GetIframeInformation(HtmlNode node)
        {
            var iframeInformation = new IframeInformation();
            if (node.HasAttributes)
            {
                var currentNode = node.PreviousSibling;
//extract header and title from the HTML in sitecore
                for (int i = 0; i < MaximumNodeRangeUpward; i++)
                {
                    if (currentNode == null) continue;
                    currentNode = currentNode.PreviousSibling;
                    if (currentNode.HasAttributes)
                    {
                        var classOfNode = currentNode.GetAttributeValue("class", string.Empty);
                        if (classOfNode.Contains("exhibit-number") || classOfNode.Contains("exhibit-title") ||
                            classOfNode.Contains("source") || classOfNode.Contains("caption"))
                        {
                            currentNode = GatherInformation(iframeInformation, currentNode, true);
                        }
                    }
                }

//extract source and caption from the HTML in sitecore
                if (node.NextSibling != null)
                {
                    currentNode = node.NextSibling;
                    for (int i = 0; i < MaximumNodeRangedownward; i++)
                    {
                        if (currentNode != null && currentNode.NextSibling!= null)
                        {
                            currentNode = currentNode.NextSibling;
                            if (currentNode.HasAttributes)
                            {
                                var classOfNode = currentNode.GetAttributeValue("class", string.Empty);
                                if (classOfNode.Contains("exhibit-number") || classOfNode.Contains("exhibit-title") ||
                                    classOfNode.Contains("source") || classOfNode.Contains("caption"))
                                {
                                    currentNode = GatherInformation(iframeInformation, currentNode, false);
                                }
                            }
                        }
                    }
                }
            }

            return iframeInformation;
        }

        private static HtmlNode GatherInformation(IframeInformation iframeInformation, HtmlNode currentNode, bool isGoingUp)
        {
            if (currentNode.Name.Equals("p"))
            {
                var classOfNode = currentNode.GetAttributeValue("class", string.Empty);

                if (classOfNode.Contains("exhibit-number"))
                {
                    iframeInformation.Number = currentNode.InnerText.Trim();
                }
                else if (classOfNode.Contains("exhibit-title"))
                {
                    iframeInformation.Title = currentNode.InnerText.Trim();
                }
                else if (classOfNode.Contains("source"))
                {
                    iframeInformation.Source = currentNode.InnerText.Trim();
                }
                else if (classOfNode.Contains("caption"))
                {
                    iframeInformation.Caption = currentNode.InnerText.Trim();
                }
            }
            return isGoingUp ? currentNode.PreviousSibling : currentNode.NextSibling;
        }

        public void Convert(HtmlNode node, XmlWriter writer)
        {
            var srcValue = node.GetAttributeValue("src", string.Empty);

            var iframeInformation = GetIframeInformation(node);

            using (writer.StartElement("supplementary-material"))
            {
                writer.WriteAttributeString("mimetype", "gen");
                writer.WriteAttributeString("xlink", "href", null, node.GetAttributeValue("src", string.Empty));
                bool isTitleEmpty = string.IsNullOrEmpty(iframeInformation.Title);
                bool isCaptionEmpty = string.IsNullOrEmpty(iframeInformation.Caption);
                bool isHeaderEmpty = string.IsNullOrEmpty(iframeInformation.Number);
                bool isSourceEmpty = string.IsNullOrEmpty(iframeInformation.Source);
                using (writer.StartElement("caption"))
                    {
                        if (!isTitleEmpty)
                        {
                            using (writer.StartElement("title"))
                            {
                                writer.WriteString(iframeInformation.Title);
                            }
                        }
                        if (!isHeaderEmpty)
                        {
                            using (writer.StartElement("p"))
                            {
                                writer.WriteString(iframeInformation.Number);
                            }
                        }
                        if (!isCaptionEmpty)
                        {
                            using (writer.StartElement("p"))
                            {
                                writer.WriteString(iframeInformation.Caption);
                            }
                        }
                        if (!isSourceEmpty)
                        {
                            using (writer.StartElement("p"))
                            {
                                writer.WriteString(iframeInformation.Source);
                            }
                        }

                    }
                // ConvertIframe(node, writer);
            }
        }
    }
}
