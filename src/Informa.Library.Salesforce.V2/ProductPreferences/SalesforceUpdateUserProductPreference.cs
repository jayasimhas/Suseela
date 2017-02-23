using Informa.Library.User.Authentication;
using Informa.Library.User.ProductPreferences;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceUpdateUserProductPreference : ISalesforceUpdateUserProductPreference
    {
        public bool UpdateUserProductPreference<T>(IAuthenticatedUser user, T value, ProductPreferenceType type)
        {
            return true;
        }
    }
}
