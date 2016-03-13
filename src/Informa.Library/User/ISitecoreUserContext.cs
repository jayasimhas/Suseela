using Informa.Library.User.Entitlement;

namespace Informa.Library.User
{
	public interface ISitecoreUserContext : IEntitledVisit
	{
		Sitecore.Security.Accounts.User User { get; }
	}
}
