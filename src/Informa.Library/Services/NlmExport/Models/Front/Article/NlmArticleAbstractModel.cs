using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Front.Article
{
    [XmlRoot("abstract")]
    [Serializable]
    public class NlmArticleAbstractModel
    {
        [XmlAttribute("abstract-type")]
        public string AbstractType { get; set; }

        [XmlElement("p")]
        public string Paragraph { get; set; }
    }
}
