namespace Informa.Web.ViewModels.PopOuts
{
	public interface ISignInPopOutViewModel
	{
		string HeaderText { get; }
        string SocialHeaderText { get; }
		ISignInViewModel SignInViewModel { get; }
	}
}
