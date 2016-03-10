using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Front.Article
{
    [XmlRoot("pub-date")]
    [Serializable]
    public class NlmArticlePubDateModel
    {
        [XmlAttribute("pub-type")]
        public string DateType { get; set; }

        [XmlElement("day")]
        public string Day { get; set; }

        [XmlElement("month")]
        public string Month { get; set; }

        [XmlElement("year")]
        public string Year { get; set; }
    }
}
