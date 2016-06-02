using Informa.Library.Globalization;
using Informa.Library.ViewModels.Account;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.PopOuts
{
	[AutowireService(LifetimeScope.PerScope)]
	public class SignInPopOutViewModel : ISignInPopOutViewModel
	{
	    private readonly ITextTranslator TextTranslator;

		public SignInPopOutViewModel(ITextTranslator textTranslator)
		{
		    TextTranslator = textTranslator;
		}

		public string HeaderText => TextTranslator.Translate("Authentication.SignIn.SignInRequired");
	}
}