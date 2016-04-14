using System.Linq;

namespace Informa.Library.Salesforce.Web
{
	public class UserAgentServiceContextEnabledCheck : ISalesforceServiceContextEnabledCheck
	{
		protected readonly IUserAgentServiceContextEnabledCheckConfiguration Configuration;

		public UserAgentServiceContextEnabledCheck(
			IUserAgentServiceContextEnabledCheckConfiguration configuration)
		{
			Configuration = configuration;
		}

		public bool Enabled
		{
			get
			{
				Configuration.UserAgents.Any(ua => ua == "Check User Agent Logic"); // TODO-Ladan: Replace with business logic for checking against user agents

				return true;
			}
		}
	}
}
