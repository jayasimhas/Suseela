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

		public string LogoImageUrl => SiteRootContext.Item == null || SiteRootContext.Item.Site_Logo == null ? string.Empty : SiteRootContext.Item.Site_Logo.Src;

		public string LogoUrl => SiteHomeContext.Item == null ? string.Empty : SiteHomeContext.Item._Url;

		public string WelcomeText
		{
			get
			{
				var accountName = UserAuthenticationContext.IsAuthenticated ? HttpContext.Current.User.Identity.Name : CorporateAccountNameContext.Name;

				return string.IsNullOrWhiteSpace(accountName) ? string.Empty : string.Concat(TextTranslator.Translate("Header.Greeting"), accountName);
			}
		}
		public bool IsAuthenticated => UserAuthenticationContext.IsAuthenticated;
		public string MyAccountLinkText => TextTranslator.Translate("Header.MyAccount");
		public string SignOutLinkText => TextTranslator.Translate("Header.SignOut");
		public string RegisterText => TextTranslator.Translate("Header.Register");
		public string RegisterLinkText => TextTranslator.Translate("Header.RegisterLink");
		public string RegisterButtonText => TextTranslator.Translate("Header.RegisterButton");
		public string RegisterUrl => SiteRootContext.Item == null || SiteRootContext.Item.Register_Link == null ? string.Empty : SiteRootContext.Item.Register_Link.Url;
		public string UsernamePlaceholderText => TextTranslator.Translate("Header.UsernamePlaceholder");
		public string UsernameInvalidText => TextTranslator.Translate("Header.UsernameInvalid");
		public string SignInText => TextTranslator.Translate("Header.SignIn");
		public string SignInLinkText => TextTranslator.Translate("Header.SignInLink");
		public string SignInButtonText => TextTranslator.Translate("Header.SignInLink");
		public string SignInInvalidText => TextTranslator.Translate("Header.SignInInvalid");
		public string PasswordPlaceholderText => TextTranslator.Translate("Header.PasswordPlaceholder");
		public string RememberMeText => TextTranslator.Translate("Header.RememberMe");
		public string ForgotPasswordText => TextTranslator.Translate("Header.ForgotPassword");
		public string ForgotPasswordLinkText => TextTranslator.Translate("Header.ForgotPasswordLink");
		public string ForgotPasswordHelpText => TextTranslator.Translate("Header.ForgotPasswordHelp");
		public string ForgotPasswordButtonText => TextTranslator.Translate("Header.ForgotPasswordButton");
		public string ForgotPasswordConfirmationText => TextTranslator.Translate("Header.ForgotPasswordConfirmation");
		public IHtmlString ForgotPasswordContactText => new HtmlString("Need help? Contact us at <b>(800) 332-2181</b>, <b>+1 (908) 748-1221</b>, or <a href=\"#\">custcare@informa.com</a>");
		public string EmailPlaceholderText => TextTranslator.Translate("Header.EmailPlaceholder");
	}
}