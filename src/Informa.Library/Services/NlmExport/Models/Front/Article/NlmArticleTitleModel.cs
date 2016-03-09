using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Front.Article
{
    [XmlRoot("title-group")]
    [Serializable]
    public class NlmArticleTitleModel
    {
        [XmlElement("article-title")]
        public string Title { get; set; }

        [XmlElement("subtitle")]
        public string SubTitle { get; set; }
    }
}
