using Informa.Library.Globalization;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Web.ViewModels.PopOuts
{
	[AutowireService(LifetimeScope.PerScope)]
	public class SignInPopOutViewModel : ISignInPopOutViewModel
	{
		private readonly ITextTranslator _textTranslator;

		public SignInPopOutViewModel(ITextTranslator textTranslator)
		{
			_textTranslator = textTranslator;
		}

		public string SignInHeaderText => _textTranslator.Translate("Authentication.SignIn.SignInRequired");
		public string HeaderSignInHeaderText => _textTranslator.Translate("Authentication.SignIn.HeadSignInHeader");
	}
}