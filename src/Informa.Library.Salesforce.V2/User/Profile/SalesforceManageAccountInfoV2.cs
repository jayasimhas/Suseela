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
using Sitecore.Configuration;

namespace Informa.Library.Salesforce.V2.User.Profile
{
    public class SalesforceManageAccountInfoV2 : IManageAccountInfoV2
    {
        protected readonly ITextTranslator TextTranslator;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly ISalesforceInfoLogger InfoLogger;
        protected string RequestFailedKey => TextTranslator.Translate("ContactInfo.RequestFailed");
        private string SalutationFieldName => TextTranslator.Translate("ContactInfo.SalesforceFields.Salutation");
        private string FirstNameFieldName => TextTranslator.Translate("ContactInfo.SalesforceFields.FirstName");
        private string MiddleNameFieldName => TextTranslator.Translate("ContactInfo.SalesforceFields.MiddleName");
        private string LastNameFieldName => TextTranslator.Translate("ContactInfo.SalesforceFields.LastName");
        private string CompanyNameFieldName => TextTranslator.Translate("ContactInfo.SalesforceFields.CompanyName");
        private string JobTitleFieldName => TextTranslator.Translate("ContactInfo.SalesforceFields.JobTitle");
        private string Industry__cFieldName => TextTranslator.Translate("ContactInfo.SalesforceFields.Industry__c");
        private string Job_Function__cFieldName => TextTranslator.Translate("ContactInfo.SalesforceFields.Job_Function__c");
        private string PhoneFieldName => TextTranslator.Translate("ContactInfo.SalesforceFields.Phone");
        private string MailingStreetFieldName => TextTranslator.Translate("ContactInfo.SalesforceFields.MailingStreet");
        private string MailingCityFieldName => TextTranslator.Translate("ContactInfo.SalesforceFields.MailingCity");
        private string MailingStateFieldName => TextTranslator.Translate("ContactInfo.SalesforceFields.MailingState");
        private string MailingCountryFieldName => TextTranslator.Translate("ContactInfo.SalesforceFields.MailingCountry");
        private string MailingPostalCodeFieldName => TextTranslator.Translate("ContactInfo.SalesforceFields.MailingPostalCode");
        private string ContactReferenceName => TextTranslator.Translate("ContactInfo.Reference.Contact");
        private string MobileFieldName => TextTranslator.Translate("ContactInfo.SalesforceFields.Mobile");
        public SalesforceManageAccountInfoV2(
ITextTranslator textTranslator,
ISalesforceConfigurationContext salesforceConfigurationContext,
ISalesforceInfoLogger infoLogger)
        {
            TextTranslator = textTranslator;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            InfoLogger = infoLogger;
        }

