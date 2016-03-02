using Informa.Library.Globalization;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Web;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SignInViewModel : ISignInViewModel
	{
		protected readonly ITextTranslator TextTranslator;

        public SignInViewModel() { }

		public SignInViewModel(
			ITextTranslator textTranslator)
		{
			TextTranslator = textTranslator;
		}

		public string SignInButtonText => TextTranslator.Translate("Authentication.SignIn.Submit");
		public string SignInInvalidText => TextTranslator.Translate("Authentication.SignIn.ErrorInvalid");
		public string SignInPasswordPlaceholderText => TextTranslator.Translate("Authentication.SignIn.PasswordPlaceholder");
		public string SignInRememberMeText => TextTranslator.Translate("Authentication.SignIn.RememberMe");
		public string SignInUsernamePlaceholderText => TextTranslator.Translate("Authentication.SignIn.UsernamePlaceholder");
		public string SignInResetPasswordLinkText => TextTranslator.Translate("Authentication.SignIn.ResetPasswordLink");
		public string ResetPasswordEmailPlaceholderText => TextTranslator.Translate("Authentication.ResetPassword.Request.EmailPlaceholder");
		public string ResetPasswordSuccessText => TextTranslator.Translate("Authentication.ResetPassword.Request.Success");
		public string ResetPasswordErrorEmailText => TextTranslator.Translate("Authentication.ResetPassword.Request.ErrorEmail");
		public string ResetPasswordErrorGeneralText => TextTranslator.Translate("Authentication.ResetPassword.Request.ErrorGeneral");
		public string ResetPasswordText => TextTranslator.Translate("Authentication.ResetPassword.Request.Header");
		public string ResetPasswordHelpText => TextTranslator.Translate("Authentication.ResetPassword.Request.Help");
		public string ResetPasswordSubmitText => TextTranslator.Translate("Authentication.ResetPassword.Request.Submit");
		public IHtmlString ResetPasswordContactText => new HtmlString(TextTranslator.Translate("Authentication.ResetPassword.Request.Contact"));
	}

    
}