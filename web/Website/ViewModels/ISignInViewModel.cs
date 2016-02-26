using System.Web;

namespace Informa.Web.ViewModels
{
	public interface ISignInViewModel
	{
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
		string UsernamePlaceholderText { get; }
        string Email { get; }
        string Password { get; }
        bool RememberMe { get; }
	}
}
