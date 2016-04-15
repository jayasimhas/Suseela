using System.Collections.Generic;

namespace Informa.Library.Salesforce.Web
{
	public interface IUserAgentServiceContextEnabledCheckConfiguration
	{
		IEnumerable<string> UserAgents { get; }
	}
}