namespace Informa.Library.User.Profile
{
	public interface IUserProfile
	{
        string UserName { get; }
        string FirstName { get; }
		string LastName { get; }
		string Email { get; }
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
        string Mobile { get; set; }
    }
}
