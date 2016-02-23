using System;
using Informa.Library.User.Authentication;

namespace Informa.Library.Salesforce.User.Authentication
{
	public class SalesforceAuthenticateUser : ISalesforceAuthenticateUser
	{
		protected readonly ISalesforceServiceContext Service;

		public SalesforceAuthenticateUser(
			ISalesforceServiceContext service)
		{
			Service = service;
		}

		public IAuthenticateUserResult Authenticate(string username, string password)
		{
			var result = Service.Execute(s => s.login(username, password));
			
			throw new NotImplementedException();
		}
	}
}
