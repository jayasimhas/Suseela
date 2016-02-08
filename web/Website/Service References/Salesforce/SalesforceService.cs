using Salesforce.Common;
using Salesforce.Force;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Web.Salesforce
{
    /// <summary>
    /// Provides methods to interact with a Salesforce service.
    /// </summary>
    public static class SalesforceService
    {
        /// <summary>
        /// Gets a ForceClient that has been authenticated using the UserName, Password, and SecurityToken settings
        /// specified in the config file.
        /// </summary>
        /// <returns>The authenticated ForceClient.</returns>
        public static async Task<ForceClient> GetUserNamePasswordForceClientAsync()
        {
//#error Update the config file with the connected app's UserName, Password and SecurityToken. For more information, see the Next Steps section of the Getting Started documentation.

            using (AuthenticationClient authenticationClient = new AuthenticationClient())
            {
                await authenticationClient.UsernamePasswordAsync(
                    SalesforceService.GetAppSetting("Salesforce:ConsumerKey"),
                    SalesforceService.GetAppSetting("Salesforce:ConsumerSecret"),
                    SalesforceService.GetAppSetting("Salesforce:UserName"),
                    SalesforceService.GetAppSetting("Salesforce:Password") + SalesforceService.GetAppSetting("Salesforce:SecurityToken", true),
                    SalesforceService.GetAppSetting("Salesforce:Domain") + "/services/oauth2/token");

                return new ForceClient(
                    authenticationClient.InstanceUrl,
                    authenticationClient.AccessToken,
                    authenticationClient.ApiVersion);
            }
        }

        /// <summary>
        /// Gets the value of the AppSetting with the specified name from the config file.
        /// </summary>
        /// <param name="name">The name of the AppSetting to retrieve.</param>
        /// <param name="isOptional">A Boolean value indicating whether or not the AppSetting is considered optional.</param>
        /// <exception cref="InvalidOperationException">If isOptional is set to false and the AppSetting is not found.</exception>
        /// <returns>
        /// The value of the AppSetting if found.  If isOptional is set to true and the AppSetting is not found, null is returned.
        /// </returns>
        internal static string GetAppSetting(string name, bool isOptional = false)
        {
            string setting = ConfigurationManager.AppSettings[name];
            if (!isOptional && (String.IsNullOrWhiteSpace(setting) || string.Equals(setting, "RequiredValue", StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.InvariantCulture, "The value for the '{0}' key is missing from the appSettings section of the config file.", name));
            }
            else if (isOptional && (String.IsNullOrWhiteSpace(setting) || string.Equals(setting, "OptionalValue", StringComparison.OrdinalIgnoreCase)))
            {
                setting = null;
            }

            return setting;
        }
    }
}