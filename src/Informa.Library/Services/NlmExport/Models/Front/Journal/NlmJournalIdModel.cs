using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Front.Journal
{
    [Serializable]
    public class NlmJournalIdModel
    {
        [XmlAttribute("journal-id-type")]
        public string IdType { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
