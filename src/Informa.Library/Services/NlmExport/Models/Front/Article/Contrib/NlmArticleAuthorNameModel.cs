using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Front.Article.Contrib
{
    [XmlRoot("name")]
    [Serializable]
    public class NlmArticleAuthorNameModel
    {
        [XmlElement("surname")]
        public string Surname { get; set; }

        [XmlElement("given-names")]
        public string GivenNames { get; set; }
    }
}
