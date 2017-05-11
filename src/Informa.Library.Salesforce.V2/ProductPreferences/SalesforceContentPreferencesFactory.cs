using Informa.Library.User.UserPreference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

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
                        Value6__c = CompressString(contentPreferences),
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
                var prefrenceString = DeCompressString(record.Value6__c);
                if (!string.IsNullOrWhiteSpace(prefrenceString))
                {
                    userPreferences = JsonConvert.DeserializeObject<UserPreferences>(prefrenceString.Replace("[CDATA[", "").Replace("]]", ""));
                    if (userPreferences != null)
                    {
                        userPreferences.LastUpdateOn = record.Value9__c;
                    }
                }
            }
            return userPreferences;
        }

        private string CompressString(string data)
        {
            string result;
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(ms, CompressionMode.Compress))
                using (var writer = new BinaryWriter(gzip, Encoding.UTF8))
                {
                    writer.Write(data);
                }
                ms.Flush();
                result = Convert.ToBase64String(ms.ToArray());
            }
            return result;
        }

        private string DeCompressString(string data)
        {
            string result;
            byte[] itemData = Convert.FromBase64String(data);
            using (MemoryStream src = new MemoryStream(itemData))
            using (GZipStream gzs = new GZipStream(src, CompressionMode.Decompress))
            using (var reader = new BinaryReader(gzs, Encoding.UTF8))
            {
                result = reader.ReadString();
            }
            return result;
        }
    }
}
