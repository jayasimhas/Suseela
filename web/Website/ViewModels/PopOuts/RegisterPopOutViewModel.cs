using Informa.Library.Globalization;
using Informa.Library.Navigation;
using Informa.Library.Site;
using Informa.Library.Wrappers;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Web.ViewModels.PopOuts
{
	[AutowireService(LifetimeScope.PerScope)]
	public class RegisterPopOutViewModel : IRegisterPopOutViewModel
	{
		protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IReturnUrlContext ReturnUrlContext;
		protected readonly IHttpContextProvider HttpContextProvider;

		public RegisterPopOutViewModel(
			ITextTranslator textTranslator,
			ISiteRootContext siteRootContext,
			IReturnUrlContext returnUrlContext,
			IHttpContextProvider httpContextProvider)
		{
			TextTranslator = textTranslator;
			SiteRootContext = siteRootContext;
			ReturnUrlContext = returnUrlContext;
			HttpContextProvider = httpContextProvider;
		}

		public string RegisterText => TextTranslator.Translate("Header.Register");
		public string RegisterButtonText => TextTranslator.Translate("Header.RegisterButton");
		public string RegisterUrl => SiteRootContext.Item?.Register_Link?.Url ?? string.Empty;
		public string UsernamePlaceholderText => TextTranslator.Translate("Header.UsernamePlaceholder");
		public string UsernameRequirementsErrorText => TextTranslator.Translate("Registration.UsernameRequirementsError");
		public string UsernamePublicRestrictedDomainErrorText => TextTranslator.Translate("Registration.UsernameRestrictedPublicDomainError");
		public string UsernameCompetitorRestrictedDomainErrorText => TextTranslator.Translate("Registration.UsernameRestrictedCompetitorDomainError");
		public string UsernameExistsErrorText => TextTranslator.Translate("Registration.UsernameExistsError");
		public string GeneralErrorText => TextTranslator.Translate("Registration.GeneralError");
		public string Username { get; set; }
		public string RegisterReturnUrl => HttpContextProvider.RequestUri.AbsolutePath;
		public string RegisterReturnUrlKey => ReturnUrlContext.Key;
        public string RegisterFreeTrialTitle => TextTranslator.Translate("CallToAction.Register.FreeTrialTitle");
        public string RegisterFreeTrialButton => TextTranslator.Translate("CallToAction.Register.FreeTrialButton");
    }
}