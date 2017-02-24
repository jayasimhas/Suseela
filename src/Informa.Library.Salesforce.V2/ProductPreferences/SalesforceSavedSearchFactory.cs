using Informa.Library.User.Search;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceSavedSearchFactory : ISalesforceSavedSearchFactory
    {
        private const string SearchCriteria = "Search Criteria";
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
                                    referenceId = Guid.NewGuid().ToString()
                            },
                            Product_Vertical__c = entity.VerticalName,
                            Type__c = SearchCriteria,
                            Username__c = entity.Username,
                            Value1__c = entity.PublicationCode,
                            Value2__c = entity.SearchString,
                            Value3__c = entity.Publication,
                            Value4__c = entity.Name,
                            Value5__c = entity.HasAlert.ToString(),
                            Value6__c = entity.UnsubscribeToken,
                            Value7__c = DateTime.Now.ToString()

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
                        Id = record.Id,
                        VerticalName = record.Product_Vertical__c,
                        PublicationCode = record.Value1__c,
                        Username = record.Username__c,
                        DateCreated = string.IsNullOrWhiteSpace(record.Value7__c) ? DateTime.MinValue : Convert.ToDateTime(record.Value7__c)
                    });
                }
            }

            return result;
        }

        public UpdateProductPreferenceRequest CreateUpdateRequest(ISavedSearchEntity entity)
        {
            if (entity != null)
            {
                return new UpdateProductPreferenceRequest()
                {
                    Value1__c = entity.PublicationCode,
                    Value2__c = entity.SearchString,
                    Value3__c = entity.Publication,
                    Value4__c = entity.Name,
                    Value5__c = entity.HasAlert.ToString(),
                    Value6__c = entity.UnsubscribeToken,
                };
            }
            return null;
        }
    }
}
