using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Front.Journal
{
    [XmlRoot("issn")]
    [Serializable]
    public class NlmIssueNumberModel
    {
        [XmlAttribute("pub-type")]
        public string PublicationType { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
