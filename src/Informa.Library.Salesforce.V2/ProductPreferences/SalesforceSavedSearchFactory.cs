using Informa.Library.User.Search;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceSavedSearchFactory : ISalesforceSavedSearchFactory
    {
        public AddProductPreferenceRequest Create(ISavedSearchEntity entity)
        {
            if (entity != null)
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
                                    referenceId = "ref1"
                            },
                            Product_Vertical__c = entity.VerticalName,
                            Type__c = "Search Criteria",
                            Username__c = entity.Username,
                            Value1__c = entity.PublicationCode,
                            Value2__c = entity.SearchString,
                            Value3__c = entity.Publication,
                            Value4__c = entity.Name,
                            Value5__c = entity.HasAlert.ToString(),
                            Value6__c = entity.UnsubscribeToken

                        }
                    }
                };
            }

            return null;
        }

        public IList<ISavedSearchEntity> Create(ProductPreferencesResult entity)
        {
            var result = new List<ISavedSearchEntity>();

            if (entity != null && entity.records != null && entity.records.Any())
            {
                foreach (Record record in entity.records)
                {
                    result.Add(new SavedSearchEntity()
                    {
                        UnsubscribeToken = record.Value6__c,
                        HasAlert = Convert.ToBoolean(record.Value5__c),
                        Name = record.Value4__c,
                        Publication = record.Value3__c,
                        SearchString = record.Value2__c,
                        Id = record.Id

                    });
                }
            }

            return result;
        }
    }
}
