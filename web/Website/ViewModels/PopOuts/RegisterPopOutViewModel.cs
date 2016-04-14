using Informa.Library.Globalization;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.PopOuts
{
	[AutowireService(LifetimeScope.PerScope)]
	public class RegisterPopOutViewModel : IRegisterPopOutViewModel
	{
		protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteRootContext SiteRootContext;

		public RegisterPopOutViewModel(
			ITextTranslator textTranslator,
			ISiteRootContext siteRootContext)
		{
			TextTranslator = textTranslator;
			SiteRootContext = siteRootContext;
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
	}
}