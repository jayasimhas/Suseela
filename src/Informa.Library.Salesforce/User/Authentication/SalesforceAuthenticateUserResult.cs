using Informa.Library.User.Authentication;
using Informa.Library.User;

namespace Informa.Library.Salesforce.User.Authentication
{
	public class SalesforceAuthenticateUserResult : IAuthenticateUserResult
	{
		public AuthenticateUserResultState State { get; set; }
		public IUser User { get; set; }
	}
}
