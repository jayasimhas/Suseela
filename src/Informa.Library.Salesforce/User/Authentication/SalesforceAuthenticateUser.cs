using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.Salesforce.User.Profile;
using Informa.Library.User.Authentication;

namespace Informa.Library.Salesforce.User.Authentication
{
	public class SalesforceAuthenticateUser : ISalesforceAuthenticateUser
	{
		protected readonly ISalesforceServiceContext Service;
		protected readonly ISalesforceFindUserProfile FindUserProfile;

		public SalesforceAuthenticateUser(
			ISalesforceServiceContext service,
			ISalesforceFindUserProfile findUserProfile)
		{
			Service = service;
			FindUserProfile = findUserProfile;
		}

		public IAuthenticateUserResult Authenticate(string username, string password)
		{
			var loginResponse = Service.Execute(s => s.login(username, password));
			
			if (!loginResponse.IsSuccess())
			{
				return ErrorResult;
			}

			var profile = FindUserProfile.Find(username);

			if (profile == null)
			{
				return ErrorResult;
			}

			return new SalesforceAuthenticateUserResult
			{
				State = AuthenticateUserResultState.Success,
				User = new SalesforceAuthenticatedUser
				{
					Username = username,
					Email = username,
					Name = string.Format("{0} {1}", profile.FirstName, profile.LastName)
				}
			};
		}

		public SalesforceAuthenticateUserResult ErrorResult => new SalesforceAuthenticateUserResult
		{
			State = AuthenticateUserResultState.Failure
		};
	}
}
