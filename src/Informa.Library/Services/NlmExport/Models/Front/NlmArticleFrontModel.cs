using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Front
{
    [XmlRoot("front")]
    [Serializable]
    public class NlmArticleFrontModel
    {
        [XmlElement("journal-meta", typeof(NlmJournalMetaModel))]
        public NlmJournalMetaModel JournalMeta { get; set; }

        [XmlElement("article-meta", typeof(NlmArticleMetaModel))]
        public NlmArticleMetaModel ArticleMeta { get; set; }
    }
}
