using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Front.Article.Contrib
{
    [XmlRoot("email")]
    [Serializable]
    public class NlmAuthorEmailModel
    {
        [XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink")]
        public string Href { get; set; }
    }
}
