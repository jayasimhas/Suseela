using System.Xml.Serialization;

namespace Informa.Library.Salesforce.V2.User.Profile
{
    [XmlRoot("Preference")]
    public class SalesforceField
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string Reference { get; set; }
    }
}
