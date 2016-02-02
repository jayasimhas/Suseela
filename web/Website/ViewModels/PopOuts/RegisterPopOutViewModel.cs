using Informa.Library.Globalization;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.PopOuts
{
	[AutowireService(LifetimeScope.SingleInstance)]
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
		public string UsernameInvalidText => TextTranslator.Translate("Header.UsernameInvalid");
	}
}