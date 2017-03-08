using Informa.Library.User.Newsletter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceNewsletterFactory : ISalesforceNewsletterFactory
    {
        private const string EmailPreference = "Email Preference";
        private const string EmailSignUp = "Email Signup";
        public AddProductPreferenceRequest Create(string username, string verticalName, string publicationCode, IEnumerable<INewsletterUserOptIn> optIns)
        {
            if (optIns != null)
            {

                AddProductPreferenceRequest request = new AddProductPreferenceRequest();
                request.records = new List<ProductPreferenceRequestRecord>();
                foreach (var item in optIns)
                {
                    request.records.Add(
                   new ProductPreferenceRequestRecord()
                   {
                       attributes = new AddProductPreferenceRequestAttributes()
                       {
                           type = "Product_Preference__c",
                           referenceId = Guid.NewGuid().ToString()
                       },
                       Product_Vertical__c = verticalName,
                       Type__c = EmailPreference,
                       Username__c = username,
                       Value1__c = publicationCode,
                       Value2__c = item.OptIn.ToString(),
                       Value3__c = item.NewsletterType,
                       Value4__c = DateTime.Now.ToString()

                   });
                }

                if (request != null && request.records.Count() > 0)
                {
                    return request;
                }
            }

            return null;
        }

        public IList<INewsletterUserOptIn> Create(ProductPreferencesResult entity)
        {
            var result = new List<INewsletterUserOptIn>();

            if (entity != null && entity.records != null && entity.records.Any())
            {
                foreach (Record record in entity.records)
                {
                    result.Add(new NewsletterUserOptIn()
                    {
                        OptIn = Convert.ToBoolean(record.Value2__c),
                        NewsletterType = record.Value3__c,
                        SalesforceId = record.Id
                    });
                }
            }
            return result;
        }

        public UpdateProductPreferenceRequest CreateUpdateRequest(INewsletterUserOptIn entity)
        {
            if (entity != null)
            {
                return new UpdateProductPreferenceRequest()
                {
                    Value2__c = entity.OptIn.ToString(),
                    Value3__c = entity.NewsletterType,
                };
            }
            return null;
        }

        public AddProductPreferenceRequest Create(string username, string verticalName, string publicationCode, bool optIns)
        {
            return new AddProductPreferenceRequest()
            {
                records = new List<ProductPreferenceRequestRecord>()
                    {
                        new ProductPreferenceRequestRecord()
                        {
                            attributes = new AddProductPreferenceRequestAttributes()
                            {
                                    type = "Product_Preference__c",
                                    referenceId = Guid.NewGuid().ToString()
                            },
                            Product_Vertical__c = verticalName,
                            Type__c = EmailSignUp,
                            Username__c = username,
                            Value1__c = publicationCode,
                            Value2__c = optIns.ToString(),
                            Value3__c = DateTime.Now.ToString(),

                        }
                    }
            };
        }

        public UpdateProductPreferenceRequest CreateUpdateRequest(OffersOptIn Optin)
        {
            return new UpdateProductPreferenceRequest()
            {
                Value2__c = Optin.OptIn.ToString(),

            };
        }


        public IOffersOptIn CreateOfferOptinGetRequest(ProductPreferencesResult entity)
        {
            var result = new OffersOptIn();

            if (entity != null && entity.records != null && entity.records.Any())
            {
                foreach (Record record in entity.records)
                {
                    result.OptIn = Convert.ToBoolean(record.Value2__c);
                    result.SalesforceId = record.Id;
                }
            }
            return result;
        }
    }
}
