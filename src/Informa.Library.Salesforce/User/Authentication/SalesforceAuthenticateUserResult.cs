using Informa.Library.User.Authentication;

namespace Informa.Library.Salesforce.User.Authentication
{
	public class SalesforceAuthenticateUserResult : IAuthenticateUserResult
	{
		public AuthenticateUserResultState State { get; set; }
		public IAuthenticatedUser User { get; set; }

        
	}
}
