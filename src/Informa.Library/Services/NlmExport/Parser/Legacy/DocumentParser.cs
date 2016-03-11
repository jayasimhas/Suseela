using System.IO;
using System.Text;
using System.Xml;
using HtmlAgilityPack;

namespace Informa.Library.Services.NlmExport.Parser.Legacy
{
    public static class DocumentParser
    {
        private static void ResetIds()
        {
            Figure.FigureBase.FigureId = 0;
            Table.TableNode.TableId = 0;
            List.BaseListNode.ListCounter = 0;
        }

        public static void Parse(string input, Stream output, Encoding encoding)
        {
            ResetIds();

            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(input);

            if (htmlDocument.DocumentNode == null || htmlDocument.DocumentNode.ChildNodes.Count == 0)
            {
                return;
            }

            using (var ms = new MemoryStream())
            {
                var settings = new XmlWriterSettings();
                settings.CheckCharacters = false;

                using (var xml = XmlWriter.Create(ms, settings))
                {
                    var content = new Container.Content(true);
                    content.Convert(htmlDocument.DocumentNode.ChildNodes[0], xml);
                }

                var contentToWrite = encoding.GetString(ms.ToArray());
                using (var sw = new StreamWriter(output))
                {
                    sw.Write(contentToWrite);
                    sw.Flush();
                }
            }
        }

        public static void Parse(string input, XmlWriter writer)
        {
            ResetIds();

            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(input);

            if (htmlDocument.DocumentNode == null || htmlDocument.DocumentNode.ChildNodes.Count == 0)
            {
                return;
            }

            var content = new Container.Content(true);
            content.Convert(htmlDocument.DocumentNode.ChildNodes[0], writer);
        }
    }
}
