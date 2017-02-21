using System.Xml.Serialization;

namespace Informa.Library.Salesforce.V2.User.Profile
{
    public class UserInfoCustomAttributes
    {
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
        [XmlElement(ElementName = "middle_name")]
        public string MiddleName { get; set; }
        [XmlElement(ElementName = "title")]
        public string Salutation { get; set; }
    }
}