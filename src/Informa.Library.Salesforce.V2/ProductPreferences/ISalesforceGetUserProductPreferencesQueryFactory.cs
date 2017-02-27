using Informa.Library.User.ProductPreferences;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public interface ISalesforceGetUserProductPreferencesQueryFactory
    {
        string Create(string userName, string verticle,string publicationCode, ProductPreferenceType type);
    }
}
