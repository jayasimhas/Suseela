using Informa.Library.User.UserPreference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceContentPreferencesFactory : ISalesforceContentPreferencesFactory
    {
        private const string ContentPersonalization = "Content Filter";
        public AddProductPreferenceRequest Create(string userName, string verticalPreferenceLocale,
            string publicationCode, string contentPreferences)
        {
            if (!string.IsNullOrWhiteSpace(contentPreferences))
            {

                AddProductPreferenceRequest request = new AddProductPreferenceRequest();
                request.records = new List<ProductPreferenceRequestRecord>();
                request.records.Add(
                    new ProductPreferenceRequestRecord()
                    {
                        attributes = new AddProductPreferenceRequestAttributes()
                        {
                            type = "Product_Preference__c",
                            referenceId = Guid.NewGuid().ToString()
                        },
                        Preference_Locale__c = verticalPreferenceLocale,
                        Type__c = ContentPersonalization,
                        Username__c = userName,
                        Value1__c = publicationCode,
                        Value6__c = contentPreferences,
                        Value9__c = DateTime.Now.ToString("yyyy-MM-dd"),
                    });
                return request;
            }
            return null;
        }

        public IUserPreferences Create(ProductPreferencesResult entity)
        {
            IUserPreferences userPreferences = null;
            if (entity != null && entity.records != null && entity.records.Any())
            {
                var record = entity.records.FirstOrDefault();
                userPreferences = JsonConvert.DeserializeObject<UserPreferences>(record.Value6__c.Replace("[CDATA[", "").Replace("]]", ""));
                if (userPreferences != null)
                {
                    userPreferences.LastUpdateOn = record.Value9__c;
                }
            }
            return userPreferences;
        }
    }
}
