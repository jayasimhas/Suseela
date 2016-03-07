using System;
using System.Xml.Serialization;

namespace Informa.Library.Services.NlmExport.Models.Common
{
    [Serializable]
    public class NlmCategoryModel
    {
        [XmlAttribute("subj-group-type")]
        public string GroupType { get; set; }

        [XmlElement("subject")]
        public string Subject { get; set; }
    }
}
