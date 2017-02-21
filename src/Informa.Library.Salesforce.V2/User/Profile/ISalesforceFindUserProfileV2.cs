using Informa.Library.Salesforce.User.Profile;

namespace Informa.Library.Salesforce.V2.User.Profile
{
    public interface ISalesforceFindUserProfileV2
    {
        ISalesforceUserProfile Find(string accessToken);
    }
}
