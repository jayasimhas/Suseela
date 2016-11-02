using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.User.Authentication;
using Informa.Library.User.Authentication.Web;

namespace Informa.Library.User.Profile
{
    public interface IManageAccountInfo
    {
        IAccountInfoWriteResult UpdatePassword(IAuthenticatedUser user, string curPassword, string newPassword, bool isTempPassword);

        IAccountInfoWriteResult UpdateContactInfo(IAuthenticatedUser user, string FirstName, string LastName,
            string MiddleInitial, string NameSuffix, string Salutation, string BillCountry, string BillAddress1, 
            string BillAddress2, string BillCity, string BillPostalCode, string BillState, string ShipCountry, 
            string ShipAddress1, string ShipAddress2, string ShipCity, string ShipPostalCode, string ShipState, 
            string Fax, string CountryCode, string PhoneExtension, string Phone, string PhoneType, 
            string Company, string JobFunction, string JobIndustry, string JobTitle);

        IAccountInfoWriteResult UpdateContactInfo(
          IAuthenticatedUser user,  
          string Id,
          string company,
          string jobTitle,
          string phone,
          string billAddress1,
          string billCity,
          string billPostalCode);
    }
}
