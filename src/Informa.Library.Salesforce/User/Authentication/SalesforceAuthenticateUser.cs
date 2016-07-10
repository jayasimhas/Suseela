using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.Salesforce.User.Profile;
using Informa.Library.User;
using Informa.Library.User.Authentication;
using System.Linq;

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
            if(string.IsNullOrEmpty(username))
                return ErrorResult;

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

			var state = loginResponse.isTempPasswordSpecified && loginResponse.isTempPassword.Value ? AuthenticateUserResultState.TemporaryPassword : AuthenticateUserResultState.Success;
			var userAccount = Service.Execute(s => s.queryAccountByUsername(username));
			return new SalesforceAuthenticateUserResult
			{
				State = state,
				User = new SalesforceAuthenticatedUser
				{
					Username = username,
					Email = username,
					Name = string.Format("{0} {1}", profile.FirstName, profile.LastName),
					AccountId = userAccount.accounts != null ? userAccount.accounts.Select(x => x.accountId).ToList() : null,
					ContactId = loginResponse.contactId
				}
			};
		}

		public SalesforceAuthenticateUserResult ErrorResult => new SalesforceAuthenticateUserResult
		{
			State = AuthenticateUserResultState.Failure
		};
	}
}
