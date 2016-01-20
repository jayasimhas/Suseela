using System.Web;

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
		string RegisterText { get; }
		string RegisterLinkText { get; }
		string RegisterButtonText { get; }
		string RegisterUrl { get; }
		string UsernamePlaceholderText { get; }
		string UsernameInvalidText { get; }
		string SignInText { get; }
		string SignInLinkText { get; }
		string SignInButtonText { get; }
		string SignInInvalidText { get; }
		string PasswordPlaceholderText { get; }
		string RememberMeText { get; }
		string ForgotPasswordText { get; }
		string ForgotPasswordLinkText { get; }
		string ForgotPasswordHelpText { get; }
		string ForgotPasswordButtonText { get; }
		string ForgotPasswordConfirmationText { get; }
		IHtmlString ForgotPasswordContactText { get; }
		string EmailPlaceholderText { get; }
	}
}
