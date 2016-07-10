using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Salesforce.Web
{
	public class UserAgentServiceContextEnabledCheckConfiguration : IUserAgentServiceContextEnabledCheckConfiguration
	{
		public UserAgentServiceContextEnabledCheckConfiguration()
		{
			_userAgents = new Lazy<string[]>(() => Sitecore.Configuration.Settings.GetSetting("Skipped.SF.User.Agent").Split('|').Select(s => s.Trim()).ToArray());
		}

		private readonly Lazy<string[]> _userAgents;
		public IEnumerable<string> UserAgents => _userAgents.Value;
	}
}
