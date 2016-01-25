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
			ISiteRootContext siteRootContext,
			ISignInViewModel signInViewModel)
		{
			UserAuthenticationContext = userAuthenticationContext;
			CorporateAccountNameContext = corporateAccountNameContext;
			TextTranslator = textTranslator;
			SiteHomeContext = siteHomeContext;
			SiteRootContext = siteRootContext;
			SignInViewModel = signInViewModel;
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
		public ISignInViewModel SignInViewModel { get; set; }
	}
}