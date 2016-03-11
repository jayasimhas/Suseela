using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Front.Article
{
    [XmlRoot("word-count")]
    [Serializable]
    public class NlmArticleWordCountModel
    {
        [XmlAttribute("count")]
        public string Count { get; set; }
    }
}
