using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Globalization;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using System.Net.Http;
using Informa.Library.SalesforceConfiguration;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Informa.Library.Salesforce.V2.User.Profile
{
    public class SalesforceManageAccountInfoV2 : IManageAccountInfoV2
    {
        protected readonly ITextTranslator TextTranslator;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected string RequestFailedKey => TextTranslator.Translate("ContactInfo.RequestFailed");
        public SalesforceManageAccountInfoV2(
            ITextTranslator textTranslator,
            ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            TextTranslator = textTranslator;
            SalesforceConfigurationContext = salesforceConfigurationContext;
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

            UpdateUserDeatilsRequest request = new UpdateUserDeatilsRequest();
            request.Preferences = new List<SalesforceField>();

            // Name 
            request.Preferences.Add(new SalesforceField()
            { FieldName = "Salutation", FieldValue = Salutation, Reference = "Contact" });
            request.Preferences.Add(new SalesforceField()
            { FieldName = "FirstName", FieldValue = FirstName, Reference = "" });
            request.Preferences.Add(new SalesforceField()
            { FieldName = "LastName", FieldValue = LastName, Reference = "" });
            request.Preferences.Add(new SalesforceField()
            { FieldName = "MiddleName", FieldValue = MiddleInitial, Reference = "" });
            ////request.Preferences.Add(new SalesforceField()
            ////{ FieldName = "Title", FieldValue = NameSuffix, Reference = "Contact" });

            ////// Address
            ////request.Preferences.Add(new SalesforceField()
            ////{ FieldName = "MailingStreet", FieldValue = string.Format("{0}.&#{1}", BillAddress1, BillAddress2), Reference = "Contact" });
            ////request.Preferences.Add(new SalesforceField()
            ////{ FieldName = "FirstName", FieldValue = BillCity, Reference = "Contact" });
            ////request.Preferences.Add(new SalesforceField()
            ////{ FieldName = "CompanyName", FieldValue = Company, Reference = "Contact" });
            ////request.Preferences.Add(new SalesforceField()
            ////{ FieldName = "MiddleName", FieldValue = MiddleInitial, Reference = "Contact" });
            ////request.Preferences.Add(new SalesforceField()
            ////{ FieldName = "Title", FieldValue = NameSuffix, Reference = "Contact" });
            ////var EBIBillAddress = new EBI_Address()
            ////{
            ////    addressLine1 = BillAddress1,
            ////    addressLine2 = BillAddress2,
            ////    city = BillCity,
            ////    company = Company,
            ////    country = BillCountry,
            ////    postalCode = BillPostalCode,
            ////    stateProvince = BillState
            ////};

            ////var EBIShipAddress = new EBI_Address()
            ////{
            ////    addressLine1 = ShipAddress1,
            ////    addressLine2 = ShipAddress2,
            ////    city = ShipCity,
            ////    company = Company,
            ////    country = ShipCountry,
            ////    postalCode = ShipPostalCode,
            ////    stateProvince = ShipState
            ////};

            ////var EBIPhone = new EBI_PhoneFax()
            ////{
            ////    countryCode = CountryCode,
            ////    fax = Fax,
            ////    phoneExtension = PhoneExtension,
            ////    phoneNumber = Phone,
            ////    phoneType = PhoneType
            ////};

            request.Preferences.Add(new SalesforceField()
            { FieldName = "companyname", FieldValue = Company, Reference = "" });
            ////request.Preferences.Add(new SalesforceField()
            ////{ FieldName = "contact_jobtitle", FieldValue = JobTitle, Reference = "Contact" });
            ////request.Preferences.Add(new SalesforceField()
            ////{ FieldName = "contact_jobfunction", FieldValue = JobFunction, Reference = "Contact" });
            ////request.Preferences.Add(new SalesforceField()
            ////{ FieldName = "contact_industry", FieldValue = JobIndustry, Reference = "Contact" });


            ////EBI_UpdateProfileRequest r = new EBI_UpdateProfileRequest()
            ////{
            ////    profile = new EBI_Profile()
            ////    {
            ////        billingAddress = EBIBillAddress,
            ////        billingName = EBIName,
            ////        billingPhoneFax = EBIPhone,
            ////        companyJob = EBICompany,
            ////        name = EBIName,
            ////        phoneFax = EBIPhone,
            ////        shippingAddress = EBIShipAddress,
            ////        shippingName = EBIName,
            ////        shippingPhoneFax = EBIPhone
            ////    },
            ////    userName = user.Username
            ////};


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Entitlement_Api_Url?.Url);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.AccessToken);
                var content = new StringContent(JsonConvert.SerializeObject(request).ToString(), Encoding.UTF8, "application/json");
                var result = client.PostAsync(SalesforceConfigurationContext?.GetUpdateUserDetailsEndPoints(user.Username), content).Result;
                if (!result.IsSuccessStatusCode)
                {
                    return WriteErrorResult(RequestFailedKey);
                }
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
