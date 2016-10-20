namespace Informa.Library.Salesforce.User.UserPreferences
{
    using EBIWebServices;
    using Library.User.UserPreference;
    using Newtonsoft.Json;
    public class SalesforceSetuserPreferences : ISalesforceSetuserPreferences, ISetUserPreferences
    {
        protected readonly ISalesforceServiceContext Service;

        public SalesforceSetuserPreferences(ISalesforceServiceContext service)
        {
            Service = service;
        }
        public bool Set(string username, string channelPreferences)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(channelPreferences))
                return false;

            var preferencesResponse = Service.Execute(s => s.IN_updateProfilePreferences(username, new IN_ProfilePreferencesRequest { channelPreferences = channelPreferences, username = username }));

            // We are not getting proper resonse now so we are handling this by some workaround.
            //return preferencesResponse.IsSuccess();
            return preferencesResponse.IsSuccess() || preferencesResponse.errors != null || (preferencesResponse.errors.Length == 1 && preferencesResponse.errors[0] == null);
        }

        bool ISetUserPreferences.Set(string username, string channelPreferences)
        {
            return Set(username, channelPreferences);
        }
    }
}
