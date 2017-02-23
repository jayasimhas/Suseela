using Informa.Library.User.Authentication;
using Informa.Library.User.ProductPreferences;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceDeleteUserProductPreferences : ISalesforceDeleteUserProductPreferences
    {
        public bool DeleteUserProductPreference<T>(IAuthenticatedUser user, T value, ProductPreferenceType type)
        {
            return true;
        }
    }
}
