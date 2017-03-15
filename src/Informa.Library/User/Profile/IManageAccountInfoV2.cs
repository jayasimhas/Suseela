using Informa.Library.User.Authentication;

namespace Informa.Library.User.Profile
{
    public interface IManageAccountInfoV2
    {
        IAccountInfoWriteResult UpdateContactInfo(IAuthenticatedUser user, string FirstName, string LastName,
            string MiddleInitial, string Salutation, string ShipCountry,
            string ShipAddress1, string ShipCity, string ShipPostalCode, string ShipState,
            string Phone, string Mobile, string Company, string JobFunction, string JobIndustry, string JobTitle);
    }
}
