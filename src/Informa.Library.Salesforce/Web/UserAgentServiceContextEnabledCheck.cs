﻿using System;
using Informa.Library.Session;
using System.Linq;
using System.Net;
using System.Net.Http;
using Sitecore.Configuration;
using System.Web;

namespace Informa.Library.Salesforce.Web
{
	public class UserAgentServiceContextEnabledCheck : ISalesforceServiceContextEnabledCheck
	{
		private const string sessionKey = nameof(UserAgentServiceContextEnabledCheck);

		protected readonly ISessionStore SessionStore;
		protected readonly IUserAgentServiceContextEnabledCheckConfiguration Configuration;

		public UserAgentServiceContextEnabledCheck(
			ISessionStore sessionStore,
			IUserAgentServiceContextEnabledCheckConfiguration configuration)
		{
			SessionStore = sessionStore;
			Configuration = configuration;
		}

		public bool Enabled
		{
			get
			{
				var enabledSession = SessionStore.Get<bool>(sessionKey);

				if (enabledSession.HasValue)
				{
					return enabledSession.Value;
				}

			    return !(Configuration.UserAgents.Any(ua => ua == HttpContext.Current.Request.UserAgent));
			}
			set
			{
				SessionStore.Set(sessionKey, value);
			}
		}
	}
}
