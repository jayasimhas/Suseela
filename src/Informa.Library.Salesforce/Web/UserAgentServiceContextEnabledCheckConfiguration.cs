using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Salesforce.Web
{
	public class UserAgentServiceContextEnabledCheckConfiguration : IUserAgentServiceContextEnabledCheckConfiguration
	{
		public IEnumerable<string> UserAgents => Enumerable.Empty<string>(); // TODO-Ladan: Replace with code to get the list of user agents
	}
}
