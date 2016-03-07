using System;
using System.Xml.Serialization;
using Informa.Library.Services.NlmExport.Models.Body;
using Informa.Library.Services.NlmExport.Models.Front;

namespace Informa.Library.Services.NlmExport.Models
{
    [XmlRoot("article")]
    [Serializable]
    public class NlmArticleModel
    {
        [XmlAttribute("dtd-version")]
        public string DtdVersion  => "3.0";

        [XmlAttribute("article-type")]
        public string ArticleType { get; set; }

        [XmlElement("front", typeof(NlmArticleFrontModel))]
        public NlmArticleFrontModel Front { get; set; }

        [XmlElement("body", typeof(NlmArticleBodyModel))]
        public NlmArticleBodyModel Body { get; set; }
    }
}
