namespace Informa.Library.User
{
	public interface ISitecoreUserContext
	{
		Sitecore.Security.Accounts.User User { get; }
	}
}
