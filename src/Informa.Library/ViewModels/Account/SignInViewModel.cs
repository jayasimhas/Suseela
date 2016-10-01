using Informa.Library.Globalization;
using Informa.Library.Site;
using System.Web;
using Informa.Library.ViewModels.Account;
using Jabberwocky.Autofac.Attributes;
using System;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.PerScope)]
	public class SignInViewModel : ISignInViewModel
	{
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly ITextTranslator TextTranslator;

		public SignInViewModel()
		{
		}

		public SignInViewModel(
				ISiteRootContext siteRootContext,
				ITextTranslator textTranslator)
		{
			SiteRootContext = siteRootContext;
			TextTranslator = textTranslator;
		}

		public string SignInButtonText => TextTranslator.Translate("Authentication.SignIn.Submit");
		public string SignInInvalidText => TextTranslator.Translate("Authentication.SignIn.ErrorInvalid");

		public string SignInPasswordPlaceholderText
				=> TextTranslator.Translate("Authentication.SignIn.PasswordPlaceholder");

		public string SignInRememberMeText => TextTranslator.Translate("Authentication.SignIn.RememberMe");

		public string SignInUsernamePlaceholderText
				=> TextTranslator.Translate("Authentication.SignIn.UsernamePlaceholder");

		public string SignInResetPasswordLinkText => TextTranslator.Translate("Authentication.SignIn.ResetPasswordLink")
				;

		public IHtmlString ResetPasswordBody
				=> new HtmlString(SiteRootContext.Item?.Forgot_Password_Text ?? string.Empty);

		public string ResetPasswordEmailPlaceholderText
				=> TextTranslator.Translate("Authentication.ResetPassword.Request.EmailPlaceholder");

		public string ResetPasswordSuccessText
				=> TextTranslator.Translate("Authentication.ResetPassword.Request.Success");

		public string ResetPasswordErrorEmailText
				=> TextTranslator.Translate("Authentication.ResetPassword.Request.ErrorEmail");

		public string ResetPasswordErrorGeneralText
				=> TextTranslator.Translate("Authentication.ResetPassword.Request.ErrorGeneral");

		public string ResetPasswordSubmitText => TextTranslator.Translate("Authentication.ResetPassword.Request.Submit")
				;

		public IHtmlString ResetPasswordContactText
				=> new HtmlString(SiteRootContext.Item?.Customer_Support_Text ?? string.Empty);

        public bool IsSignInFromMyView { get; set; }
        
    }

}