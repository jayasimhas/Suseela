using System.Collections.Generic;

namespace Informa.Library.User.Authentication
{
    public interface IAuthenticatedUser : IUser
    {
        string UserId { get; set; }
        string Name { get; set; }
        string Email { get; }
        string ContactId { get; }
        IList<string> AccountId { get; }
        string SalesForceSessionId { get; set; }
        string SalesForceURL { get; set; }
    }
}
