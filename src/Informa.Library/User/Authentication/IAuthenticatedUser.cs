using System.Collections.Generic;

namespace Informa.Library.User.Authentication
{
	public interface IAuthenticatedUser : IUser
	{
		string Name { get; }
		string Email { get; }
		string ContactId { get; }
		IList<string> AccountId { get; }
        string SalesForceSessionId { get; set; }
        string SalesForceURL { get; set; }
    }
}
