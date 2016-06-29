namespace Informa.Web.ViewModels.PopOuts
{
	public interface IRegisterPopOutViewModel
	{
		string RegisterText { get; }
		string RegisterButtonText { get; }
		string RegisterUrl { get; }
		string UsernamePlaceholderText { get; }
		string UsernameRequirementsErrorText { get; }
		string UsernamePublicRestrictedDomainErrorText { get; }
		string UsernameCompetitorRestrictedDomainErrorText { get; }
		string UsernameExistsErrorText { get; }
		string GeneralErrorText { get; }
		string Username { get; }
		string RegisterReturnUrl { get; }
		string RegisterReturnUrlKey { get; }
	}
}
