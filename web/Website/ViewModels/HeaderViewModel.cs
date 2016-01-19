using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Web;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class HeaderViewModel : IHeaderViewModel
	{
		protected readonly ISiteHomeContext SiteHomeContext;
		protected readonly ISiteRootContext SiteRootContext;

		public HeaderViewModel(
			ISiteHomeContext siteHomeContext,
			ISiteRootContext siteRootContext)
		{
			SiteHomeContext = siteHomeContext;
			SiteRootContext = siteRootContext;
		}

		public string LogoImageUrl => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Site_Logo.Src;

		public string LogoUrl => SiteHomeContext.Item == null ? string.Empty : SiteHomeContext.Item._Url;

		public string WelcomeText => string.Concat("Hi, ", "#Company or Account Name#");
		public bool IsAuthenticated => false;
		public string MyAccountLinkText => "My Account";
		public string SignOutLinkText => "Sign Out";
		public string RegisterText => "Register";
		public string RegisterLinkText => "Register";
		public string RegisterButtonText => "Register";
		public string UsernamePlaceholderText => "Email Address (User Name)";
		public string UsernameInvalidText => "Invalid Email Address (User Name)";
		public string SignInText => "Sign In";
		public string SignInLinkText => "Sign In";
		public string SignInButtonText => "Sign In";
		public string SignInInvalidText => "Your login and/or password information does not match our records. Please try again.";
		public string PasswordPlaceholderText => "Password";
		public string RememberMeText => "Remember me";
		public string ForgotPasswordText => "Forgot your password?";
		public string ForgotPasswordLinkText => "Forgot your password?";
		public string ForgotPasswordHelpText => "Enter the email address associated with your account and a temporary password will be sent to you.";
		public string ForgotPasswordButtonText => "Submit";
		public string ForgotPasswordConfirmationText => "Thanks, we've sent a link to reset your password";
		public IHtmlString ForgotPasswordContactText => new HtmlString("Need help? Contact us at <b>(800) 332-2181</b>, <b>+1 (908) 748-1221</b>, or <a href=\"#\">custcare@informa.com</a>");
		public string EmailPlaceholderText => "Email Address";
	}
}