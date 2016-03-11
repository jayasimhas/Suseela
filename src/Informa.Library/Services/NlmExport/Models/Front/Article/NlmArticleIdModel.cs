using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Front.Article
{
    //[XmlRoot("article-id")]
    [Serializable]
    public class NlmArticleIdModel
    {
        [XmlAttribute("pub-id-type")]
        public string IdType { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
