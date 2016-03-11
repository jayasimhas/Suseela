using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Front.Journal
{
    [XmlRoot("publisher")]
    [Serializable]
    public class NlmPublisherModel
    {
        [XmlElement("publisher-name")]
        public string Name { get; set; }
    }
}
