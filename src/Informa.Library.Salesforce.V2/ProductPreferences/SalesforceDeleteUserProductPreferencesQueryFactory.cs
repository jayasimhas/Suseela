using Informa.Library.User.ProductPreferences;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceDeleteUserProductPreferencesQueryFactory : ISalesforceDeleteUserProductPreferencesQueryFactory
    {
        private const string SearchCriteria = "Search Criteria";
        private const string Bookmark = "Bookmark";
        private const string ContentPersonalization = "Content Filter";
        public string Create(string userName, string publicationCode, ProductPreferenceType type)
        {
            var query = string.Empty;
            if (type == ProductPreferenceType.SavedSearches)
            {
                query = "UserName__c='" + userName + "' and Type__c='" + SearchCriteria + "' and Value1__c='" + publicationCode + "'";
            }
            else if (type == ProductPreferenceType.SavedDocuments)
            {
                query = "UserName__c='" + userName + "' and Type__c='" + Bookmark + "' and Value1__c='" + publicationCode + "'";
            }
            else if (type == ProductPreferenceType.PersonalPreferences)
            {
                query = "UserName__c='" + userName + "' and Type__c='" + ContentPersonalization + "' and Value1__c='" + publicationCode + "'";
            }

            return query;
        }
    }
}
