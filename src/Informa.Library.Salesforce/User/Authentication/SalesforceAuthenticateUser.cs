using Informa.Library.Salesforce.EBIWebServices;
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
			var loginResponse = Service.Execute(s => s.login(username, password));
			
			if (!loginResponse.IsSuccess())
			{
				return ErrorResult;
			}

			var profileResponse = Service.Execute(s => s.queryProfileContactInformation(username));

			if (!profileResponse.IsSuccess())
			{
				return ErrorResult;
			}

			var profile = profileResponse.profile;

			return new SalesforceAuthenticateUserResult
			{
				State = AuthenticateUserResultState.Success,
				User = new SalesforceAuthenticatedUser
				{
					Username = username,
					Email = username,
					Name = string.Format("{0} {1}", profile.name.firstName, profile.name.lastName)
				}
			};
		}

		public SalesforceAuthenticateUserResult ErrorResult => new SalesforceAuthenticateUserResult
		{
			State = AuthenticateUserResultState.Failure
		};
	}
}
