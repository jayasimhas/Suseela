using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using Informa.Library.Salesforce.Properties;
using Informa.Library.Utilities.Settings;

namespace Informa.Library.Salesforce.Web
{
	public class UserAgentServiceContextEnabledCheckConfiguration : IUserAgentServiceContextEnabledCheckConfiguration
	{
	    public IEnumerable<string> UserAgents
        {
	        get
	        {
                return Sitecore.Configuration.Settings.GetSetting("Skipped.SF.User.Agent").Split('|').Select(s => s.Trim());
            }
        }
	}
}
