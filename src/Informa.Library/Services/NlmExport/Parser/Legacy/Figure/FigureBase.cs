using System;
using System.Xml;
using HtmlAgilityPack;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Figure
{
    public abstract class FigureBase : IDocumentNode
    {
        internal static int FigureId;

        protected abstract void ConvertFigure(HtmlNode node, XmlWriter writer);

        private const int MaximumNodeRange = 3;
        private static FigureInformation GetFigureInformation(HtmlNode node)
        {
            var figureInformation = new FigureInformation();

            var currentNode = node.ParentNode;
            for (int i = 0; i < MaximumNodeRange; i++)
            {
                if (figureInformation.IsFull || currentNode == null)
                {
                    break;
                }

                currentNode = GatherInformation(figureInformation, currentNode, true);
            }

            if (node.ParentNode != null)
            {
                currentNode = node.ParentNode.NextSibling;
                for (int i = 0; i < MaximumNodeRange; i++)
                {
                    if (figureInformation.IsFull || currentNode == null)
                    {
                        break;
                    }

                    currentNode = GatherInformation(figureInformation, currentNode, false);
                }
            }

            return figureInformation;
        }

        private static HtmlNode GatherInformation(FigureInformation figureInformation, HtmlNode currentNode, bool isGoingUp)
        {
            if (currentNode.Name.Equals("p"))
            {
                var classOfNode = currentNode.GetAttributeValue("class", string.Empty);

                if (classOfNode.Equals("exhibitnumber", StringComparison.CurrentCultureIgnoreCase))
                {
                    figureInformation.Number = currentNode.InnerText.Trim();
                }
                else if (classOfNode.Equals("exhibittitle", StringComparison.CurrentCultureIgnoreCase))
                {
                    figureInformation.Title = currentNode.InnerText.Trim();
                }
                else if (classOfNode.Contains("source"))
                {
                    figureInformation.Source = currentNode.InnerText.Trim();
                }
                else if (classOfNode.Contains("caption"))
                {
                    figureInformation.Title = currentNode.InnerText.Trim();
                }
            }
            else if (currentNode.Name.Equals("a"))
            {
                figureInformation.Link = currentNode.GetAttributeValue("href", null);
            }

            return isGoingUp ? currentNode.PreviousSibling : currentNode.NextSibling;
        }

        public void Convert(HtmlNode node, XmlWriter writer)
        {
            var hrefValue = node.GetAttributeValue("src", string.Empty);

            if (hrefValue.Equals("/images/icon_external.gif"))
            {
                return;
            }

            var figureInformation = GetFigureInformation(node);

            using (writer.StartElement("fig"))
            {
                writer.WriteAttributeString("fig-type", "figure");
                writer.WriteAttributeString("position", "float");

                writer.WriteAttributeString("id", "fa" + FigureId);
                ++FigureId;

                bool isTitleEmpty = string.IsNullOrEmpty(figureInformation.Title);
                bool isCaptionEmpty = string.IsNullOrEmpty(figureInformation.Source);
                if (!isTitleEmpty || !isCaptionEmpty)
                {
                    using (writer.StartElement("caption"))
                    {
                        if (!isTitleEmpty)
                        {
                            using (writer.StartElement("title"))
                            {
                                writer.WriteString(figureInformation.Title);
                            }
                        }

                        if (!isCaptionEmpty)
                        {
                            using (writer.StartElement("p"))
                            {
                                writer.WriteString(figureInformation.Source);
                            }
                        }
                    }
                }

                ConvertFigure(node, writer);
            }
        }
    }
}
