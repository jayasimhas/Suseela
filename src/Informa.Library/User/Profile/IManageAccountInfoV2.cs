using Informa.Library.User.Authentication;

namespace Informa.Library.User.Profile
{
    public interface IManageAccountInfoV2
    {
        IAccountInfoWriteResult UpdateContactInfo(IAuthenticatedUser user, string FirstName, string LastName,
            string MiddleInitial, string NameSuffix, string Salutation, string BillCountry, string BillAddress1,
            string BillAddress2, string BillCity, string BillPostalCode, string BillState, string ShipCountry,
            string ShipAddress1, string ShipAddress2, string ShipCity, string ShipPostalCode, string ShipState,
            string Fax, string CountryCode, string PhoneExtension, string Phone, string PhoneType,
            string Company, string JobFunction, string JobIndustry, string JobTitle, string Mobile);
    }
}
