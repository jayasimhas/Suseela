using System;
using System.Collections.Generic;
using Informa.Library.User.Entitlement;

namespace Informa.Library.User.Authentication
{
	public class AuthenticatedUser : IAuthenticatedUser
	{
		public string Username { get; set; }
		public string Name { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public IList<IEntitlement> Entitlements { get; set; }

		public string ContactId	{ get; set; }

		public IList<string> AccountId { get; set; }

        public string SalesForceSessionId { get; set; }
        public string SalesForceURL { get; set; }
    }
}
