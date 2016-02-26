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
			SalesforceAuthenticateUserResult errorResult;
			var loginResponse = Service.Execute(s => s.login(username, password));
			
			if (IsFailure(loginResponse, out errorResult))
			{
				return errorResult;
			}

			var profileResponse = Service.Execute(s => s.queryProfileContactInformation(username));

			if (IsFailure(profileResponse, out errorResult))
			{
				return errorResult;
			}

			var profile = profileResponse.profile;

			return new SalesforceAuthenticateUserResult
			{
				State = AuthenticateUserResultState.Success,
				User = new SalesforceUser
				{
					Username = username,
					Email = username,
					Name = string.Format("{0} {1}", profile.name.firstName, profile.name.lastName)
				}
			};
		}

		public bool IsFailure(IEbiResponse response, out SalesforceAuthenticateUserResult errorResult)
		{
			if (response.success.HasValue && response.success.Value)
			{
				errorResult = null;

				return true;
			}

			errorResult = new SalesforceAuthenticateUserResult
			{
				State = AuthenticateUserResultState.Failure
			};

			return true;
		}
	}
}
