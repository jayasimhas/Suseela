using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Informa.Library.Salesforce.V2.User.Profile
{
    public class UpdateUserDeatilsRequest
    {
        [XmlElement(ElementName = "preferences")]
        [JsonProperty(PropertyName = "preferences")]
        public List<SalesforceField> Preferences { get; set; }
    }
}
