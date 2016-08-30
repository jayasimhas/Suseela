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

		public string InnerPagesSignInHeaderText => _textTranslator.Translate("Authentication.SignIn.InnerPagesSignInHeaderText");
        public string TopHeaderSignInHeaderText => _textTranslator.Translate("Authentication.SignIn.TopHeaderSignInHeaderText");
    }
}