using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Informa.Library.Services.NlmExport.Models;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Services.NlmExport.Serialization
{
    [AutowireService]
    public class NlmSerializer : INlmSerializer
    {
        public void Serialize(NlmArticleModel model, Stream output)
        {
            var xmlSerializer = new XmlSerializer(typeof (NlmArticleModel));

            var headerStream = new MemoryStream();
            using (var writer = new XmlTextWriter(headerStream, Encoding.UTF8))
            {
                writer.WriteStartDocument(false);
                writer.WriteDocType("article", null, "journalpub-cals3.dtd", null);
                writer.Flush();

                headerStream.Seek(0, SeekOrigin.Begin);
                headerStream.CopyTo(output);
            }

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true
            };
            using (var writer = XmlWriter.Create(output, settings))
            {
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("mml", "http://www.w3.org/1998/Math/MathML");
                namespaces.Add("xlink", "http://www.w3.org/1999/xlink");

                xmlSerializer.Serialize(writer, model, namespaces);
                writer.Flush();
            }
        }
    }
}
