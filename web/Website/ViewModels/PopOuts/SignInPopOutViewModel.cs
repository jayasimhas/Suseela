using Informa.Library.Globalization;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.PopOuts
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class SignInPopOutViewModel : ISignInPopOutViewModel
    {
        protected readonly ITextTranslator TextTranslator;
        public SignInPopOutViewModel(
            ISignInViewModel signInViewModel, ITextTranslator textTranslator)
        {
            SignInViewModel = signInViewModel;
            TextTranslator = textTranslator;
        }

        public string HeaderText => TextTranslator.Translate("Authentication.SignIn.SignInPopupHeader");
        public ISignInViewModel SignInViewModel { get; set; }
    }
}