namespace Informa.Web.ViewModels.SiteDebugging
{
	public interface IToolbarViewModel
	{
		IUserIpAddressViewModel UserIpAddressViewModel { get; }
		IEntitlementsCheckEnabledViewModel EntitlementsCheckEnabledViewModel { get; }
		bool Enabled { get; }
		string ToggleButtonText { get; }
	}
}