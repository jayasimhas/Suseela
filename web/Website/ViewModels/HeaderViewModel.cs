using Informa.Library.User.Authentication;
using Informa.Library.Corporate;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class HeaderViewModel : IHeaderViewModel
	{
		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
		protected readonly ICorporateAccountNameContext CorporateAccountNameContext;
		protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteHomeContext SiteHomeContext;
		protected readonly ISiteRootContext SiteRootContext;

		public HeaderViewModel(
			IAuthenticatedUserContext authenticatedUserContext,
			ICorporateAccountNameContext corporateAccountNameContext,
			ITextTranslator textTranslator,
			ISiteHomeContext siteHomeContext,
			ISiteRootContext siteRootContext)
		{
			AuthenticatedUserContext = authenticatedUserContext;
			CorporateAccountNameContext = corporateAccountNameContext;
			TextTranslator = textTranslator;
			SiteHomeContext = siteHomeContext;
			SiteRootContext = siteRootContext;
		}

		public string LogoImageUrl => SiteRootContext.Item?.Site_Logo?.Src ?? string.Empty;
		public string LogoUrl => SiteHomeContext.Item?._Url ?? string.Empty;
		public string WelcomeText
		{
			get
			{
				var accountName = AuthenticatedUserContext.IsAuthenticated ? AuthenticatedUserContext.User.Name : CorporateAccountNameContext.Name;

				return string.IsNullOrWhiteSpace(accountName) ? string.Empty : string.Concat(TextTranslator.Translate("Header.Greeting"), accountName);
			}
		}

	    public string CookiePolicyText => TextTranslator.Translate("Global.CookiePolicy");
		public bool IsAuthenticated => AuthenticatedUserContext.IsAuthenticated;
		public string MyAccountLinkText => TextTranslator.Translate("Header.MyAccount");
		public string SignOutLinkText => TextTranslator.Translate("Header.SignOut");
		public string RegisterLinkText => TextTranslator.Translate("Header.RegisterLink");
		public string SignInText => TextTranslator.Translate("Header.SignIn");
		public string SignInLinkText => TextTranslator.Translate("Header.SignInLink");
	}
}