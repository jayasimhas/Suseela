namespace Informa.Library.Salesforce.V2.User.Profile
{
    public interface ISalesforceFindUserInfo
    {
        ISalesforceUserInfo Find(string accessToken);
    }
}
