using Informa.Library.User.ProductPreferences;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceGetUserProductPreferencesQueryFactory : ISalesforceGetUserProductPreferencesQueryFactory
    {
        private const string SearchCriteria = "Search Criteria";
        private const string Bookmark = "Bookmark";
        private const string ContentPersonalization = "Content Filter";
        public string Create(string userName, string verticle, string publicationCode, ProductPreferenceType type)
        {
            var query = string.Empty;
            ////if (type == ProductPreferenceType.SavedSearches)
            ////{
            ////    query = "SELECT+Id, Username__c, Product_Vertical__c, Value1__c, Value2__c, Value3__c, Value4__c, Value5__c, Value6__c, Value7__c, Value8__c+from+Product_Preference__c+where+UserName__c='" + userName + "' and Type__c='" + SearchCriteria + "' and Value1__c='" + publicationCode + "'";
            ////}
            ////else if (type == ProductPreferenceType.SavedDocuments)
            ////{
            ////    query = "SELECT+Id, Username__c, Product_Vertical__c, Value1__c, Value2__c, Value3__c, Value4__c, Value5__c, Value6__c, Value7__c, Value8__c+from+Product_Preference__c+where+UserName__c='" + userName + "' and Type__c='" + Bookmark + "' and Value1__c='" + publicationCode + "'";
            ////}
            if (type == ProductPreferenceType.SavedSearches)
            {
                query = "SELECT+Id, Username__c, Product_Vertical__c, Value1__c, Value2__c, Value3__c, Value4__c, Value5__c, Value6__c, Value7__c, Value8__c+from+Product_Preference__c+where+UserName__c='" + userName + "' and Type__c='" + SearchCriteria + "' and Product_Vertical__c='" + verticle + "'";
            }
            else if (type == ProductPreferenceType.SavedDocuments)
            {
                query = "SELECT+Id, Username__c, Product_Vertical__c, Value1__c, Value2__c, Value3__c, Value4__c, Value5__c, Value6__c, Value7__c, Value8__c+from+Product_Preference__c+where+UserName__c='" + userName + "' and Type__c='" + Bookmark + "' and Product_Vertical__c='" + verticle + "'";
            }
            else if (type == ProductPreferenceType.PersonalPreferences)
            {
                query = "SELECT+Id, Username__c, Product_Vertical__c, Value1__c, Value2__c, Value3__c, Value4__c, Value5__c, Value6__c, Value7__c, Value8__c+from+Product_Preference__c+where+UserName__c='" + userName + "' and Type__c='" + ContentPersonalization + "' and Value1__c='" + publicationCode + "'";
            }
            
            return query;
        }
    }
}
