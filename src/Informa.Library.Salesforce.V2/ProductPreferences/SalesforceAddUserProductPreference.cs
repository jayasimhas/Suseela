using Informa.Library.User.Authentication;
using Informa.Library.User.ProductPreferences;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceAddUserProductPreference : ISalesforceAddUserProductPreference
    {
        public bool AddUserProductPreference<T>(IAuthenticatedUser user, T value, ProductPreferenceType type)
        {
            return true;
        }
    }
}
