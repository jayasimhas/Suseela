using System.Web;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
namespace Informa.Library.SalesforceConfiguration
{
    public interface ISalesforceConfigurationContext
    {
        bool IsNewSalesforceEnabled { get; }

        ISalesforce_Configuration SalesForceConfiguration { get; }

        string GetLoginEndPoints(string productCode, string callbackUrl, string state);

        string GetUserEntitlementsEndPoints(string userName);

        string GetUserAccessTokenEndPoints();

        string GetUserInfoEndPoints();

        string GetLogoutEndPoints();

        string GetRegistrationEndPoints(string referralurl, string referralid);

        string GetChangePasswordEndpoint(string referralurl, string referralid);

        string GetUpdateUserDetailsEndPoints(string userName);

        string GetUserProductPreferencesEndPoints(string query);

        string AddUserProductPreferencesEndPoints();

        string DeleteUserProductPreferenceEndPoints(string itemId);
    }
}
