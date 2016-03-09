using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Front.Article
{
    [XmlRoot("permissions")]
    [Serializable]
    public class NlmArticlePermissionsModel
    {
        [XmlElement("copyright-statement")]
        public string CopyrightStatement { get; set; }

        [XmlElement("copyright-year")]
        public string CopyrightYear { get; set; }
    }
}
