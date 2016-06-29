using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Globalization;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;

namespace Informa.Library.Salesforce.User.Profile
{
    public class SalesforceManageAccountInfo : IManageAccountInfo
    {
        protected readonly ISalesforceServiceContext Service;
        protected readonly ITextTranslator TextTranslator;

        protected string RequestFailedKey => TextTranslator.Translate("ContactInfo.RequestFailed");
        public SalesforceManageAccountInfo(
            ISalesforceServiceContext service,
            ITextTranslator textTranslator)
        {
            Service = service;
            TextTranslator = textTranslator;
        }

        public IAccountInfoWriteResult UpdatePassword(IAuthenticatedUser user, string curPassword, string newPassword, bool isTempPassword)
        {
            var response = Service.Execute(s => s.updatePassword(user.Username, curPassword, isTempPassword, newPassword));

            if (!response.IsSuccess())
            {
                return WriteErrorResult(response.errors?.First()?.message ?? RequestFailedKey);
            }

            return new AccountInfoWriteResult()
            {
                Success = true,
                Message = string.Empty
            };
        }

        public IAccountInfoWriteResult UpdateContactInfo(
            IAuthenticatedUser user,
            string FirstName,
            string LastName,
            string MiddleInitial,
            string NameSuffix,
            string Salutation,
            string BillCountry,
            string BillAddress1,
            string BillAddress2,
            string BillCity,
            string BillPostalCode,
            string BillState,
            string ShipCountry,
            string ShipAddress1,
            string ShipAddress2,
            string ShipCity,
            string ShipPostalCode,
            string ShipState,
            string Fax,
            string CountryCode,
            string PhoneExtension,
            string Phone,
            string PhoneType,
            string Company,
            string JobFunction,
            string JobIndustry,
            string JobTitle
            )
        {
            if (string.IsNullOrEmpty(user?.Username))
                return WriteErrorResult(RequestFailedKey);

            var EBIName = new EBI_Name()
            {
                firstName = FirstName,
                lastName = LastName,
                middleInitial = MiddleInitial,
                nameSuffix = NameSuffix,
                salutation = Salutation
            };

            var EBIBillAddress = new EBI_Address()
            {
                addressLine1 = BillAddress1,
                addressLine2 = BillAddress2,
                city = BillCity,
                company = Company,
                country = BillCountry,
                postalCode = BillPostalCode,
                stateProvince = BillState
            };

            var EBIShipAddress = new EBI_Address()
            {
                addressLine1 = ShipAddress1,
                addressLine2 = ShipAddress2,
                city = ShipCity,
                company = Company,
                country = ShipCountry,
                postalCode = ShipPostalCode,
                stateProvince = ShipState
            };

            var EBIPhone = new EBI_PhoneFax()
            {
                countryCode = CountryCode,
                fax = Fax,
                phoneExtension = PhoneExtension,
                phoneNumber = Phone,
                phoneType = PhoneType
            };

            var EBICompany = new EBI_CompanyJob()
            {
                company = Company,
                function = JobFunction,
                industry = JobIndustry,
                title = JobTitle
            };

            EBI_UpdateProfileRequest r = new EBI_UpdateProfileRequest()
            {
                profile = new EBI_Profile()
                {
                    billingAddress = EBIBillAddress,
                    billingName = EBIName,
                    billingPhoneFax = EBIPhone,
                    companyJob = EBICompany,
                    name = EBIName,
                    phoneFax = EBIPhone,
                    shippingAddress = EBIShipAddress,
                    shippingName = EBIName,
                    shippingPhoneFax = EBIPhone
                },
                userName = user.Username
            };

            var response = Service.Execute(s => s.updateProfile(r));

            if (!response.IsSuccess())
            {
                return WriteErrorResult(response.errors?.First()?.message ?? RequestFailedKey);
            }
                
            return new AccountInfoWriteResult()
            {
                Success = true,
                Message = string.Empty
            };
        }
        
        public IAccountInfoWriteResult WriteErrorResult(string message)
        {
            return new AccountInfoWriteResult
            {
                Success = false,
                Message = message
            };
        }
    }
}
