using Informa.Library.User.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceSaveDocumentFactory : ISalesforceSaveDocumentFactory
    {
        private const string SaveDocument = "Bookmark";
        public AddProductPreferenceRequest Create(string verticalName, string publicationCode, string Username, string documentName, string documentDescription, string documentId)
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
                            Type__c = SaveDocument,
                            Username__c = Username,
                            Value1__c = publicationCode,
                            Value2__c = documentName,
                            Value3__c = documentDescription,
                            Value4__c = documentId,
                            Value5__c = DateTime.Now.ToString(),

                        }
                    }
            };
        }

        public IList<ISavedDocument> Create(ProductPreferencesResult entity)
        {
            var result = new List<ISavedDocument>();

            if (entity != null && entity.records != null && entity.records.Any())
            {
                foreach (Record record in entity.records)
                {
                    result.Add(new SavedDocument()
                    {
                      DocumentId = record.Value4__c,
                      Description = record.Value3__c,
                      Name = record.Value2__c,
                      SalesforceId = record.Id,
                      SaveDate= string.IsNullOrWhiteSpace(record.Value5__c) ? DateTime.MinValue : Convert.ToDateTime(record.Value5__c)
                    });
                }
            }

            return result;
        }
    }
}
