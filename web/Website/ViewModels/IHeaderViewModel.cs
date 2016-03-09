namespace Informa.Web.ViewModels
{
	public interface IHeaderViewModel
	{
		string LogoImageUrl { get; }
		string LogoUrl { get; }

		// Account/Sign In/Registration
		string WelcomeText { get; }
        string PolicyText { get; }
        bool IsAuthenticated { get; }
		string MyAccountLinkText { get; }
        string MyAccountLink { get; }
        string SignOutLinkText { get; }
		string RegisterLinkText { get; }
		string SignInLinkText { get; }
	}
}
