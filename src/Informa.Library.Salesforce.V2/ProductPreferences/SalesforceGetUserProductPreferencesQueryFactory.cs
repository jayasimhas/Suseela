using Informa.Library.User.ProductPreferences;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceGetUserProductPreferencesQueryFactory : ISalesforceGetUserProductPreferencesQueryFactory
    {
        private const string SearchCriteria = "Search Criteria";
        private const string Bookmark = "Bookmark";
        private const string ContentPersonalization = "Content Filter";
        private const string EmailSignUp = "Email SignUp";
        private const string EmailPreference = "Email Preference";



        public string Create(string userName, string verticalPreferenceLocale, string publicationCode, ProductPreferenceType type)
        {
            var query = string.Empty;
          
            if (type == ProductPreferenceType.SavedSearches)
            {
                query = "SELECT+Id, Username__c, Preference_Locale__c, Value1__c, Value2__c, Value3__c, Value4__c, Value5__c, Value6__c, Value7__c, Value8__c, Value9__c+from+Product_Preference__c+where+UserName__c='" + userName + "' and Type__c='" + SearchCriteria + "' and Preference_Locale__c='" + verticalPreferenceLocale + "'";
            }
            else if (type == ProductPreferenceType.SavedDocuments)
            {
                query = "SELECT+Id, Username__c, Preference_Locale__c, Value1__c, Value2__c, Value3__c, Value4__c, Value5__c, Value6__c, Value7__c, Value8__c, Value9__c+from+Product_Preference__c+where+UserName__c='" + userName + "' and Type__c='" + Bookmark + "' and Preference_Locale__c='" + verticalPreferenceLocale + "'";
            }
            else if (type == ProductPreferenceType.ContentPreferences)
            {
                query = "SELECT+Id, Username__c, Preference_Locale__c, Value1__c, Value2__c, Value3__c, Value4__c, Value5__c, Value6__c, Value7__c, Value8__c, Value9__c+from+Product_Preference__c+where+UserName__c='" + userName + "' and Type__c='" + ContentPersonalization + "' and Value1__c='" + publicationCode + "'";
            }
            else if(type == ProductPreferenceType.EmailPreference)
            {
                query = "SELECT+Id, Username__c, Preference_Locale__c, Value1__c, Value2__c, Value3__c, Value4__c, Value5__c, Value6__c, Value7__c, Value8__c, Value9__c+from+Product_Preference__c+where+UserName__c='" + userName + "' and Type__c='" + EmailPreference + "' and Preference_Locale__c='" + verticalPreferenceLocale + "'";
            }
            else if (type == ProductPreferenceType.EmailSignUp)
            {
                query = "SELECT+Id, Username__c, Preference_Locale__c, Value1__c, Value2__c, Value3__c, Value4__c, Value5__c, Value6__c, Value7__c, Value8__c, Value9__c+from+Product_Preference__c+where+UserName__c='" + userName + "' and Type__c='" + EmailSignUp + "' and Preference_Locale__c='" + verticalPreferenceLocale + "'";
            }

            return query;
        }
    }
}
