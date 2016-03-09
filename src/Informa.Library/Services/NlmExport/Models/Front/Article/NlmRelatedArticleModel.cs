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

        [XmlAttribute("href", Namespace = "xlink")]
        public string Href { get; set; }
    }
}
