using System;
using System.Xml.Serialization;
using Informa.Library.Services.NlmExport.Models.Front.Journal;

namespace Informa.Library.Services.NlmExport.Models.Front
{
    [XmlRoot("journal-meta")]
    [Serializable]
    public class NlmJournalMetaModel
    {
        [XmlElement("journal-id", typeof(NlmJournalIdModel))]
        public NlmJournalIdModel Id { get; set; }

        [XmlElement("journal-title-group", typeof(NlmJournalTitleModel))]
        public NlmJournalTitleModel TitleGroup { get; set; }

        [XmlElement("issn", typeof(NlmIssueNumberModel))]
        public NlmIssueNumberModel IssueNumber { get; set; }

        [XmlElement("publisher", typeof(NlmPublisherModel))]
        public NlmPublisherModel Publisher { get; set; }
    }
}
