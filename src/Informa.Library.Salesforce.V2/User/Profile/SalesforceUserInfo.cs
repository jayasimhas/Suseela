using System.Xml.Serialization;

namespace Informa.Library.Salesforce.V2.User.Profile
{
    public class SalesforceUserInfo
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
        [XmlElement(ElementName = "middle_name")]
        public string MiddleName { get; set; }
        [XmlElement(ElementName = "title")]
        public string NameSuffix { get; set; }
        [XmlElement(ElementName = "salutation")]
        public string Salutation { get; set; }
        [XmlElement(ElementName = "companyname")]
        public string CompanyName { get; set; }
        [XmlElement(ElementName = "contact_jobtitle")]
        public string JobTitle { get; set; }
        [XmlElement(ElementName = "contact_jobfunction")]
        public string JobFunction { get; set; }
        [XmlElement(ElementName = "contact_industry")]
        public string JobIndustry { get; set; }
        [XmlElement(ElementName = "phone")]
        public string Phone { get; set; }
        [XmlElement(ElementName = "mobile")]
        public string Mobile { get; set; }
        [XmlElement(ElementName = "contact_mailingstreet")]
        public string MailingStreet { get; set; }
        [XmlElement(ElementName = "contact_mailingcity")]
        public string MailingCity { get; set; }
        [XmlElement(ElementName = "contact_mailingstate")]
        public string MailingState { get; set; }
        [XmlElement(ElementName = "contact_mailingcountry")]
        public string MailingCountry { get; set; }
        [XmlElement(ElementName = "contact_mailingpostalcode")]
        public string MailingPostalCode { get; set; }
    }
}