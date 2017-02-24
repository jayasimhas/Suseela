using Informa.Library.User.ProductPreferences;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceGetUserProductPreferencesQueryFactory : ISalesforceGetUserProductPreferencesQueryFactory
    {
        private const string SearchCriteria = "Search Criteria";
        public string Create(string userName, string publicationCode, ProductPreferenceType type)
        {
            var query = string.Empty;
            if (type == ProductPreferenceType.SavedSearches)
            {
                query = "SELECT+Id, Username__c, Product_Vertical__c, Value1__c, Value2__c, Value3__c, Value4__c, Value5__c, Value6__c+from+Product_Preference__c+where+UserName__c='" + userName + "' and Type__c='" + SearchCriteria + "' and Value1__c='" + publicationCode + "'";
            }

            return query;
        }
    }
}
