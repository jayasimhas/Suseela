using Informa.Library.User.ProductPreferences;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public interface ISalesforceDeleteUserProductPreferencesQueryFactory
    {
        string Create(string userName, string publicationCode, ProductPreferenceType type);
    }
}
