using Informa.Library.ViewModels.Account;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.PopOuts
{
	[AutowireService(LifetimeScope.PerScope)]
	public class SignInPopOutViewModel : ISignInPopOutViewModel
	{
		public SignInPopOutViewModel(
			ISignInViewModel signInViewModel)
		{
			SignInViewModel = signInViewModel;
		}

		public string HeaderText => "You must sign in to use this functionality";
		public ISignInViewModel SignInViewModel { get; set; }
	}
}