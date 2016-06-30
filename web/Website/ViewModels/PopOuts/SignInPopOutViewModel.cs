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

		public string HeaderText => _textTranslator.Translate("Authentication.SignIn.SignInRequired");
        public string SocialHeaderText => _textTranslator.Translate("Authentication.SignIn.SocialSignInPopupHeader");
    }
}