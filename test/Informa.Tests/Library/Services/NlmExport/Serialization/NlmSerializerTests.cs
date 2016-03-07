using System.IO;
using System.Text;
using System.Xml.Serialization;
using Informa.Library.Services.NlmExport.Models;
using Informa.Library.Services.NlmExport.Serialization;
using NUnit.Framework;

namespace Informa.Tests.Library.Services.NlmExport.Serialization
{
    [TestFixture]
    public class NlmSerializerTests
    {
        [Test]
        public void Serialize_BasicModel_CanRoundTrip()
        {
            var model = new NlmArticleModel
            {
                ArticleType = "top"
            };
            var serializer = new NlmSerializer();

            var stream = new MemoryStream();
            serializer.Serialize(model, stream);

            stream.Seek(0, SeekOrigin.Begin);
            var text = new StreamReader(stream, Encoding.UTF8).ReadToEnd();

            stream.Seek(0, SeekOrigin.Begin);
            var modelFromText = (NlmArticleModel)new XmlSerializer(typeof (NlmArticleModel)).Deserialize(stream);
            stream = new MemoryStream();
            serializer.Serialize(modelFromText, stream);

            stream.Seek(0, SeekOrigin.Begin);
            var roundTrippedText = new StreamReader(stream, Encoding.UTF8).ReadToEnd();

            Assert.AreEqual(text, roundTrippedText);
        }
    }
}
