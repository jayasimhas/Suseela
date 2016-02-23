using System;
using Informa.Library.User.Authentication;

namespace Informa.Library.Salesforce.User.Authentication
{
	public class SalesforceAuthenticateUser : ISalesforceAuthenticateUser
	{
		protected readonly ISalesforceServiceExecutor<ISalesforceServiceContext> ServiceExecutor;

		public SalesforceAuthenticateUser(
			ISalesforceServiceExecutor<ISalesforceServiceContext> serviceExecutor)
		{
			ServiceExecutor = serviceExecutor;
		}

		public IAuthenticateUserResult Authenticate(string username, string password)
		{
			var result = ServiceExecutor.Execute(() => ServiceExecutor.Service.login(username, password));

			throw new NotImplementedException();
		}
	}
}
