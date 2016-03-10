using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Front.Journal
{
    [XmlRoot("journal-title-group")]
    [Serializable]
    public class NlmJournalTitleModel
    {
        [XmlElement("journal-title")]
        public string Title { get; set; }
    }
}
