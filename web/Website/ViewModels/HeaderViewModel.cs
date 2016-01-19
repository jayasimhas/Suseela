using Informa.Library.Authentication;
using Informa.Library.Corporate;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Web;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class HeaderViewModel : IHeaderViewModel
	{
		protected readonly IUserAuthenticationContext UserAuthenticationContext;
		protected readonly ICorporateAccountNameContext CorporateAccountNameContext;
		protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteHomeContext SiteHomeContext;
		protected readonly ISiteRootContext SiteRootContext;

		public HeaderViewModel(
			IUserAuthenticationContext userAuthenticationContext,
			ICorporateAccountNameContext corporateAccountNameContext,
			ITextTranslator textTranslator,
			ISiteHomeContext siteHomeContext,
			ISiteRootContext siteRootContext)
		{
			UserAuthenticationContext = userAuthenticationContext;
			CorporateAccountNameContext = corporateAccountNameContext;
			TextTranslator = textTranslator;
			SiteHomeContext = siteHomeContext;
			SiteRootContext = siteRootContext;
		}

		public string LogoImageUrl => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Site_Logo.Src;

		public string LogoUrl => SiteHomeContext.Item == null ? string.Empty : SiteHomeContext.Item._Url;

		public string WelcomeText
		{
			get
			{
				var accountName = UserAuthenticationContext.IsAuthenticated ? HttpContext.Current.User.Identity.Name : CorporateAccountNameContext.Name;

				return string.IsNullOrWhiteSpace(accountName) ? string.Empty : string.Concat(TextTranslator.Translate("Greeting"), accountName); //Greeting: "Hi, "
			}
		}
		public bool IsAuthenticated => UserAuthenticationContext.IsAuthenticated;
		public string MyAccountLinkText => TextTranslator.Translate("My Account");
		public string SignOutLinkText => TextTranslator.Translate("Sign Out");
		public string RegisterText => TextTranslator.Translate("Register");
		public string RegisterLinkText => TextTranslator.Translate("Register Link");
		public string RegisterButtonText => TextTranslator.Translate("Register Button");
		public string UsernamePlaceholderText => TextTranslator.Translate("Username Placeholder"); //"Email Address (User Name)"
		public string UsernameInvalidText => TextTranslator.Translate("Username Invalid"); //"Invalid Email Address (User Name)"
		public string SignInText => TextTranslator.Translate("Sign In");
		public string SignInLinkText => TextTranslator.Translate("Sign In Link");
		public string SignInButtonText => TextTranslator.Translate("Sign In Link");
		public string SignInInvalidText => TextTranslator.Translate("Sign In Invalid"); //"Your login and/or password information does not match our records. Please try again."
		public string PasswordPlaceholderText => TextTranslator.Translate("Password Placeholder");
		public string RememberMeText => TextTranslator.Translate("Remember Me");
		public string ForgotPasswordText => TextTranslator.Translate("Forgot Password"); //"Forgot your password?"
		public string ForgotPasswordLinkText => TextTranslator.Translate("Forgot Password Link"); //"Forgot your password?"
		public string ForgotPasswordHelpText => TextTranslator.Translate("Forgot Password Help"); //"Enter the email address associated with your account and a temporary password will be sent to you."
		public string ForgotPasswordButtonText => TextTranslator.Translate("Forgot Password Button"); //Submit
		public string ForgotPasswordConfirmationText => TextTranslator.Translate("Forgot Password Confirmation"); //"Thanks, we've sent a link to reset your password"
		public IHtmlString ForgotPasswordContactText => new HtmlString("Need help? Contact us at <b>(800) 332-2181</b>, <b>+1 (908) 748-1221</b>, or <a href=\"#\">custcare@informa.com</a>");
		public string EmailPlaceholderText => TextTranslator.Translate("Email Placeholder"); //"Email Address"
	}
}