namespace Informa.Library.User.Authentication.Web.SiteDebugging
{
	public interface ISiteDebuggingWebLoginUser
	{
		bool IsDebugging { get; }

		void StartDebugging(string username);
		void StopDebugging();
	}
}