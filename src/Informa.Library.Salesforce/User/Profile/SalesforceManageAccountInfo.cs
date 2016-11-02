using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Globalization;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using System.ServiceModel;
using enterprise = Informa.Library.Salesforce.SFv2;
using Informa.Library.User.Authentication.Web;

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

        public IAccountInfoWriteResult UpdateContactInfo(
           IAuthenticatedUser user,
           string Id,
           string company,
           string jobTitle,
           string phone,
           string billAddress1,
           string billCity,
           string billPostalCode)
        {
            string userId = user?.UserId;
            string url = user?.SalesForceURL;
            string sessionId = user?.SalesForceSessionId;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(url) || string.IsNullOrEmpty(sessionId))
            {
                return WriteErrorResult(RequestFailedKey);
            }

            enterprise.User_Preference__c userPreference = new enterprise.User_Preference__c()
            {
                Id = "a2E3E00000010TGUAY", // Id recieved while fetching details-  "a2E3E00000010TGUAY",

                Company__c = company,
                Job_or_function__c = jobTitle,
                Telephone__c = phone,
                Primary_Address__c = billAddress1,
                City_town__c = billCity,
                Postcode__c = billPostalCode
            };


            SFv2.SaveResult[] result = Update(sessionId, userId, url, userPreference);

            if (result.Length>0 &&  !result[0].success)
            {
                return WriteErrorResult(result[0].errors[0]?.message ?? RequestFailedKey);
            }

            return new AccountInfoWriteResult()
            {
                Success = true,
                Message = string.Empty
            };

        }

        private SFv2.SaveResult[] Update(string sessionId, string userId, string url, enterprise.User_Preference__c userPreference)
        {
            SFv2.SaveResult[] result = null;

            try
            {
                EndpointAddress EndpointAddr = new EndpointAddress(url);
                //instantiate session header object and set session id

                enterprise.SessionHeader header = new enterprise.SessionHeader();
                header.sessionId = sessionId;

                SFv2.SoapClient client = new SFv2.SoapClient("Soap", EndpointAddr);

                SFv2.sObject[] sobject = new enterprise.sObject[] { userPreference };
                SFv2.LimitInfo[] limitInfo = new SFv2.LimitInfo[] { };

                client.update(header, null, null, null, null, null, null, null, null, null, null, null, null, sobject, out limitInfo, out result);


                return result;

            }
            catch (System.Exception ex)
            {
                string s = ex.ToString();


            }

            return result;
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
