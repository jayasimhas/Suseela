using System.Web;

namespace Informa.Web.ViewModels
{
	public interface ISignInViewModel
	{
		string SignInButtonText { get; }
		string SignInInvalidText { get; }
		string SignInPasswordPlaceholderText { get; }
		string SignInRememberMeText { get; }
		string SignInUsernamePlaceholderText { get; }
		string SignInResetPasswordLinkText { get; }
		string ResetPasswordEmailPlaceholderText { get; }
		string ResetPasswordSuccessText { get; }
		string ResetPasswordErrorEmailText { get; }
		string ResetPasswordErrorGeneralText { get; }
		string ResetPasswordText { get; }
		string ResetPasswordHelpText { get; }
		string ResetPasswordSubmitText { get; }
		IHtmlString ResetPasswordContactText { get; }
	}
}
