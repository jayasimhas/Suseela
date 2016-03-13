using Informa.Library.User.Profile;

namespace Informa.Library.Salesforce.User.Profile
{
	public interface ISalesforceUserProfile : IUserProfile
	{
        string MiddleInitial { get; set; }
        string NameSuffix { get; set; }
        string Salutation { get; set; }
        string BillCountry { get; set; }
        string BillAddress1 { get; set; }
        string BillAddress2 { get; set; }
        string BillCity { get; set; }
        string BillPostalCode { get; set; }
        string BillState { get; set; }
        string ShipCountry { get; set; }
        string ShipAddress1 { get; set; }
        string ShipAddress2 { get; set; }
        string ShipCity { get; set; }
        string ShipPostalCode { get; set; }
        string ShipState { get; set; }
        string CountryCode { get; set; }
        string Fax { get; set; }
        string PhoneExtension { get; set; }
        string Phone { get; set; }
        string PhoneType { get; set; }
        string Company { get; set; }
        string JobFunction { get; set; }
        string JobIndustry { get; set; }
        string JobTitle { get; set; }
    }
}
