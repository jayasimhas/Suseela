namespace Informa.Library.Salesforce.User.UserPreferences
{
    using Informa.Library.User.UserPreference;

    public interface ISalesforceSetuserPreferences
    {
        bool Set(string username, string channelPreferences);
    }
}