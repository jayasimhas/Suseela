using System.Xml.Serialization;

namespace Informa.Library.Salesforce.V2.User.Profile
{
    public class SalesforceUserInfo : ISalesforceUserInfo
    {
        [XmlElement(ElementName = "preferred_username")]
        public string UserName { get; set; }
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "family_name")]
        public string LastName { get; set; }
        [XmlElement(ElementName = "email")]
        public string Email { get; set; }
        [XmlElement(ElementName = "given_name")]
        public string FirstName { get; set; }
        [XmlElement(ElementName = "nickname")]
        public string NickName { get; set; }
    }
}