﻿using Informa.Library.User.Document;
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
        public AddProductPreferenceRequest Create(string verticalPreferenceLocale, string publicationCode, string Username, string documentName, string documentDescription, string documentId)
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
                            Preference_Locale__c = verticalPreferenceLocale,
                            Type__c = SaveDocument,
                            Username__c = Username,
                            Value1__c = publicationCode,
                            Value2__c = documentName,
                            Value3__c = documentDescription,
                            Value4__c = documentId,
                            Value9__c = DateTime.Now.ToString("yyyy-MM-dd")

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
                      SaveDate= Convert.ToDateTime(record.Value9__c)
                    });
                }
            }

            return result;
        }
    }
}
