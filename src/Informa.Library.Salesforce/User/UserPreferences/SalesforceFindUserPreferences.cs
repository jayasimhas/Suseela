namespace Informa.Library.Salesforce.User.UserPreferences
{
    using EBIWebServices;
    using Library.User.UserPreference;
    using Newtonsoft.Json;
    public class SalesforceFindUserPreferences : ISalesforceFindUserPreferences, IFindUserPreferences
    {
        protected readonly ISalesforceServiceContext Service;

        public SalesforceFindUserPreferences(ISalesforceServiceContext service)
        {
            Service = service;
        }
        public IUserPreferences Find(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }

            var preferencesResponse = Service.Execute(s => s.IN_queryProfilePreferences(username));

            if (!preferencesResponse.IsSuccess() || preferencesResponse.channelPreferences == null)
            {
                return null;
            }

            var result = JsonConvert.DeserializeObject<UserPreferences>(preferencesResponse.channelPreferences.Replace("[CDATA[", "").Replace("]]", ""));
            return (result as IUserPreferences);
        }

        IUserPreferences IFindUserPreferences.Find(string username)
        {
            return Find(username);
        }
    }
}
