namespace Informa.Web.ViewModels
{
	public interface IHeaderViewModel
	{
		string LogoImageUrl { get; }
		string LogoUrl { get; }

		// Account/Sign In/Registration
		string WelcomeText { get; }
		bool IsAuthenticated { get; }
		string MyAccountLinkText { get; }
		string SignOutLinkText { get; }
		string RegisterLinkText { get; }
		string SignInLinkText { get; }
	}
}
