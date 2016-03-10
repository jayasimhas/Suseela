using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Front.Article
{
    [XmlRoot("custom-meta")]
    [Serializable]
    public class NlmArticleCustomMetaModel
    {
        [XmlElement("meta-name")]
        public string MetaName { get; set; }

        [XmlElement("meta-value")]
        public string MetaValue { get; set; }
    }
}
