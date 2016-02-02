﻿using System;
using System.Web.Services.Protocols;
using SitecoreTreeWalker.SitecoreTree;
using SitecoreTreeWalker.Config;

namespace SitecoreTreeWalker.User
{
	public class SitecoreUser
	{
		private static readonly SitecoreUser User = new SitecoreUser();
		protected SCTree _scTree;
		public const string DefaultDomain = @"sitecore";

		/// <summary>
		/// True if a user is logged in; otherwise, false.
		/// </summary>
		public bool IsLoggedIn { get; set; }

		/// <summary>
		/// Name of logged in user. Null if no user is logged in.
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// Name of logged in user. Null if no user is logged in.
		/// </summary>
		public string Password { get; set; }

		private SitecoreUser()
		{
			IsLoggedIn = false;
			_scTree = new SCTree();
		}



		/// <summary>
		/// Get singleton user
		/// </summary>
		/// <returns>The user (singleton)</returns>
		public static SitecoreUser GetUser()
		{
			return User;
		}

		public delegate void LoginEventHandler(object sender, EventArgs e);

		public void ResetAuthenticatedSubscription()
		{
			Authenticated = null;
		}

		public event LoginEventHandler Authenticated;

		public void InvokeAuthenticated(EventArgs e)
		{
			LoginEventHandler handler = Authenticated;
			if (handler != null) handler(this, e);
		}

		/// <summary>
		/// Authenticates username and password.
		/// </summary>
		/// <param name="username">User's username</param>
		/// <param name="password">User's password</param>
		/// <returns>True if authentication successful; otherwise, false.</returns>
		public UserStatusStruct Authenticate(string username, string password)
		{
			Globals.SitecoreAddin.Log("SitecoreUser.Authenticate: Trying to authenticate user [" + username + "]...");
			var domainAndUsername = DefaultDomain + @"\" + username;
			try
			{
                _scTree.Url = Constants.EDITOR_ENVIRONMENT_LOGINURL;
				var userStatus = SitecoreArticle.AuthenticateUser(domainAndUsername, password);
				if (userStatus.LoginSuccessful)
				{
					Globals.SitecoreAddin.Log("SitecoreUser.Authenticate: Authentication succeeded.");
					IsLoggedIn = true;
					Username = domainAndUsername;
					Password = password;
					InvokeAuthenticated(EventArgs.Empty);
				}
				return userStatus;
			}
			catch (SoapException ex)
			{
				Globals.SitecoreAddin.LogException("SitecoreUser.Authenticate: Error during login or login failed!", ex);
				throw ex;
			}
		}

		/// <summary>
		/// Logs user out
		/// </summary>
		/// <returns>True if logout successful; otherwise, false (eg, no logged in user to logout)</returns>
		public bool Logout()
		{
			Globals.SitecoreAddin.Log("SitecoreUser.Logout: Attempting to log out...");
			if(!IsLoggedIn)
			{
				Globals.SitecoreAddin.Log("SitecoreUser.Logout: Logout failed because no user is logged in.");
				return false;
			}
			Globals.SitecoreAddin.Log("SitecoreUser.Logout: Logout succeeded.");
			Username = null;
			IsLoggedIn = false;
			return true;
		}
	}
}