        public IAccountInfoWriteResult UpdateContactInfo(
            IAuthenticatedUser user,
            string FirstName,
            string LastName,
            string MiddleInitial,
            string Salutation,
            string ShipCountry,
            string ShipAddress1,
            string ShipCity,
            string ShipPostalCode,
            string ShipState,
            string Phone,
                string Mobile,
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
            if (!string.IsNullOrWhiteSpace(SalutationFieldName) && !string.IsNullOrWhiteSpace(ContactReferenceName))
                request.Preferences.Add(new SalesforceField()
                { FieldName = SalutationFieldName, FieldValue = Salutation, Reference = ContactReferenceName });

            if (!string.IsNullOrWhiteSpace(FirstNameFieldName))
                request.Preferences.Add(new SalesforceField()
                { FieldName = FirstNameFieldName, FieldValue = FirstName, Reference = "" });

            if (!string.IsNullOrWhiteSpace(LastNameFieldName))
                request.Preferences.Add(new SalesforceField()
                { FieldName = LastNameFieldName, FieldValue = LastName, Reference = "" });

            if (!string.IsNullOrWhiteSpace(MiddleNameFieldName))
                request.Preferences.Add(new SalesforceField()
                { FieldName = MiddleNameFieldName, FieldValue = MiddleInitial, Reference = "" });

            if (!string.IsNullOrWhiteSpace(CompanyNameFieldName))
                request.Preferences.Add(new SalesforceField()
                { FieldName = CompanyNameFieldName, FieldValue = Company, Reference = "" });

            if (!string.IsNullOrWhiteSpace(JobTitleFieldName) && !string.IsNullOrWhiteSpace(ContactReferenceName))
                request.Preferences.Add(new SalesforceField()
                { FieldName = JobTitleFieldName, FieldValue = JobTitle, Reference = ContactReferenceName });

            if (!string.IsNullOrWhiteSpace(Job_Function__cFieldName) && !string.IsNullOrWhiteSpace(ContactReferenceName))
                request.Preferences.Add(new SalesforceField()
                { FieldName = Job_Function__cFieldName, FieldValue = JobFunction, Reference = ContactReferenceName });

            if (!string.IsNullOrWhiteSpace(Industry__cFieldName) && !string.IsNullOrWhiteSpace(ContactReferenceName))
                request.Preferences.Add(new SalesforceField()
                { FieldName = Industry__cFieldName, FieldValue = JobIndustry, Reference = ContactReferenceName });

            if (!string.IsNullOrWhiteSpace(PhoneFieldName) && !string.IsNullOrWhiteSpace(ContactReferenceName))
                request.Preferences.Add(new SalesforceField()
                {
                    FieldName = PhoneFieldName,
                    FieldValue = Phone,
                    Reference = ContactReferenceName
                });

            if (!string.IsNullOrWhiteSpace(MobileFieldName) && !string.IsNullOrWhiteSpace(ContactReferenceName))
                request.Preferences.Add(new SalesforceField()
                {
                    FieldName = MobileFieldName,
                    FieldValue = Mobile,
                    Reference = ContactReferenceName
                });

            // Address
            if (!string.IsNullOrWhiteSpace(MailingStreetFieldName) && !string.IsNullOrWhiteSpace(ContactReferenceName))
                request.Preferences.Add(new SalesforceField()
                { FieldName = MailingStreetFieldName, FieldValue = ShipAddress1, Reference = ContactReferenceName });
            if (!string.IsNullOrWhiteSpace(MailingCityFieldName) && !string.IsNullOrWhiteSpace(ContactReferenceName))
                request.Preferences.Add(new SalesforceField()
                { FieldName = MailingCityFieldName, FieldValue = ShipCity, Reference = ContactReferenceName });
            if (!string.IsNullOrWhiteSpace(MailingStateFieldName) && !string.IsNullOrWhiteSpace(ContactReferenceName))
                request.Preferences.Add(new SalesforceField()
                { FieldName = MailingStateFieldName, FieldValue = ShipState, Reference = ContactReferenceName });
            if (!string.IsNullOrWhiteSpace(MailingCountryFieldName) && !string.IsNullOrWhiteSpace(ContactReferenceName))
                request.Preferences.Add(new SalesforceField()
                { FieldName = MailingCountryFieldName, FieldValue = ShipCountry, Reference = ContactReferenceName });
            if (!string.IsNullOrWhiteSpace(MailingPostalCodeFieldName) && !string.IsNullOrWhiteSpace(ContactReferenceName))
                request.Preferences.Add(new SalesforceField()
                { FieldName = MailingPostalCodeFieldName, FieldValue = ShipPostalCode, Reference = ContactReferenceName });


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Entitlement_Api_Url?.Url);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.AccessToken);
                var content = new StringContent(JsonConvert.SerializeObject(request).ToString(), Encoding.UTF8, "application/json");
                var result = client.PostAsync(SalesforceConfigurationContext?.GetUpdateUserDetailsEndPoints(user.Username), content).Result;

                InfoLogger.Log(SalesforceConfigurationContext?.GetUpdateUserDetailsEndPoints(user.Username), this.GetType().Name);
                InfoLogger.Log(result.Content.ReadAsStringAsync().Result, this.GetType().Name);
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
