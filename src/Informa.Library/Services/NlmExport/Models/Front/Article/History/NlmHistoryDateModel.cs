using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Front.Article.History
{
    [XmlRoot("date")]
    [Serializable]
    public class NlmHistoryDateModel
    {
        [XmlAttribute("date-type")]
        public string DateType { get; set; }

        [XmlElement("day")]
        public string Day { get; set; }

        [XmlElement("month")]
        public string Month { get; set; }

        [XmlElement("year")]
        public string Year { get; set; }
    }
}
