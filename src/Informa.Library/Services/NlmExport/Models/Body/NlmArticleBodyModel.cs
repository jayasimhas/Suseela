using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Body
{
    [XmlRoot("body")]
    [Serializable]
    public class NlmArticleBodyModel : IXmlSerializable
    {
        [XmlIgnore]
        public string SerializedBody { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteRaw(SerializedBody);
        }
    }
}
