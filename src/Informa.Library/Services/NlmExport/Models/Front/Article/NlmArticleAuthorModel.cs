using System;
using System.Xml.Serialization;
using Informa.Library.Services.NlmExport.Models.Front.Article.Contrib;

namespace Informa.Library.Services.NlmExport.Models.Front.Article
{
    [XmlRoot("contrib")]
    [Serializable]
    public class NlmArticleAuthorModel
    {
        [XmlAttribute("contrib-type")]
        public string Type { get; set; }

        [XmlElement("name")]
        public NlmArticleAuthorNameModel Name { get; set; }

        [XmlElement("email")]
        public NlmAuthorEmailModel Email { get; set; }
    }
}
