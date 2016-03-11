using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Front.Article
{
    [XmlRoot("related-article")]
    [Serializable]
    public class NlmRelatedArticleModel
    {
        [XmlAttribute("article-type")]
        public string ArticleType { get; set; }

        [XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink")]
        public string Href { get; set; }
    }
}
