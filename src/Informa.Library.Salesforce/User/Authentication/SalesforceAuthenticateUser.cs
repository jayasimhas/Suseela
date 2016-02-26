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
			
			if (!result.success.HasValue || !result.success.Value)
			{
				return new SalesforceAuthenticateUserResult
				{
					State = AuthenticateUserResultState.Failure
				};
			}
			
			return new SalesforceAuthenticateUserResult
			{
				State = AuthenticateUserResultState.Success,
				User = new SalesforceUser
				{
					Username = username,
					Email = username,
					Name = username
				}
			};
		}
	}
}
